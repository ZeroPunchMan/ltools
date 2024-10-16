using System.Windows.Forms;
using System.IO;
using System;

namespace SgpProtocol
{
    //这里只考虑了数据错误重发,没有考虑超时重发
    public class DfuSender
    {
        const int binBytesOnce = 200;    //一次发送bin文件字节数
        FileStream binFileStream = null;       //读取文件
        readonly int maxFileSize = 500 * 1024; //文件最大尺寸
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
            //退订事件
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

            if (TimeoutCheck((int)span.TotalMilliseconds)) //超时监测
            { //todo 超时重发

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

        byte[] binData = new byte[2 + binBytesOnce]; //2字节数据包计数,后面是数据
        int sendBytes = 0;          //此次发送数据量
        UInt16 sendBinPackCount = 0;    //数据包计数
        int totalBytes = 0;
        //读取新的数据发送,返回true到达文件结尾
        bool SendBinData()
        {
            //2字节数据包计数
            binData[0] = (byte)(sendBinPackCount >> 8);
            binData[1] = (byte)(sendBinPackCount & 0xff);
            int readBytes = binFileStream.Read(binData, 2, binBytesOnce);
            if (readBytes > 0)
            {
                sendBytes = readBytes + 2;
                Parser.SendPacket(this.sendMethod, SgpCmd.Dfu, SgpSubCmd.DfuData, binData, 0, sendBytes);
                //Console.WriteLine("send " + sendBinPackCount);
                sendBinPackCount++; //计数+1
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

        DfuStatus dfuStatus = DfuStatus.WaitReady; //固件升级状态
        //状态切换到等ready
        void StatusToWaitReady()
        {
            //发送升级请求
            SendUpgradeReq();
            //切换状态
            dfuStatus = DfuStatus.WaitReady;
            ResetTimeout();

        }

        //状态切换到发送bin文件
        void StatusToSending()
        {
            sendBinPackCount = 0; //重置计数
            SendBinData();      //开始发送文件

            //切换状态
            dfuStatus = DfuStatus.BinSending;
            ResetTimeout();
            this.logMethod("开始发送");
        }

        //状态切换到等待结果
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

        //设备回复ready
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

        //设备返回bin文件数据结果
        void OnBinDataResult(System.Object sender, System.Object eventArg)
        {
            SgpPacket pack = eventArg as SgpPacket;
            if (pack == null)
                return;
            if (pack.subCmd != SgpSubCmd.DfuDataRsp)
                return;

            if (dfuStatus == DfuStatus.BinSending)
            {   //在发送状态
                //data[0]和data[1]为包计数,data[2]为结果,1表示正确
                if (pack.data != null && pack.data.Length >= 3)
                {
                    UInt16 packCount = (UInt16)((UInt16)pack.data[0] << 8);
                    packCount |= (UInt16)pack.data[1];
                    if ((packCount == sendBinPackCount - 1) && pack.data[2] == 1)
                    {   //收到了数据包正确
                        this.logMethod(string.Format("pack {0} ok", packCount));
                        ResetTimeout(); //清零超时
                        if (SendBinData()) //继续发下一包
                        {   //到达文件结尾
                            StatusToWaitResult();
                        }
                        //result ok
                        return;
                    }
                }
            }
        }

        //设备返回升级结果
        void OnUpgradeResult(System.Object sender, System.Object eventArg)
        {
            SgpPacket pack = eventArg as SgpPacket;
            if (pack == null)
                return;
            if (pack.subCmd != SgpSubCmd.DfuVerifyRsp)
                return;

            //data[0]为结果,1表示成功,0表示失败
            if (pack.data != null && pack.data.Length >= 1)
            {
                if (pack.data[0] == 1)
                {   //返回结果成功,通知升级成功
                    this.logMethod("固件升级成功!");
                    StatusToDone();
                }
                else
                {
                    this.logMethod("固件升级失败..");
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