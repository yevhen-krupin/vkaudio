using System;
using System.IO;
using System.Net;
using System.Text;

namespace VKAUDIO
{
    public class RwpAppWebClient
    {
        public static string GetResponse(Uri uri)
        {
            var request = WebRequest.Create(uri);
            var response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                var streamReader = new StreamReader(stream, Encoding.UTF8);
                string str = streamReader.ReadToEnd();
                return str;
            }
        }
    }
}