using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;
using VKAUDIO.Configuration;

namespace VKAUDIO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            OAuthWb.LoadCompleted += OAuthWbOnLoadCompleted;
            startLogin();
        }

        private void startLogin()
        {
            try
            {
                var uri = UriBuilder.GetLoginPageUrl(VKAPP_Config.ClientId, "audio", "mobile");
                OAuthWb.Source = uri;
            }
            catch (Exception)
            {

                if (
                    MessageBox.Show("Попытка логина не удалась, повторить попытку?", "Логин не удался",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    startLogin();
                }
                else
                {
                    Close();
                }
            }
           
        }

        private void OAuthWbOnLoadCompleted(object sender, NavigationEventArgs navigationEventArgs)
        {
            try
            {
                string str = OAuthWb.Source.AbsoluteUri;
                string code = getCode(str);
                if (code != string.Empty)
                {
                    var tokenObj = getToken(code);
                    JObject jObject = JObject.Parse(tokenObj);
                    OAuthToken oAuthToken = new OAuthToken() { Token = jObject["access_token"].ToString() };
                    var oauthController = new OAuthController(oAuthToken);
                    RWPAppWindow rwpAppWindow = new RWPAppWindow();
                    rwpAppWindow.Open(oauthController);
                    Close();
                }
            }
            catch (Exception)
            {
                if (
                    MessageBox.Show("Попытка логина не удалась, повторить попытку?", "Логин не удался",
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    startLogin();
                }
                else
                {
                    Close();
                }
            }
            
        }

        private string getCode(string url)
        {
            string result = "";
            string separator = "code=";
            string[] array = url.Split(new string[1] {separator}, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length > 1)
            {
                result = array.Last();
            }
            return result;
        }

        private string getToken(string code)
        {
            using (var wc = new WebClient())
            {
                var response = wc.DownloadString(UriBuilder.GetToken(code, VKAPP_Config.ClientId));
                return response;
            }
        }
    }


    

    public class OAuthToken
    {
        public string Token { get; set; }
    }

    public class OAuthController
    {
        public OAuthToken OAuthToken { get; set; }
        public OAuthController(OAuthToken token)
        {
            OAuthToken = token;
        }
    }
}
