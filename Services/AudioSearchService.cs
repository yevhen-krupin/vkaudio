using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;
using VKAUDIO.EventArgs;
using VKAUDIO.Utils;
using UriBuilder = VKAUDIO.Utils.UriBuilder;

namespace VKAUDIO.Services
{
    public class AudioSearchService
    {
        public EventHandler<SearchingErrorArgs> Error { get; set; }
        private Dispatcher _dispatcher;
        private ObservableCollection<Audio> _audios;
        public AudioSearchService(Dispatcher dispatcher, ObservableCollection<Audio> audios)
        {
            _dispatcher = dispatcher;
            _audios = audios;
        }

        public void Search(string token, string query, int max)
        {
           
            Task.Factory.StartNew(() => search(token, query, max));
        }

        private void search(string token, string query, int max)
        {
            try
            {
                _dispatcher.Invoke(new Action(()=>_audios.Clear()));
                int localCount = 50;
                int offset = 0;
                while (localCount == 50 & offset < max)
                {
                    var request = UriBuilder.GetAudioSearchUrl(token, query, false, offset, 50, 2);
                    var response = RwpAppWebClient.GetResponse(request);
                    var respObj = JObject.Parse(response).SelectToken("response");
                    localCount = 0;
                    if (respObj == null)
                    {
                        return ;
                    }
                    foreach (var obj in respObj)
                    {
                        if (obj is JObject)
                        {
                            Audio audio = obj.ToObject<Audio>();
                            _dispatcher.Invoke(new Action(()=>_audios.Add(audio)));
                            localCount++;

                        }
                    }
                    offset += localCount;
                    Thread.Sleep(330);
                }
            }
            catch (Exception)
            {
                if (Error != null)
                {
                    Error(this, new SearchingErrorArgs());
                }
            }
        }
        
    }
}