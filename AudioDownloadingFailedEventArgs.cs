using System;

namespace VKAUDIO
{
    public class AudioDownloadingFailedEventArgs:EventArgs
    {
        public Audio Audio { get; set; }
    }
}