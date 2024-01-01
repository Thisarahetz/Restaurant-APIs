using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_app_API.Entity
{
    [Table("user_tokens", Schema = "public")]
    public class User_Tokens
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("token")]
        public string Token { get; set; }

        [Column("refresh_token")]
        public string RefreshToken { get; set; }

        public User user { get; set; }
    }
}
