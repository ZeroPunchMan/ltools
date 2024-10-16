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
        byte[] signature = null;
        public DfuSender(string filePath, string signPath, Parser.SendDataMethod sendMethod, LogMethod logMethod)
        {
            FileInfo info = new FileInfo(filePath);
            binFileSize = (int)info.Length;

            if (binFileSize > maxFileSize)
                throw new Exception("File too large");

            binFileStream = File.OpenRead(filePath);

            info = new FileInfo(signPath);
            if (info.Length != 64)
                throw new Exception("Invalid signature");

            signature = File.ReadAllBytes(signPath);

            this.sendMethod = sendMethod;
            this.logMethod = logMethod;
            EventManager.AddListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, this.OnDevReady);
            EventManager.AddListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, this.OnBinDataResult);
            EventManager.AddListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, this.OnUpgradeResult);
            EventManager.AddListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, this.OnRecvError);
            StatusToWaitReady();
        }

        ~DfuSender()
        {
            if (binFileStream != null)
            {
                binFileStream.Close();
                binFileStream = null;
            }
        }

        public void Dispose()
        {
            //�˶��¼�
            EventManager.RemoveListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, this.OnDevReady);
            EventManager.RemoveListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, this.OnBinDataResult);
            EventManager.RemoveListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, this.OnUpgradeResult);
            EventManager.RemoveListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, this.OnRecvError);
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
        public int Update()
        {
            TimeSpan span = DateTime.Now - lastUpdateTime;

            if (TimeoutCheck((int)span.TotalMilliseconds)) //��ʱ���
            { //todo ��ʱ�ط�

            }
            switch (dfuStatus)
            {
                case DfuStatus.WaitReady:
                    return 0;
                case DfuStatus.BinSending:
                    float percent = totalBytes / (float)binFileSize;
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
            Helper.Serialize.Uint32ToBytes((UInt32)binFileSize, data, 0, Helper.Serialize.Endian.Big);
            Parser.SendPacket(this.sendMethod, SgpCmd.Dfu, SgpSubCmd.DfuReq, data, 0, data.Length);
            //Console.WriteLine("send upgrade request");
            totalBytes = 0;
        }

        byte[] binData = new byte[2 + binBytesOnce]; //2�ֽ����ݰ�����,����������
        int sendBytes = 0;          //�˴η���������
        UInt16 sendBinPackCount = 0;    //���ݰ�����
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
                totalBytes += readBytes;
                return false;
            }
            else
            {
                return true;
            }
        }

        void SendBinVerify(byte[] sig)
        {
            Parser.SendPacket(this.sendMethod, SgpCmd.Dfu, SgpSubCmd.DfuVerify, sig, 0, sig.Length);
            this.logMethod("send verify");
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

            SendBinVerify(signature);
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
        void OnRecvError(System.Object sender, System.Object eventArg)
        {
            SgpPacket pack = eventArg as SgpPacket;
            if (pack == null)
                return;

            if (pack.subCmd != SgpSubCmd.DfuError)
                return;

            StatusToError();
        }

        
    }
}