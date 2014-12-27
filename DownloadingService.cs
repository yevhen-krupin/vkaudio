using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VKAUDIO.Configuration;

namespace VKAUDIO
{
    public class DownloadingService
    {
        public EventHandler<AudioDownloadingFailedEventArgs> FailedAudio { get; set; }
        public EventHandler<DownloadingProgressArgs> ProgressChanged { get; set; }
        public EventHandler<EventArgs> DownloadFinished { get; set; }
        volatile int currentIndex = 0;
        public void Download(Audio audio)
        {
            Task.Factory.StartNew(() =>
            {
                string url = audio.url.Split(new string[1] { "?extra" }, StringSplitOptions.RemoveEmptyEntries)
                                   .FirstOrDefault();
                using (WebClient wc = new WebClient())
                {
                    string name = ApplicationConfiguration.Instance.SavingDirectory +
                                    "\\" + audio.ToString();
                    wc.DownloadProgressChanged += WcOnDownloadProgressChanged;
                    wc.DownloadFileCompleted += WcOnDownloadFileCompleted;
                    wc.DownloadFileAsync(new Uri(url), name, audio);
                }
            });

        }

        private void WcOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs asyncCompletedEventArgs)
        {

            if (asyncCompletedEventArgs.Error != null)
            {
                reportFail(asyncCompletedEventArgs.UserState as Audio);
            }
            else
            {
                reportFinish();
            }
        }

        private void WcOnDownloadProgressChanged(object sender, 
            DownloadProgressChangedEventArgs downloadProgressChangedEventArgs)
        {
            reportProgress(downloadProgressChangedEventArgs.BytesReceived, downloadProgressChangedEventArgs.TotalBytesToReceive);
        }

        public void DownloadList(IEnumerable<Audio> audios)
        {
            int totalCount = audios.Count();
            currentIndex = 0;
            Task.Factory.StartNew(() =>
            {
                foreach (var audio in audios)
                {
                    string url =
                                audio.url.Split(new string[1] { "?extra" }, StringSplitOptions.RemoveEmptyEntries)
                                    .FirstOrDefault();
                    try
                    {
                        using (WebClient wc = new WebClient())
                        {
                            string name = ApplicationConfiguration.Instance.SavingDirectory +
                                          "\\" + audio.ToString();
                            wc.DownloadFile(url, name);
                        }
                    }
                    catch (Exception)
                    {
                        reportFail(audio);
                    }
                    finally
                    {
                        currentIndex++;
                        reportProgress(currentIndex, totalCount);
                    }
                }
                reportFinish();
            });
        }

        private void reportFinish()
        {
            if (DownloadFinished != null)
            {
                DownloadFinished(this, new EventArgs());
            }
        }

        private void reportFail(Audio audio)
        {
            if (FailedAudio != null)
            {
                FailedAudio(this, new AudioDownloadingFailedEventArgs() { Audio = audio });
            }
        }

        private void reportProgress(long val, long total)
        {
            if (ProgressChanged != null)
            {
                DownloadingProgressArgs args = new DownloadingProgressArgs();
                args.Percentage = ((double)val / (double)total) * 100;
                ProgressChanged(this, args);
            }
        }
    }
}