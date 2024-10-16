using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO.Ports;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using System.Reflection;
using System;

namespace SgpProtocol
{
    using FastCrc;
    public enum SgpCmd
    {
        Dfu = 0x01,
    }

    public enum SgpSubCmd
    {
        DfuReq = 0x70,
        DfuData = 0x71,
        DfuVerify = 0x72,
        DfuBootVer = 0x73,
        AppVer = 0x74,

        DfuReady = 0x70 | 0x80,
        DfuDataRsp = 0x71 | 0x80,
        DfuVerifyRsp = 0x72 | 0x80,
        DfuBootVerRsp = 0x73 | 0x80,
        AppVerRsp = 0x74 | 0x80,

        DfuTest = 0x7e | 0x80,
        DfuError = 0x7f | 0x80,
    }

    public class SgpPacket
    {
        public SgpCmd cmd;
        public SgpSubCmd subCmd;
        public byte[] data;
    }

    public class Parser
    {
        static readonly byte[] protoHead = { 0xfe, 0xef };

        public delegate void SendDataMethod(byte[] data, int offset, int count);
        //data长度不要超过255
        public static void SendPacket(SendDataMethod sendMethod, SgpCmd msgType, SgpSubCmd subCmd, byte[] data, int offset, int count)
        {
            if (count < 0 || count > 255)
                return;
            //帧头
            sendMethod(protoHead, 0, protoHead.Length);
            //命令+长度
            byte[] mt = new byte[3];
            mt[0] = (byte)msgType;
            mt[1] = (byte)subCmd;
            mt[2] = (byte)count;
            sendMethod(mt, 0, mt.Length);
            //Console.WriteLine(BitConverter.ToString(mt));
            //计算命令和长度crc
            UInt16 crc = 0;
            crc = CrcCalc.ModbusCrc16(mt, 0, (UInt16)mt.Length);

            //发送数据
            if (count > 0)
            {
                sendMethod(data, offset, count);
                //Console.WriteLine(BitConverter.ToString(data));

                //计算数据段CRC
                crc = CrcCalc.ModbusCrc16(crc, data, offset, count);
            }

            mt[0] = (byte)(crc >> 8);
            mt[1] = (byte)(crc & 0xff);

            //发送crc
            sendMethod(mt, 0, 2);
            //Console.WriteLine(BitConverter.ToString(mt));
        }

        enum ParseStatus
        {
            Head,       //2 bytes
            Cmd,        //1 byte
            SubCmd,    //1 byte
            Length,     //1 byte
            Data,       //n bytes
            Verify,     //2 byte
        }
        ParseStatus parseStatus = ParseStatus.Head;

        SgpPacket recvPacket = new SgpPacket();


        DateTime lastRecvTime = DateTime.Now;
        public void ParseData(byte[] data)
        {
            TimeSpan span = DateTime.Now - lastRecvTime;
            if(span.TotalMilliseconds >= 500)
            {
                ToParseHead();
            }
            lastRecvTime = DateTime.Now;

            foreach (byte c in data)
            {
                switch (parseStatus)
                {
                    case ParseStatus.Head:
                        if (c == protoHead[headCount])
                        {
                            headCount++;
                            if (headCount >= protoHead.Length)
                            {
                                ToParseCmd();
                            }
                        }
                        else
                        {
                            headCount = 0;
                        }
                        break;
                    case ParseStatus.Cmd:
                        recvPacket.cmd = (SgpCmd)c;
                        ToParseSubCmd();
                        break;
                    case ParseStatus.SubCmd:
                        recvPacket.subCmd = (SgpSubCmd)c;
                        ToParseLength();
                        break;
                    case ParseStatus.Length:
                        if (c == 0)
                        {   //长度为0 没有数据
                            recvPacket.data = null;
                            ToParseVerify();   //直接接收校验
                        }
                        else
                        {   //长度不是0,有数据
                            recvPacket.data = new byte[c];
                            ToParseData();  //开始接收数据
                        }
                        break;
                    case ParseStatus.Data:
                        recvPacket.data[recvDataCount++] = c;
                        if (recvDataCount >= recvPacket.data.Length)
                        {   //数据接收完成
                            ToParseVerify();
                        }
                        break;
                    case ParseStatus.Verify:
                        VeriyProcess(c);
                        break;
                    default:
                        break;
                }
            }
        }

        UInt16 headCount = 0;
        void ToParseHead()
        {
            parseStatus = ParseStatus.Head;
            headCount = 0;
        }

        void ToParseCmd()
        {
            parseStatus = ParseStatus.Cmd;
        }
        void ToParseSubCmd()
        {
            parseStatus = ParseStatus.SubCmd;
        }

        void ToParseLength()
        {
            parseStatus = ParseStatus.Length;
        }

        int recvDataCount = 0;
        void ToParseData()
        {
            parseStatus = ParseStatus.Data;
            recvDataCount = 0;
        }

        int verifyCount = 0;
        void ToParseVerify()
        {
            parseStatus = ParseStatus.Verify;
            verifyCount = 0;
        }

        UInt16 verifyValue;
        void VeriyProcess(byte c)
        {
            switch (verifyCount)
            {
                case 0:
                    verifyValue = (UInt16)(((UInt16)c) << 8);  //第一个字节 
                    break;
                case 1:
                    verifyValue |= (UInt16)c;  //第二个字节
                    byte[] mt = new byte[3];
                    mt[0] = (byte)recvPacket.cmd;
                    mt[1] = (byte)recvPacket.subCmd;
                    if (recvPacket.data != null)
                        mt[2] = (byte)recvPacket.data.Length;
                    else
                        mt[2] = 0;

                    UInt16 crc = CrcCalc.ModbusCrc16(mt, 0, mt.Length); //msg + length校验

                    if (recvPacket.data != null)
                    {   //如果有数据,加上数据校验
                        crc = CrcCalc.ModbusCrc16(crc, recvPacket.data, 0, recvPacket.data.Length);
                    }

                    if (crc == verifyValue)
                    {   //收到数据正确,发起事件
                        //Console.Write("recv msg: " + Enum.GetName(typeof(DevMsgType), recvMsg.msgType));
                        //if(recvMsg.data != null)
                        //{
                        //    foreach (byte b in recvMsg.data)
                        //    {
                        //        Console.Write("{0:x2}, ", b);
                        //    }
                        //}
                        //Console.WriteLine("");

                        EventManager.RaiseEvent(EventId.ProtocolRecvMsg, (int)recvPacket.cmd, this, recvPacket);
                        ToParseHead(); //切换到等待帧头

                    }
                    else
                    {   //收到数据错误
                        ToParseHead(); //切换到等待帧头
                        //Console.WriteLine("msg verify error");
                    }
                    break;
                default:
                    break;
            }

            verifyCount++;
        }
    }
}