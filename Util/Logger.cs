using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WBAssistantF
{
    public class Logger
    {
        private readonly MainForm context;

        public Logger(MainForm mw)
        {
            context = mw;
        }

        public void LogI(string str)
        {
            context.Invoke(new Action(() =>
            {
                context.LogTextBox.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] [Info] ", Color.Green);
                context.LogTextBox.AppendText(str + "\r\n", Color.Black);
                context.LogTextBox.ScrollToCaret();
            }));
            File.AppendAllText("WBAData\\log\\info.log", "[" + DateTime.Now.ToString("") + "] [Info] " + str + "\r\n");
        }

        public void LogC(string str)
        {
            context.Invoke(new Action(() =>
            {
                context.LogTextBox.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] [Core] ", Color.DimGray);
                context.LogTextBox.AppendText(str + "\r\n", Color.Black);
                context.LogTextBox.ScrollToCaret();
            }));
        }

        public void LogW(string str)
        {
            context.Invoke(new Action(() =>
            {
                context.LogTextBox.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] [Warn] ", Color.Gold);
                context.LogTextBox.AppendText(str + "\r\n", Color.Black);
                context.LogTextBox.ScrollToCaret();
            }));
        }
        public void LogE(string str)
        {
            context.Invoke(new Action(() =>
            {
                context.LogTextBox.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] [Error] ", Color.Red);
                context.LogTextBox.AppendText(str + "\r\n", Color.Black);
                context.LogTextBox.ScrollToCaret();
            }));
        }

        public void WriteE(string str)
        {
            File.AppendAllText("WBAData\\log\\error.log", "[" + DateTime.Now.Date.ToString() + "]" + str + "\r\n");
        }

        public void LogTrigger(string str)
        {
            context.Invoke(new Action(() =>
            {
                context.LogTextBox.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] [Trig] ", Color.DeepSkyBlue);
                context.LogTextBox.AppendText(str + "\r\n", Color.Black);
                context.LogTextBox.ScrollToCaret();
            }));
            File.AppendAllText("WBAData\\log\\info.log", "[" + DateTime.Now.ToString("") + "] [Trig] " + str + "\r\n");
        }
    }
    public static class Extensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
