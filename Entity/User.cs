using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_app_API.Entity
{
    [Table("users", Schema = "public")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        [MaxLength(255)]
        public string Username { get; set; }

        [Column("password")]
        [MaxLength(255)]
        public string Password { get; set; }

        [Column("role")]
        [MaxLength(255)]
        public string Role { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        public User_Tokens? user_Tokens { get; set; }


    }
}