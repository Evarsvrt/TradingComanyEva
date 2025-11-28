using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompanyEva.Domain.Entites
{
    public class User
    {
        public int UserId { get; set; } 
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        
        public Cart Cart { get; set; } 
        public ICollection<UserRole> UserRoles { get; set; } 
        
        
    }
}
