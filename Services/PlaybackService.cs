using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace VKAUDIO.Services
{
    public class PlaybackService
    {
        public MediaElement MediaElement;
        public int Index { get; set; }
        public ObservableCollection<Audio> Audios { get; set; }
        public PlaybackService(MediaElement mediaElement, ObservableCollection<Audio> audios)
        {
            Audios = audios;
            MediaElement = mediaElement;
            MediaElement.LoadedBehavior= MediaState.Manual;
            MediaElement.MediaEnded += MediaElementOnMediaEnded;
        }

        private void MediaElementOnMediaEnded(object sender, RoutedEventArgs routedEventArgs)
        {
            Index++;
            if (Audios.Count > Index)
            {
                var uri = new Uri(Audios[Index].url);
                MediaElement.Source = uri;
                MediaElement.Play();
            }
        }

        public void Play(Audio audio)
        {
            Index = Audios.IndexOf(audio);
            var uri = new Uri(audio.url);
            MediaElement.Source = uri;
            Play();
        }

        public void Pause()
        {
            MediaElement.Pause();
        }

        public void Play()
        {
            try
            {
                MediaElement.Play();
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
    }
}