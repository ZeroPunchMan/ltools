using System.Windows.Forms;
using System.IO;
using System;

namespace SgpProtocol
{
    //����ֻ���������ݴ����ط�,û�п��ǳ�ʱ�ط�
    public class DfuSender
    {
        const int binBytesOnce = 200;    //һ�η���bin�ļ��ֽ���
        FileStream binFileStream = null;       //��ȡ�ļ�
        readonly int maxFileSize = 500 * 1024; //�ļ����ߴ�
        int binFileSize = 0;
        Parser.SendDataMethod sendMethod = null;
        LogMethod logMethod = null;

        public delegate void LogMethod(string log);

        public DfuSender(string filePath, Parser.SendDataMethod sendMethod, LogMethod logMethod)
        {
            FileInfo info = new FileInfo(filePath);
            binFileSize = (int)info.Length;

            if (binFileSize > maxFileSize)
                throw new Exception("File too large");

            binFileStream = File.OpenRead(filePath);

            this.sendMethod = sendMethod;
            this.logMethod = logMethod;
            EventManager.AddListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, this.OnDevReady);
            EventManager.AddListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, this.OnBinDataResult);
            EventManager.AddListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, this.OnUpgradeResult);
            StatusToWaitReady();
        }

        ~DfuSender()
        {
            //�˶��¼�
            EventManager.RemoveListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, this.OnDevReady);
            EventManager.RemoveListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, this.OnBinDataResult);
            EventManager.RemoveListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, this.OnUpgradeResult);

            if (binFileStream != null)
            {
                binFileStream.Close();
                binFileStream = null;
            }
        }

        int timeoutCount = 0;
        bool TimeoutCheck(int interval)
        {
            timeoutCount += interval;
            if (timeoutCount > 30000)
            {
                return true;
            }
            return false;
        }

        void ResetTimeout()
        {
            timeoutCount = 0;
        }

        DateTime lastUpdateTime = DateTime.Now;
        public int Update(int interval)
        {
            TimeSpan span = DateTime.Now - lastUpdateTime;

            if(TimeoutCheck((int)span.TotalMilliseconds)) //��ʱ���
            { //todo ��ʱ�ط�
                
            }
            switch (dfuStatus)
            {
                case DfuStatus.WaitReady:
                    return 0;
                case DfuStatus.BinSending:
                    float percent = totalBytes / binFileSize;
                    percent = Helper.Math.Clamp<float>(percent, 1, 99);
                    return (int)percent;
                case DfuStatus.WaitResult:
                    return 99;
                case DfuStatus.Done:
                    return 100;
                default:
                    return -1;
            }
        }

        void SendUpgradeReq()
        {
            byte[] data = new byte[4];
            data[0] = (byte)((binFileSize >> 24) & 0xff);
            data[1] = (byte)((binFileSize >> 16) & 0xff);
            data[2] = (byte)((binFileSize >> 8) & 0xff);
            data[3] = (byte)((binFileSize >> 0) & 0xff);
            Parser.SendPacket(this.sendMethod, SgpCmd.Dfu, SgpSubCmd.DfuReq, data, 0, data.Length);
            //Console.WriteLine("send upgrade request");
            totalBytes = 0;
        }

        byte[] binData = new byte[2 + binBytesOnce]; //2�ֽ����ݰ�����,����������
        int sendBytes = 0;          //�˴η���������
        UInt16 sendBinPackCount = 0;    //���ݰ�����
        UInt32 binFileCRC = 0;  //����bin�ļ���crc
        int totalBytes = 0;
        //��ȡ�µ����ݷ���,����true�����ļ���β
        bool SendBinData()
        {
            //2�ֽ����ݰ�����
            binData[0] = (byte)(sendBinPackCount >> 8);
            binData[1] = (byte)(sendBinPackCount & 0xff);
            int readBytes = binFileStream.Read(binData, 2, binBytesOnce);
            if (readBytes > 0)
            {
                sendBytes = readBytes + 2;
                Parser.SendPacket(this.sendMethod, SgpCmd.Dfu, SgpSubCmd.DfuData, binData, 0, sendBytes);
                //Console.WriteLine("send " + sendBinPackCount);
                sendBinPackCount++; //����+1
                binFileCRC = FastCrc.CrcCalc.EthernetCrc32(binFileCRC, binData, 2, readBytes); //�ۼ�crc
                totalBytes += readBytes;
                return false;
            }
            else
            {
                return true;
            }
        }

        void SendBinVerify(UInt32 hash)
        {
            byte[] data = new byte[4];
            data[0] = (byte)((hash >> 24) & 0xff);
            data[1] = (byte)((hash >> 16) & 0xff);
            data[2] = (byte)((hash >> 8) & 0xff);
            data[3] = (byte)((hash >> 0) & 0xff);

            this.logMethod(string.Format("crc: {0:x}", hash));
            Parser.SendPacket(this.sendMethod, SgpCmd.Dfu, SgpSubCmd.DfuVerify, data, 0, data.Length);
            //Console.WriteLine("send verify: {0:X}", hash);
        }

        enum DfuStatus
        {
            WaitReady,
            BinSending,
            WaitResult,
            Done,
            Error,
        }

        DfuStatus dfuStatus = DfuStatus.WaitReady; //�̼�����״̬
        //״̬�л�����ready
        void StatusToWaitReady()
        {
            //������������
            SendUpgradeReq();
            //�л�״̬
            dfuStatus = DfuStatus.WaitReady;
            ResetTimeout();

        }

        //״̬�л�������bin�ļ�
        void StatusToSending()
        {
            sendBinPackCount = 0; //���ü���
            binFileCRC = 0;     //����crc
            SendBinData();      //��ʼ�����ļ�

            //�л�״̬
            dfuStatus = DfuStatus.BinSending;
            ResetTimeout();
            this.logMethod("��ʼ����");
        }

        //״̬�л����ȴ����
        void StatusToWaitResult()
        {
            dfuStatus = DfuStatus.WaitResult;
            ResetTimeout();

            SendBinVerify(binFileCRC);
        }

        void StatusToDone()
        {
            dfuStatus = DfuStatus.Done;
        }

        void StatusToError()
        {
            dfuStatus = DfuStatus.Error;
        }

        //�豸�ظ�ready
        void OnDevReady(System.Object sender, System.Object eventArg)
        {
            SgpPacket pack = eventArg as SgpPacket;
            if (pack == null)
                return;

            if (pack.subCmd != SgpSubCmd.DfuReady)
                return;

            if (dfuStatus == DfuStatus.WaitReady)
            {
                //Console.WriteLine("device is ready");
                StatusToSending();
            }
        }

        //�豸����bin�ļ����ݽ��
        void OnBinDataResult(System.Object sender, System.Object eventArg)
        {
            SgpPacket pack = eventArg as SgpPacket;
            if (pack == null)
                return;
            if (pack.subCmd != SgpSubCmd.DfuDataRsp)
                return;

            if (dfuStatus == DfuStatus.BinSending)
            {   //�ڷ���״̬
                //data[0]��data[1]Ϊ������,data[2]Ϊ���,1��ʾ��ȷ
                if (pack.data != null && pack.data.Length >= 3)
                {
                    UInt16 packCount = (UInt16)((UInt16)pack.data[0] << 8);
                    packCount |= (UInt16)pack.data[1];
                    if ((packCount == sendBinPackCount - 1) && pack.data[2] == 1)
                    {   //�յ������ݰ���ȷ
                        this.logMethod(string.Format("pack {0} ok", packCount));
                        ResetTimeout(); //���㳬ʱ
                        if (SendBinData()) //��������һ��
                        {   //�����ļ���β
                            StatusToWaitResult();
                        }
                        //result ok
                        return;
                    }
                }
            }
        }

        //�豸�����������
        void OnUpgradeResult(System.Object sender, System.Object eventArg)
        {
            SgpPacket pack = eventArg as SgpPacket;
            if (pack == null)
                return;
            if (pack.subCmd != SgpSubCmd.DfuVerifyRsp)
                return;

            //data[0]Ϊ���,1��ʾ�ɹ�,0��ʾʧ��
            if (pack.data != null && pack.data.Length >= 1)
            {
                Stop();
                if (pack.data[0] == 1)
                {   //���ؽ���ɹ�,֪ͨ�����ɹ�
                    this.logMethod("�̼������ɹ�!");
                    StatusToDone();
                }
                else
                {
                    this.logMethod("�̼�����ʧ��..");
                    StatusToError();
                }
            }
        }

    }
}