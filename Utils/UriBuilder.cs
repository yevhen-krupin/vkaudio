using System;
using System.Text;
using VKAUDIO.Configuration;

namespace VKAUDIO.Utils
{
    public class UriBuilder
    {
        static string OAuthUrl = "https://oauth.vk.com/authorize";
        static string OAuthTokenUrl = "https://oauth.vk.com/access_token";
        static string APIUrl = "https://api.vk.com/method/";

        public static Uri GetLoginPageUrl(string clientId, string scope, string display)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(OAuthUrl)
                .Append("?client_id=")
                .Append(clientId)
                .Append("&scope=")
                .Append(scope)
                .Append("&redirect_url=google.com")
                .Append("&display=")
                .Append(display);
            return new Uri(sb.ToString());
        }

        public static Uri GetToken(string code, string clientId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(OAuthTokenUrl)
                .Append("?client_id=")
                .Append(clientId)
                .Append("&client_secret=")
                .Append(VKAPP_Config.Key)
                .Append("&code=")
                .Append(code)
                .Append("&redirect_url=google.com");

            return new Uri(sb.ToString());

        }

        public static Uri GetAudioSearchUrl(string token, string q, bool autoComplete, int offset, int count, int sort)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(APIUrl)
                .Append("audio.search")
                .Append("?q=")
                .Append(q)
                .Append("&auto_complete=")
                .Append(Convert.ToInt32(autoComplete))
                .Append("&sort=")
                .Append(sort)
                .Append("&offset=")
                .Append(offset)
                .Append("&count=")
                .Append(count)
                .Append("&access_token=")
                .Append(token);
            return new Uri(sb.ToString());
        }
    }
}