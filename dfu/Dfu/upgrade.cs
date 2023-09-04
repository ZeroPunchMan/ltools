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
    //����ֻ���������ݴ����ط�,û�п��ǳ�ʱ�ط�
    public partial class Form1 : Form
    {
        Timer upgradeTimer = new Timer();
        readonly int timerInterval = 200;    //����ʱ�����

        DfuSender dfuSender = null;

        public void DfuSendMethod(byte[] data, int offset, int count)
        {
            if (!portOpened)
            {
                DebugLog("����δ��");
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
            //������ʱ��
            upgradeTimer.Tick += UpgradeTimerHandler;
            upgradeTimer.Interval = timerInterval;
            upgradeTimer.Start();
            this.buttonUpgrade.Text = "ȡ������";

            return true;
        }

        void UpgradeStop()
        {
            //�رն�ʱ��
            upgradeTimer.Tick -= UpgradeTimerHandler;
            upgradeTimer.Stop();

            dfuSender.Dispose();
            dfuSender = null;
            this.buttonUpgrade.Text = "��ʼ����";
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