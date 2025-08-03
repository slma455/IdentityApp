using Microsoft.AspNetCore.Identity;
using System;

namespace webApplication.Models
{
    //5th step
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
