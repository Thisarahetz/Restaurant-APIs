using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using postgreanddotnet.Data;

namespace restaurant_app_API.Helper
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly AppDbContex context;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, AppDbContex appDbContex) : base(options, logger, encoder, clock)
        {
            this.context = appDbContex;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
            }

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);


            var bytes = Convert.FromBase64String(authHeader.Parameter);
            string str = Encoding.UTF8.GetString(bytes);

            string[] credentials = str.Split([',']);

            Debug.WriteLine(
                "Username: " + credentials[0].Split(':')[1].Trim() +
                "Password: " + credentials[1].Split(':')[1].Trim()
            );

            // Trim any extra whitespace from the username and password
            string username = credentials[0].Split(':')[1].Trim();
            string password = credentials[1].Split(':')[1].Trim();
            var user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
            }
            else
            {
                var claims = new[] { new Claim(ClaimTypes.Name, user.Username), new Claim(ClaimTypes.Role, user.Role) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }


        }
    }
}
