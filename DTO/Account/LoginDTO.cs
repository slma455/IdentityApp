using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace webApplication.DTO.Account
{
    public class LoginDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
