using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;


namespace pause
{
    public partial class Form1 : Form
    {
        private class PauseConfig
        {
            public string? exeName = null;
            public int pauseKey;
            public int resumeKey;
        }

        class HotKey
        {
            //如果函数执行成功，返回值不为0。
            //如果函数执行失败，返回值为0。要得到扩展错误信息，调用GetLastError。
            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool RegisterHotKey(
                            IntPtr hWnd,                //要定义热键的窗口的句柄
                int id,                     //定义热键ID（不能与其它ID重复）
                KeyModifiers fsModifiers,   //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效
                Keys vk                     //定义热键的内容
                );
            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool UnregisterHotKey(
                IntPtr hWnd,                //要取消热键的窗口的句柄
                int id                      //要取消热键的ID
                );
            //定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）
            [Flags()]
            public enum KeyModifiers
            {
                None = 0,
                Alt = 1,
                Ctrl = 2,
                Shift = 4,
                WindowsKey = 8
            }
        }

        void SaveConfiguration()
        {
            if (config == null)
                config = new PauseConfig();
            config.exeName = textBoxExeName.Text;


            using (FileStream stream = File.Create(strCfgFileName))
            {
                JsonSerializerOptions options = new JsonSerializerOptions() { IncludeFields = true };
                JsonSerializer.Serialize(stream, config, options);
                stream.Close();
            }
        }

        private PauseConfig? config;
        readonly string strCfgFileName = "pause.cfg";
        public Form1()
        {
            InitializeComponent();

            try
            {
                using (FileStream stream = File.OpenRead(strCfgFileName))
                {
                    JsonSerializerOptions options = new JsonSerializerOptions() { IncludeFields = true };
                    PauseConfig? pauseConfig = JsonSerializer.Deserialize<PauseConfig>(stream, options);
                    config = pauseConfig;    //反序列化
                    if (config != null)
                    {
                        textBoxExeName.Text = config.exeName;
                    }
                    stream.Close();
                }
            }
            catch
            {
                textBoxExeName.Text = null;
            }

            HotKey.RegisterHotKey(Handle, 100, HotKey.KeyModifiers.None, Keys.F6);
            HotKey.RegisterHotKey(Handle, 101, HotKey.KeyModifiers.None, Keys.F7);

        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfiguration();

            HotKey.UnregisterHotKey(Handle, 100);
            HotKey.UnregisterHotKey(Handle, 101);
        }

        protected override void WndProc(ref Message m)
        {

            const int WM_HOTKEY = 0x0312;
            //按快捷键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:    //按下的是F9
                            {
                                string exeName = textBoxExeName.Text;
                                if (exeName == null || exeName.Length == 0)
                                {
                                    DebugLog("未输入进程名");
                                    return;
                                }

                                bool res = CallSuspend(exeName, true);
                                DebugLog(string.Format("暂停{0} {1}", exeName, res ? "成功" : "失败"));
                            }
                            break;
                        case 101:    //按下的是F10
                            {
                                string exeName = textBoxExeName.Text;
                                if (exeName == null || exeName.Length == 0)
                                {
                                    DebugLog("未输入进程名");
                                    return;
                                }

                                bool res = CallSuspend(exeName, false);
                                DebugLog(string.Format("恢复{0} {1}", exeName, res ? "成功" : "失败"));
                            }
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        void DebugLog(string s)
        {
            textBoxLog.AppendText(s);
            textBoxLog.AppendText(Environment.NewLine);
        }


        bool CallSuspend(string exeName, bool suspend)
        {
            DateTime start = DateTime.Now;
            string arguments = string.Format("{0} {1}", suspend ? "" : "-r", exeName);
            ProcessStartInfo psi = new ProcessStartInfo("pssuspend64", arguments) { CreateNoWindow = true };

            //启动
            Process? proc = Process.Start(psi);
            if (proc != null)
            {
                while (!proc.HasExited)
                {
                    TimeSpan span = DateTime.Now - start;
                    if (span.TotalMilliseconds >= 500)
                    {
                        proc.Kill();
                        return false;
                    }
                }
                if (proc.ExitCode == 0)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

    }
}