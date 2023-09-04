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
using SgpProtocol;
using FastCrc;

namespace DfuTool
{
    //这里只考虑了数据错误重发,没有考虑超时重发
    public partial class Form1 : Form
    {
        Timer upgradeTimer = new Timer();
        readonly int timerInterval = 200;    //处理定时器间隔

        DfuSender dfuSender = null;

        public void DfuSendMethod(byte[] data, int offset, int count)
        {
            if (!portOpened)
            {
                DebugLog("串口未打开");
            }
            else
            {
                serialPort.Write(data, offset, count);
            }
        }

        bool UpgradeStart()
        {
            try
            {
                dfuSender = new DfuSender(textBoxBinFilePath.Text, textBoxSignPath.Text, this.DfuSendMethod, this.DebugLog);
            }
            catch
            {
                return false;
            }
            //开启定时器
            upgradeTimer.Tick += UpgradeTimerHandler;
            upgradeTimer.Interval = timerInterval;
            upgradeTimer.Start();
            this.buttonUpgrade.Text = "取消升级";

            return true;
        }

        void UpgradeStop()
        {
            //关闭定时器
            upgradeTimer.Tick -= UpgradeTimerHandler;
            upgradeTimer.Stop();

            dfuSender.Dispose();
            dfuSender = null;
            this.buttonUpgrade.Text = "开始升级";
        }

        void UpgradeTimerHandler(object sender, EventArgs e)
        {
            if (this.dfuSender != null)
            {
                int percent = this.dfuSender.Update();
                if (percent == 100 || percent < 0)
                {
                    UpgradeStop();
                }
            }
        }


    }
}