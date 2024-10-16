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
            //�������ִ�гɹ�������ֵ��Ϊ0��
            //�������ִ��ʧ�ܣ�����ֵΪ0��Ҫ�õ���չ������Ϣ������GetLastError��
            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool RegisterHotKey(
                            IntPtr hWnd,                //Ҫ�����ȼ��Ĵ��ڵľ��
                int id,                     //�����ȼ�ID������������ID�ظ���
                KeyModifiers fsModifiers,   //��ʶ�ȼ��Ƿ��ڰ�Alt��Ctrl��Shift��Windows�ȼ�ʱ�Ż���Ч
                Keys vk                     //�����ȼ�������
                );
            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool UnregisterHotKey(
                IntPtr hWnd,                //Ҫȡ���ȼ��Ĵ��ڵľ��
                int id                      //Ҫȡ���ȼ���ID
                );
            //�����˸����������ƣ�������ת��Ϊ�ַ��Ա��ڼ��䣬Ҳ��ȥ����ö�ٶ�ֱ��ʹ����ֵ��
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
                    config = pauseConfig;    //�����л�
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
            //����ݼ� 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:    //���µ���F9
                            {
                                string exeName = textBoxExeName.Text;
                                if (exeName == null || exeName.Length == 0)
                                {
                                    DebugLog("δ���������");
                                    return;
                                }

                                bool res = CallSuspend(exeName, true);
                                DebugLog(string.Format("��ͣ{0} {1}", exeName, res ? "�ɹ�" : "ʧ��"));
                            }
                            break;
                        case 101:    //���µ���F10
                            {
                                string exeName = textBoxExeName.Text;
                                if (exeName == null || exeName.Length == 0)
                                {
                                    DebugLog("δ���������");
                                    return;
                                }

                                bool res = CallSuspend(exeName, false);
                                DebugLog(string.Format("�ָ�{0} {1}", exeName, res ? "�ɹ�" : "ʧ��"));
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

            //����
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