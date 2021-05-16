using NAudio.CoreAudioApi;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;


namespace WBAssistantF.Module.AutoPlay
{
    internal class AutoPlay
    {
        private readonly Logger _logger;
        public AutoPlay(Logger lgr)
        {
            _logger = lgr;
        }

        

        public void CheckFtp(MainForm form, bool forceCheck = false)
        {
            if (!forceCheck)
            {
                if (DateTime.Now.DayOfWeek != DayOfWeek.Monday || DateTime.Now.Hour > 9) { return; }
            }

            form.Invoke(new Action(form.Show));
            string filename = Download();

            if (filename == "") return;
            OpenFile(filename);
            form.Invoke(new Action(form.Hide));
        }

        public void checkEnglishAudio(string unit, string filename, bool forceCheck = false)
        {
            if (!forceCheck)
            {
                if (
               (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday || DateTime.Now.DayOfWeek == DayOfWeek.Thursday) &&
               DateTime.Now.Hour == 7 && DateTime.Now.Minute <= 35) { }
                else
                { return; }
            }

            string[] dirs = Directory.GetDirectories("audios/");
            foreach (string dirName in dirs)
            {
                if (dirName.IndexOf(unit, StringComparison.Ordinal) != -1)
                {
                    string[] files = Directory.GetFiles(dirName);
                    foreach (string file in files)
                    {
                        if (file.IndexOf(filename, StringComparison.Ordinal) == -1) continue;
                        OpenFile(file);
                        adjustVolume(0.7f);
                        return;
                    }
                }
            }
        }

        private string Download()
        {
            try
            {
                _logger.LogI($"正在获取文件夹列表");
                string ftpRoot = "ftp://192.168.2.8/校园电视台/";
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpRoot);
                req.Method = WebRequestMethods.Ftp.ListDirectory;

                FtpWebResponse response = (FtpWebResponse)req.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string[] files = Utils.FMap(reader.ReadToEnd().Split('\n'), (x) => x.Trim('\r'));

                reader.Close();
                response.Close();

                _logger.LogI($"成功获取了文件夹列表:  {Utils.Intersperse(files, ',')}");

                string finalFilename = "";
                foreach (string file in files)
                {
                    if (file.Length <= 5 || file[^3..] != "mp4") continue;
                    finalFilename = file;
                    break;
                }
                if (finalFilename == "")
                {
                    _logger.LogI($"未从ftp中找到有效视频，文件列表为: {Utils.Intersperse(files, ',')}");
                    return "";
                }

                // Start downloading file
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpRoot + finalFilename);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                using Stream ftpStream = request.GetResponse().GetResponseStream();
                using Stream fileStream = File.Create(finalFilename);
                _logger.LogI($"正在下载文件: {finalFilename}");
                byte[] buffer = new byte[102400];
                int read;
                long pos = 0;
                while ((read = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, read);
                    if (pos != fileStream.Position / 1024 / 1024)
                    {
                        pos = fileStream.Position / 1024 / 1024;
                        _logger.LogI($"已下载：{pos}兆");
                    }

                }

                return finalFilename;
            }
            catch (WebException e)
            {
                _logger.LogE("从FTP下载文件失败：" + ((FtpWebResponse)e.Response).StatusDescription);
                return "";
            }
            catch (Exception ex)
            {
                _logger.LogE("从FTP下载文件失败：" + ex.Message);
                return "";
            }
        }

        private static void OpenFile(string filename)
        {
            Process p = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe", Arguments = $"/c start \"\" \"{filename}\"", CreateNoWindow = true
                }
            };
            p.Start();
        }

        private void adjustVolume(float value)
        {
            MMDeviceEnumerator devEnum = new MMDeviceEnumerator();
            MMDevice defaultDevice = devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            defaultDevice.AudioEndpointVolume.Mute = false;
            while (defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar > value)
            {
                defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar -= 0.01F;
                Thread.Sleep(100);
            }

            while (defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar < value)
            {
                defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar += 0.01F;
                Thread.Sleep(100);
            }
        }
    }
}
