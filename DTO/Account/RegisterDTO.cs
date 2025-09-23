using System.ComponentModel.DataAnnotations;

namespace webApplication.DTO.Account
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(15,MinimumLength =5,ErrorMessage ="First Name must be more than 5 less than or equal 15")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15,MinimumLength =5,ErrorMessage = "Last Name must be more then 5 less than or equal 15")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessage ="Invalid Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Invalid Password")]

        public string Password { get; set; }
    }
}
