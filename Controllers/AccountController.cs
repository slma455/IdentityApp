using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using webApplication.DTO.Account;
using webApplication.Models;
using webApplication.Services;

namespace webApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTService _JWTService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public AccountController(JWTService jWTService , SignInManager<User> signInManager ,UserManager<User> userManager)
        {
            _JWTService = jWTService;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        #region private methods


        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized("Invalid User");
            }
            if (user.EmailConfirmed == false)
                return Unauthorized("please confirm your email");
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid username or password");
            return CreateApplicationUserDTO(user);
        }
        private UserDTO CreateApplicationUserDTO(User user)
        {
            return new UserDTO {
            firstName = user.FirstName,
            lasttName = user.LastName,
            JWT = _JWTService.CreateJWT(user)
            };
        }
        #endregion
    }
}
