using System.Security.Cryptography;
using postgreanddotnet.Data;
using restaurant_app_API.Entity;
using restaurant_app_API.Service;

namespace restaurant_app_API.Container
{
    public class RefreshHandler : IRefreshHandler
    {
        private readonly AppDbContex context;

        public RefreshHandler(AppDbContex appDbContex)
        {
            this.context = appDbContex;
        }

        public async Task<string> GenerateToken(int userId)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                string RefreshToken = Convert.ToBase64String(randomNumber);

                var Existtoken = context.User_Tokens.FirstOrDefault(u => u.UserId == userId);

                if (Existtoken != null)
                {
                    Existtoken.RefreshToken = RefreshToken;
                    context.User_Tokens.Update(Existtoken);
                    await context.SaveChangesAsync();
                }
                else
                {
                    var token = new User_Tokens
                    {
                        UserId = userId,
                        RefreshToken = RefreshToken
                    };
                    context.User_Tokens.Add(token);
                    await context.SaveChangesAsync();
                }

                return RefreshToken;


            }
        }
    }
}
