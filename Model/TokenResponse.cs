using System.Security.Cryptography.X509Certificates;

namespace restaurant_app_API.Model
{
    public class TokenResponse
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
