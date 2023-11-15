using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO.Ports;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using SgpProtocol;

namespace DfuTool
{
    public partial class Form1 : Form
    {
        private void CommInit()
        {
            EventManager.AddListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, OnRecvVersion);
        }

        private void CommExit()
        {
            EventManager.RemoveListener(EventId.ProtocolRecvMsg, (int)SgpCmd.Dfu, OnRecvVersion);
        }

        void OnRecvVersion(System.Object sender, System.Object eventArg)
        {
            SgpPacket msg = eventArg as SgpPacket;
            //bootloader版本
            if (msg.cmd != SgpCmd.Dfu || (msg.subCmd != SgpSubCmd.DfuBootVerRsp && msg.subCmd != SgpSubCmd.AppVerRsp))
                return;

            if (msg.data != null && msg.data.Length == 14)    //版本号长度,硬编码了
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(System.Text.Encoding.ASCII.GetString(msg.data, 0, 10));    //版本号head
                sb.Append(": ");
                sb.Append(msg.data[10].ToString());  //主版本号
                sb.Append(".");
                sb.Append(msg.data[11].ToString());  //次版本号
                sb.Append(".");
                //修订版本号
                UInt16 revision = (UInt16)(msg.data[12] << 8);
                revision |= msg.data[13];
                sb.Append(revision.ToString());

                string s = sb.ToString();

                if (msg.subCmd == SgpSubCmd.DfuBootVerRsp)
                    DebugLog("BOOT版本 " + s);
                else if (msg.subCmd == SgpSubCmd.AppVerRsp)
                    DebugLog("APP版本 " + s);
            }
        }


        private DateTime ConvertStringToDateTime(UInt32 timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = TimeSpan.FromSeconds(timeStamp);
            return dtStart.Add(toNow);
        }


        UInt32 GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            UInt32 timestamp = (UInt32)ts.TotalSeconds;
            return timestamp;
        }

    }
}