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

namespace UART_demo
{
    public partial class Form1 : Form
    {
        readonly int[] allBaudrates = { 2400, 4800, 9600, 38400, 57600, 74880, 115200, 115200 * 2, 115200 * 5 };  //波特率
        readonly int[] allDataBits = { 5, 6, 7, 8 };    //数据位数
        readonly float[] allStopBits = { 1, 1.5f, 2 };    //停止位数
        readonly string strCfgFileName = "uart-tool.cfg";

        PortCfg config = null;
        SerialPort serialPort = null;

        public Form1()
        {

            InitializeComponent();

            //串口列表 
            comboBoxPort.Items.AddRange(SerialPort.GetPortNames());

            //波特率列表 
            comboBoxBaudrate.BeginUpdate();
            foreach (int br in allBaudrates)
            {
                comboBoxBaudrate.Items.Add(br.ToString());
            }
            comboBoxBaudrate.EndUpdate();

            //数据位列表 
            comboBoxDataBits.BeginUpdate();
            foreach (int db in allDataBits)
            {
                comboBoxDataBits.Items.Add(db.ToString());
            }
            comboBoxDataBits.EndUpdate();

            //停止位列表 
            comboBoxStopBits.BeginUpdate();
            foreach (float sb in allStopBits)
            {
                comboBoxStopBits.Items.Add(sb.ToString());
            }
            comboBoxStopBits.EndUpdate();


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

                    comboBoxPort.SelectedIndex = Clamp<int>(i, 0, comboBoxPort.Items.Count - 1);
                    comboBoxBaudrate.SelectedIndex = Clamp<int>(config.baudrateIdx, 0, comboBoxBaudrate.Items.Count - 1);
                    comboBoxDataBits.SelectedIndex = Clamp<int>(config.dataBitsIdx, 0, comboBoxDataBits.Items.Count - 1);
                    comboBoxStopBits.SelectedIndex = Clamp<int>(config.stopBitsIdx, 0, comboBoxStopBits.Items.Count - 1);


                    stream.Close();
                }
            }
            catch
            {
                //配置文件不存在,使用默认设置 
                if (comboBoxPort.Items.Count > 0)
                    comboBoxPort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = 2;
                comboBoxDataBits.SelectedIndex = 3;
                comboBoxStopBits.SelectedIndex = 0;

                config = new PortCfg();
            }

            Type type = textBoxReceive.GetType();
            MethodInfo setStyle = type.GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo updateStyle = type.GetMethod("UpdateStyles", BindingFlags.Instance | BindingFlags.NonPublic);
            try
            {
                object[] param = new object[2];
                param[0] = ControlStyles.OptimizedDoubleBuffer;
                param[1] = true;
                setStyle.Invoke(textBoxReceive, param);
                updateStyle.Invoke(textBoxReceive, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

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
                    buttonOpenPort.Text = "打开串口";
                }
            }

        }

        //尝试打开端口
        bool TryOpenPort()
        {
            serialPort = new SerialPort();
            //设置端口
            if (comboBoxPort.Text != null && comboBoxPort.Text != "")
                serialPort.PortName = comboBoxPort.Text;
            else
                return false;
            //设置波特率
            serialPort.BaudRate = allBaudrates[comboBoxBaudrate.SelectedIndex];
            //设置校验
            serialPort.Parity = Parity.None;
            //设置数据位数
            serialPort.DataBits = allDataBits[comboBoxDataBits.SelectedIndex];

            //设置停止位
            if (comboBoxStopBits.Text == "1")
            {
                serialPort.StopBits = StopBits.One;
            }
            else if (comboBoxStopBits.Text == "1.5")
            {
                serialPort.StopBits = StopBits.OnePointFive;
            }
            else if (comboBoxStopBits.Text == "2")
            {
                serialPort.StopBits = StopBits.Two;
            }

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
            config.baudrateIdx = comboBoxBaudrate.SelectedIndex;
            config.dataBitsIdx = comboBoxDataBits.SelectedIndex;
            config.stopBitsIdx = comboBoxStopBits.SelectedIndex;

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
                serialPort.Close();
                serialPort = null;
            }
        }


        DateTime lastRecvTime = DateTime.Now;
        delegate void AppendNewReceivedData(byte[] data);
        //接收数据处理
        void AppendRecvData(byte[] data)
        {
            if (data.Length == 0)
                return;

            TimeSpan span = DateTime.Now - lastRecvTime;
            lastRecvTime = DateTime.Now;

            int newLineSpan = 50;
            try
            {
                newLineSpan = int.Parse(textBoxNewLineSpan.Text);
            }
            catch
            {

            }

            if (span.TotalMilliseconds >= newLineSpan)
            {
                if (checkBoxRecvNewLine.Checked || this.checkBoxDisTime.Checked)
                    this.textBoxReceive.AppendText(Environment.NewLine);

                if (this.checkBoxDisTime.Checked)
                {
                    DateTime now = DateTime.Now;
                    string s = string.Format("{0:00}:{1:00}:{2:00}.{3:000}: ", now.Hour, now.Minute, now.Second, now.Millisecond);
                    this.textBoxReceive.AppendText(s);
                }
            }

            if (!this.checkBoxDisHex.Checked)
            {   //字符串显示
                string s = System.Text.Encoding.ASCII.GetString(data);
                this.textBoxReceive.AppendText(s);
            }
            else
            {   //hex显示
                StringBuilder sb = new StringBuilder();
                foreach (byte b in data)
                {
                    sb.AppendFormat("{0:x2} ", b);
                }
                this.textBoxReceive.AppendText(sb.ToString());
            }

            //if (this.checkBoxDisTime.Checked)
            //    this.textBoxReceive.AppendText(Environment.NewLine);
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

            if (comboBoxPort.Items.Count <= tmp)
                tmp = -1;

            comboBoxPort.SelectedIndex = tmp;
        }

        private void buttonClearDisplay_Click(object sender, EventArgs e)
        {
            //清除窗口
            textBoxReceive.Text = null;
        }



        //form关闭时
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClosePort(); //关闭串口
        }

        //clamp
        public static T Clamp<T>(T value, T min, T max)
            where T : System.IComparable<T>
        {
            T result = value;
            if (value.CompareTo(max) > 0)
                result = max;
            if (value.CompareTo(min) < 0)
                result = min;
            return result;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (portOpened)
            {
                byte[] sendData = null;
                if (checkBoxSendHex.Checked)
                {
                    string[] allHexStr = textBoxSend.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    sendData = new byte[allHexStr.Length];
                    for (int i = 0; i < allHexStr.Length; i++)
                    {
                        try
                        {
                            sendData[i] = byte.Parse(allHexStr[i], System.Globalization.NumberStyles.HexNumber);
                        }
                        catch
                        {
                            MessageBox.Show("非法hex输入");
                            return;
                        }
                    }
                }
                else
                {
                    sendData = System.Text.Encoding.ASCII.GetBytes(textBoxSend.Text);
                }

                if (sendData != null)
                {
                    serialPort.Write(sendData, 0, sendData.Length);
                    if (checkBoxSendNewLine.Checked)
                    {
                        byte[] newLine = { (byte)'\r', (byte)'\n' };
                        serialPort.Write(newLine, 0, newLine.Length);
                    }
                }
            }
            else
            {
                MessageBox.Show("串口未打开");
            }
        }
    }



    [Serializable]
    class PortCfg
    {
        public string portName = null; //串口名字
        public int baudrateIdx = 2; //这些配置的可选项是硬编码的内容,用索引存
        public int dataBitsIdx = 3;
        public int stopBitsIdx = 0;
    }
}
