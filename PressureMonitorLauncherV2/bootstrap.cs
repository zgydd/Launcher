using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using System.Net.Sockets;
using System.Collections.Generic;
using IWshRuntimeLibrary;

namespace PressureMonitorLauncherV2
{
    public partial class bootstrap : Form
    {
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);
        private delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);
        [DllImport("user32.dll")]
        private static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        private Dictionary<string, string> dicActivedLang = new Dictionary<string, string>();
        private static string[] targetNames = new string[] { "Presure Monitor"
            ,"思澄智能", "八益"
            , "步态分析系统", "压疮监测系统"
            , "Gait Analysis System", "Pressure ulcer Assessment System" };
        private void initLang()
        {
            switch (System.Threading.Thread.CurrentThread.CurrentCulture.Name)
            {
                case "zh-CN":
                    dicActivedLang.Add("TITLE", "压力垫监测分析系统");
                    dicActivedLang.Add("LAUNCHER", " 启动器");
                    dicActivedLang.Add("C001", "---已复制");
                    dicActivedLang.Add("C002", "向");
                    dicActivedLang.Add("C003", "程序将在5秒后关闭");
                    dicActivedLang.Add("L001", "清理...");
                    dicActivedLang.Add("L002", "目录下复制配置文件");
                    dicActivedLang.Add("L003", "寻找适配IP地址...");
                    dicActivedLang.Add("N001", "复制文件失败");
                    dicActivedLang.Add("N002", "请确认配置文件完整且用户拥有向C盘写入文件的权限（尝试管理员运行）");
                    dicActivedLang.Add("N003", "未找到192.168.0网段的IP地址，请修改IP后重启程序。");
                    dicActivedLang.Add("N004", "未连接到设备，请连接设备后重启程序。");
                    dicActivedLang.Add("N005", "找到适配IP地址：");
                    dicActivedLang.Add("N006", "启动主程序，启动器将在主程序启动后自行关闭");
                    dicActivedLang.Add("N007", "主程序启动失败。");
                    break;
                default:
                    dicActivedLang.Add("TITLE", "Pressure pad monitoring and analysis system");
                    dicActivedLang.Add("LAUNCHER", " Launcher");
                    dicActivedLang.Add("C001", "---Copyed");
                    dicActivedLang.Add("C002", "Target:");
                    dicActivedLang.Add("C003", "The program will close after 5 seconds.");
                    dicActivedLang.Add("L001", "Cleanning...");
                    dicActivedLang.Add("L002", " copy profiles");
                    dicActivedLang.Add("L003", "Find a suitable IP address...");
                    dicActivedLang.Add("N001", "Failed to copy file");
                    dicActivedLang.Add("N002", "Please confirm that the profile is completed and the user has permission to write files to the C disk (try run as administrator)");
                    dicActivedLang.Add("N003", "The IP address of the 192.168.0 network segment was not found, please modify the equipment IP and restart the program.");
                    dicActivedLang.Add("N004", "The device is not connected, please connect the device and restart the program.");
                    dicActivedLang.Add("N005", "Find the appropriate IP address:");
                    dicActivedLang.Add("N006", "Start the main program. The launcher will shut down itself after the main program starts");
                    dicActivedLang.Add("N007", "The main program failed to start.");
                    break;
            }

        }
        private void copyFile(DirectoryInfo baseDirectory, string strType)
        {
            string strTargetDir = @"C:\\City Zen Solutions\Pressure Monitoring\";
            if (!Directory.Exists(strTargetDir)) Directory.CreateDirectory(strTargetDir);
            FileInfo[] files = baseDirectory.GetFiles(strType, SearchOption.TopDirectoryOnly);
            foreach (FileInfo file in files)
            {
                using (FileStream fileN = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                {
                    byte[] bt = new byte[5];
                    int count = 0;
                    using (FileStream fileS = new FileStream(strTargetDir + file.Name, FileMode.Create, FileAccess.Write))
                    {
                        while ((count = fileN.Read(bt, 0, bt.Length)) > 0)
                        {
                            fileS.Write(bt, 0, count);
                        }
                    }
                }
                rtxtDetail.AppendText(file.Name + dicActivedLang["C001"] + "\r\n");
            }
        }
        static string findIPA()
        {
            string result = "";
            NetworkInterface[] fNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in fNetworkInterfaces)
            {
                if (result.Length > 0) break;
                string fRegistryKey = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\" + adapter.Id + "\\Connection";
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                if (rk != null)
                {
                    // 区分 PnpInstanceID  
                    // 如果前面有 PCI 就是本机的真实网卡 
                    string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                    int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
                    if (fPnpInstanceID.Length > 3 && fPnpInstanceID.Substring(0, 3) == "PCI")
                    {
                        IPInterfaceProperties fIPInterfaceProperties = adapter.GetIPProperties();
                        UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = fIPInterfaceProperties.UnicastAddresses;
                        foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                        {
                            if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                IPAddress ipa = UnicastIPAddressInformation.Address;
                                Match m = Regex.Match(ipa.ToString(), @"192.168.0.(?:(25[0-5])|(2[0-4]\d)|((1\d{2})|([1-9]?\d)))");
                                if (m.Success && ipa.ToString() != "192.168.0.57")
                                {
                                    result = ipa.ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        static bool checkIPA(string targetIPA)
        {
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork && ipa.ToString() == targetIPA) return true;
            }
            return false;
        }
        private void sleepAndClose()
        {
            Task.Run(async delegate
            {
                await Task.Delay(5000);
                Environment.Exit(0);
                return 42;
            });
        }
        private void killExist()
        {
            Process[] p = Process.GetProcessesByName("pressureMonitorV3_win_x64");
            foreach (Process sp in p)
            {
                sp.Kill();
            }
        }
        private static void checkAndClose(object source, ElapsedEventArgs e)
        {
            EnumWindows(delegate (IntPtr hWnd, int lParam)
            {
                StringBuilder sb = new StringBuilder(256);
                //get window name
                GetWindowTextW(hWnd, sb, sb.Capacity);
                string szWindowName = sb.ToString();
                for (int i = 0; i < targetNames.Length; i++)
                {
                    if (szWindowName.IndexOf(targetNames[i]) >= 0)
                    {
                        Environment.Exit(0);
                        break;
                    }
                }
                return true;
            }, 0);
        }
        private void createShortCut()
        {
            string DesktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            WshShell shell = new WshShell();
            IWshShortcut shortcutL = (IWshShortcut)shell.CreateShortcut(DesktopPath + "\\" + dicActivedLang["TITLE"] + dicActivedLang["LAUNCHER"] + ".lnk");
            shortcutL.TargetPath = AppDomain.CurrentDomain.BaseDirectory + @"PressureMonitorLauncherV2.exe";
            shortcutL.Arguments = "";
            shortcutL.Description = dicActivedLang["LAUNCHER"];
            shortcutL.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            shortcutL.Hotkey = "CTRL+SHIFT+ALT+L";
            shortcutL.WindowStyle = 1;
            shortcutL.Save();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(DesktopPath + "\\" + dicActivedLang["TITLE"] + ".lnk");
            shortcut.TargetPath = AppDomain.CurrentDomain.BaseDirectory + @"bin\pressureMonitorV3_win_x64.exe";
            shortcut.Arguments = "";
            shortcut.Description = dicActivedLang["TITLE"];
            shortcut.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + @"bin";
            shortcut.Hotkey = "CTRL+SHIFT+ALT+P";
            shortcut.WindowStyle = 1;
            shortcut.Save();
        }
        public bootstrap()
        {
            InitializeComponent();
            initLang();
        }

        private void bootstrap_Shown(object sender, EventArgs e)
        {
            rtxtDetail.AppendText(dicActivedLang["L001"] + "\r\n");
            try
            {
                killExist();
            }
            catch (Exception ex)
            {
                rtxtDetail.AppendText(ex.Message);
            }
            rtxtDetail.AppendText(dicActivedLang["C002"] + "C:\\City Zen Solutions\\Pressure Monitoring\\" + dicActivedLang["L002"] + "\r\n");
            try
            {
                DirectoryInfo baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "configures\\");
                copyFile(baseDirectory, "*.json");
                copyFile(baseDirectory, "*.scale");
            }
            catch (Exception)
            {
                rtxtDetail.AppendText(dicActivedLang["N001"] + "\r\n" + dicActivedLang["N002"] + "\r\n" + dicActivedLang["C003"]);
                lblView.Text = dicActivedLang["N001"] + "\r\n" + dicActivedLang["N002"] + "\r\n" + dicActivedLang["C003"];
                sleepAndClose();
                return;
            }
            rtxtDetail.AppendText(dicActivedLang["L003"] + "\r\n");
            string ipa = findIPA();
            if (ipa.Length <= 0)
            {
                rtxtDetail.AppendText(dicActivedLang["N003"] + dicActivedLang["C003"]);
                lblView.Text = dicActivedLang["N003"] + dicActivedLang["C003"];
                sleepAndClose();
                return;
            }
            if (!checkIPA(ipa))
            {
                rtxtDetail.AppendText(dicActivedLang["N004"] + dicActivedLang["C003"]);
                lblView.Text = dicActivedLang["N004"] + dicActivedLang["C003"];
                sleepAndClose();
                return;
            }
            rtxtDetail.AppendText(dicActivedLang["N005"] + ipa + "\r\n");
            rtxtDetail.AppendText(dicActivedLang["N006"] + "\r\n");
            lblView.Text = dicActivedLang["N005"] + ipa + "\r\n" + dicActivedLang["N006"];
            try
            {
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"bin\pressureMonitorV3_win_x64.exe");
                createShortCut();
                System.Timers.Timer timerCheck = new System.Timers.Timer();
                timerCheck.Enabled = true;
                timerCheck.Interval = 1000;
                timerCheck.Start();
                timerCheck.Elapsed += new ElapsedEventHandler(checkAndClose);
            }
            catch (Exception ex)
            {
                rtxtDetail.AppendText(dicActivedLang["N007"] + dicActivedLang["C003"] + "\r\n");
                lblView.Text = dicActivedLang["N007"] + dicActivedLang["C003"];
                rtxtDetail.AppendText(ex.Message);
                sleepAndClose();
                return;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
