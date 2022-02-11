using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using NAudio.CoreAudioApi;

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
                if (DateTime.Now.DayOfWeek != DayOfWeek.Monday || DateTime.Now.Hour > 9)
                    return;

            form.Invoke(new Action(form.Show));
            var filename = Download();

            if (filename == "") return;
            OpenFile(filename);
            form.Invoke(new Action(form.Hide));
        }

        public static void CheckEnglishAudio(string unit, string filename, bool forceCheck = false)
        {
            if (!forceCheck)
            {
                if (
                    (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday || DateTime.Now.DayOfWeek == DayOfWeek.Thursday) &&
                    DateTime.Now.Hour == 7 && DateTime.Now.Minute <= 35)
                {
                }
                else
                    return;
            }

            var dirs = Directory.GetDirectories("audios/");
            foreach (var dirName in dirs)
                if (dirName.IndexOf(unit, StringComparison.Ordinal) != -1)
                {
                    var files = Directory.GetFiles(dirName);
                    foreach (var file in files)
                    {
                        if (!file.Contains(filename)) continue;
                        OpenFile(file);
                        AdjustVolume(0.7f);
                        return;
                    }
                }
        }

        private string Download()
        {
            try
            {
                _logger.LogI("正在获取文件夹列表");
                var ftpRoot = "ftp://192.168.2.8/校园电视台/";
                var req = (FtpWebRequest)WebRequest.Create(ftpRoot);
                req.Method = WebRequestMethods.Ftp.ListDirectory;
                _logger.LogI("hit when list directory.");
                var response = (FtpWebResponse)req.GetResponse();

                var responseStream = response.GetResponseStream();
                var reader = new StreamReader(responseStream);
                var files = Utils.FMap(reader.ReadToEnd().Split('\n'), x => x.Trim('\r'));

                reader.Close();
                response.Close();

                _logger.LogI($"成功获取了文件夹列表:  {Utils.Intersperse(files, ',')}");

                var finalFilename = "";
                foreach (var file in files)
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
                var request = (FtpWebRequest)WebRequest.Create(ftpRoot + finalFilename);
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                using var ftpStream = request.GetResponse().GetResponseStream();
                using Stream fileStream = File.Create(finalFilename);
                _logger.LogI($"正在下载文件: {finalFilename}");
                var buffer = new byte[102400];
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
                _logger.LogE("从FTP下载文件失败：" + e.Message + "\n" + ((FtpWebResponse)e.Response).StatusDescription);
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
            var p = new Process
            {
                StartInfo =
                {
                    FileName = "explorer.exe", Arguments = $"\"{filename}\""
                }
            };
            p.Start();
        }

        private static void AdjustVolume(float value)
        {
            var devEnum = new MMDeviceEnumerator();
            var defaultDevice = devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
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