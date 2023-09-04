using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace DfuTool
{
    public partial class Form1 : Form
    {
        readonly int[] allBaudrates = { 2400, 4800, 9600, 38400, 57600, 74880, 115200 };  //波特率
        readonly int[] allDataBits = { 5, 6, 7, 8 };    //数据位数
        readonly float[] allStopBits = { 1, 1.5f, 2 };    //停止位数
        readonly string strPortNotOpen = "串口未打开";
        readonly string strCfgFileName = "Simulator.cfg";

        PortCfg config = null;
        SerialPort serialPort = null;

        public Form1()
        {
            InitializeComponent();

            //串口列表 
            comboBoxPort.Items.AddRange(SerialPort.GetPortNames());


            try
            {
                using (FileStream stream = File.OpenRead(strCfgFileName))
                {
                    BinaryFormatter deserializer = new BinaryFormatter();   //二进制序列化器
                    config = deserializer.Deserialize(stream) as PortCfg;    //反序列化

                    int i = 0;
                    foreach (string name in SerialPort.GetPortNames())
                    {
                        if (name == config.portName)
                            break;
                        i++;
                    }

                    comboBoxPort.SelectedIndex = Helper.Math.Clamp<int>(i, 0, comboBoxPort.Items.Count - 1);

                    textBoxBinFilePath.Text = config.binFilePath;
                    textBoxBaudRate.Text = config.baudrate.ToString();
                    textBoxSignPath.Text = config.sigFilePath;
                    stream.Close();
                }
            }
            catch
            {
                //配置文件不存在,使用默认设置 
                if (comboBoxPort.Items.Count > 0)
                    comboBoxPort.SelectedIndex = 0;


                config = new PortCfg();
            }

            CommInit(); //utility 初始化
        }

        bool portOpened
        {
            get
            {
                bool res = serialPort != null && serialPort.IsOpen;
                return res;
            }
        }

        //打开端口按钮
        private void buttonOpenPort_Click(object sender, EventArgs e)
        {
            if (portOpened)
            {   //关闭串口
                ClosePort();

                buttonOpenPort.Text = "打开串口";
            }
            else
            {   //打开串口
                if (TryOpenPort())
                {
                    buttonOpenPort.Text = "关闭串口";
                }
                else
                {
                    MessageBox.Show("打开串口失败");
                }
            }

        }

        //尝试打开端口
        bool TryOpenPort()
        {
            string portName;
            //设置端口
            if (comboBoxPort.Text != null && comboBoxPort.Text != "")
                portName = comboBoxPort.Text;
            else
                return false;

            //baudrate
            int baudRate;
            try
            {
                baudRate = int.Parse(textBoxBaudRate.Text);
            }
            catch
            {
                MessageBox.Show("输入波特率无效");
                return false;
            }
            if (baudRate <= 0)
            {
                MessageBox.Show("输入波特率无效");
                return false;
            }

            serialPort = new SerialPort();
            serialPort.PortName = portName;
            serialPort.RtsEnable = true;
            //serialPort.CtsHolding
            //serialPort.DsrHolding
            serialPort.DtrEnable = true;

            //设置波特率
            serialPort.BaudRate = (int)baudRate;
            //设置校验
            serialPort.Parity = Parity.None;
            //设置数据位数
            serialPort.DataBits = 8;

            //设置停止位
            serialPort.StopBits = StopBits.One;

            //接收数据回调
            serialPort.DataReceived += this.OnReceivedData;
            //回调阈值设置为1
            serialPort.ReceivedBytesThreshold = 1;

            try
            {
                serialPort.Open();
            }
            catch
            {
                serialPort = null;
                return false;
            }

            config.portName = serialPort.PortName;

            SaveConfiguration();
            return true;
        }

        void SaveConfiguration()
        {
            using (FileStream stream = File.Create(strCfgFileName))
            {
                BinaryFormatter serializer = new BinaryFormatter();　　//二进制格式序列化器
                serializer.Serialize(stream, config);　　//序列化对象到文件中
                stream.Close();
            }
        }

        //关闭端口
        void ClosePort()
        {
            if (serialPort != null)
            {
                if (dfuSender != null)
                {
                    UpgradeStop();
                }

                try
                {
                    serialPort.Close();
                }
                catch
                {

                }
                serialPort = null;
            }
        }


        delegate void AppendNewReceivedData(byte[] data);
        SgpProtocol.Parser protocol = new SgpProtocol.Parser();
        //接收数据处理
        void AppendRecvData(byte[] data)
        {
            protocol.ParseData(data);
        }

        //接收回调
        void OnReceivedData(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] data = new byte[serialPort.BytesToRead];
            serialPort.Read(data, 0, data.Length);//读取数据

            this.BeginInvoke(new AppendNewReceivedData(AppendRecvData), data);
        }

        //端口列表下拉
        private void comboBoxPort_DropDown(object sender, EventArgs e)
        {
            int tmp = comboBoxPort.SelectedIndex;
            comboBoxPort.Items.Clear();
            comboBoxPort.Items.AddRange(SerialPort.GetPortNames());
            if (tmp >= comboBoxPort.Items.Count)
                tmp = comboBoxPort.Items.Count - 1;
            comboBoxPort.SelectedIndex = tmp;
        }

        //form关闭时
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClosePort(); //关闭串口
            CommExit();
        }


        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();     //显示选择文件对话框
            openFileDialog.InitialDirectory = null; //"c:\\";
            openFileDialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBoxBinFilePath.Text = openFileDialog.FileName;          //显示文件路径
                config.binFilePath = openFileDialog.FileName;
                SaveConfiguration();
            }
        }

        private void buttonStartUpgrade_Click(object sender, EventArgs e)
        {
            if (dfuSender != null)
            {   //正在升级中
                UpgradeStop();
            }
            else
            {   //目前没有升级
                if (!UpgradeStart())
                {   //开始升级失败，可能是串口没打开
                    MessageBox.Show("串口没打开或文件不存在");
                }
            }

        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {
            this.textBoxLog.Clear();
        }

        private void DebugLog(string s)
        {
            textBoxLog.AppendText(s);
            textBoxLog.AppendText(Environment.NewLine);
        }

        //bootloader版本
        private void buttonBootVersion_Click(object sender, EventArgs e)
        {
            if (portOpened)
            {
                for (int i = 0; i < 20; i++)
                    SgpProtocol.Parser.SendPacket(serialPort.Write, SgpProtocol.SgpCmd.Dfu, SgpProtocol.SgpSubCmd.DfuBootVer, null, 0, 0);
            }
            else
            {
                MessageBox.Show(strPortNotOpen);
            }
        }

        private void buttonSelectSign_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();     //显示选择文件对话框
            openFileDialog.InitialDirectory = null; //"c:\\";
            openFileDialog.Filter = "sig files (*.sig)|*.sig|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBoxSignPath.Text = openFileDialog.FileName;          //显示文件路径
                config.sigFilePath = openFileDialog.FileName;
                SaveConfiguration();
            }
        }
    }



    [Serializable]
    class PortCfg
    {
        public string portName = null; //串口名字
        public int baudrate = 115200;
        public string binFilePath = null; //bin文件路径
        public string sigFilePath = null; //sig文件路径
    }
}
