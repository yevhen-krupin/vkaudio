namespace VKAUDIO.OAuth
{
    public class OAuthController
    {
        public OAuthToken OAuthToken { get; set; }
        public OAuthController(OAuthToken token)
        {
            OAuthToken = token;
        }
    }
}