using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace VKAUDIO
{
    /// <summary>
    /// Interaction logic for RWPAppWindow.xaml
    /// </summary>
    public partial class RWPAppWindow : Window
    {
       
        public AudioSearchService AudioSearchService { get; set; }
        public OAuthController OauthController { get; set; }
        public ObservableCollection<Audio> Audios { get; set; }
        public DownloadingService DownloadingService { get; set; }
        public PlaybackService PlaybackService { get; set; }
        public RWPAppWindow()
        {
            InitializeComponent();
            Audios = new ObservableCollection<Audio>();
            AudioSearchService = new AudioSearchService(Dispatcher, Audios);
            AudioSearchService.Error += Error;
            DownloadingService = new DownloadingService();
            PlaybackService = new PlaybackService(MediaElement, Audios);
            AudioDataGrid.ItemsSource = Audios;
            SetProgress(0);
            SetStatus("Привет");
        }

        private void Error(object sender, SearchingErrorArgs searchingErrorArgs)
        {
            MessageBox.Show(searchingErrorArgs.Message);
        }


        public void Open(OAuthController controller)
        {
            OauthController = controller;
            Show();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AudioSearchService.Search(OauthController.OAuthToken.Token, SearchTextBox.Text, 200);
            }
            
        }

        private void mouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var row = sender as DataGridRow;
                var audio = row.Item as Audio;
                PlaybackService.Play(audio);
            }
            catch (Exception)
            {
                MessageBox.Show("Не могу воспроизвести трек");
            }
           

        }

        

        private void download(Audio audio)
        {
            try
            {
                SetStatus("Загрузка");
                DownloadingService.DownloadFinished += DownloadFinished;
                DownloadingService.ProgressChanged += ProgressChanged;
                DownloadingService.FailedAudio += FailedAudio;
                DownloadingService.Download(audio);
            }
            catch (Exception)
            {
                MessageBox.Show("Не могу скачать файл");
            }
        }

        private void Click_Pause(object sender, RoutedEventArgs e)
        {
          PlaybackService.Pause();
        }

        private void Click_Play(object sender, RoutedEventArgs e)
        {
            PlaybackService.Play();
        }
        

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
        }


        private void download_checked(object sender, RoutedEventArgs e)
        {
            SetStatus("Загрузка");
            var checkedAudios = Audios.Where(x => x.ischecked);
            DownloadingService.FailedAudio += FailedAudio;
            DownloadingService.ProgressChanged += ProgressChanged;
            DownloadingService.DownloadFinished+= DownloadFinished;
            DownloadingService.DownloadList(checkedAudios);
        }

        private void SetStatus(string status)
        {
            Dispatcher.Invoke(new Action(() => StatusTextBlock.Text = status));
        }

        private void SetProgress(double percentage)
        {
            if (percentage < 1||percentage > 99)
            {
                Dispatcher.Invoke(new Action(() => ProgressBar.Visibility = Visibility.Hidden));
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    ProgressBar.Visibility = Visibility.Visible;
                    ProgressBar.Value = percentage;
                }));    
            }
            
        }

        private void ProgressChanged(object sender, DownloadingProgressArgs downloadingProgressArgs)
        {
            SetProgress(downloadingProgressArgs.Percentage);
        }

        private void FailedAudio(object sender, AudioDownloadingFailedEventArgs audioDownloadingFailedEventArgs)
        {
            MessageBox.Show("Загрузка песни не удалась: " + audioDownloadingFailedEventArgs.Audio.ToString());
        }

        private void DownloadFinished(object sender, EventArgs eventArgs)
        {
            DownloadingService.ProgressChanged -= ProgressChanged;
            DownloadingService.DownloadFinished -= DownloadFinished;
            DownloadingService.FailedAudio -= FailedAudio;
            SetStatus("Загрузка окончена");
        }
        
        private void checkbox_mousedown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            var checkbox = ChildHelper.FindVisualChildren<CheckBox>(cell).FirstOrDefault();
            checkbox.IsChecked = !checkbox.IsChecked;
            
        }

        private void DownloadOne_Click(object sender, RoutedEventArgs e)
        {
            var obj = AudioDataGrid.SelectedItem;
            if (obj != null)
            {
                Audio audio = obj as Audio;
                download(audio);
            }
            
        }
    }
}
