using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace restaurant_app_API.Model
{
    [Keyless]
    public class UserModel
    {
      
        public int Id { get; set; }
    
        public string Username { get; set; }
        
        public string Password { get; set; }
      
        public string Role { get; set; }

        public bool IsActive { get; set; }
        public string ?StatusName { get; set; }
    }
}