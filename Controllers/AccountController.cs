using Azure.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterDTO registerDTO)
        {
            if (await CheckEmailExistAsync(registerDTO.Email))
            {
                return new JsonResult(new
                {
                    success = false,
                    message = $"An existing account is using {registerDTO.Email}, please try another one"
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            var userToAdd = new User
            {
                FirstName = registerDTO.FirstName.ToLower(),
                LastName = registerDTO.LastName.ToLower(),
                Email = registerDTO.Email.ToLower(),
                UserName = registerDTO.Email.ToLower(),
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(userToAdd, registerDTO.Password);

            if (!result.Succeeded)
            {
                return new JsonResult(new
                {
                    success = false,
                    errors = result.Errors
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            return new JsonResult(new
            {
                success = true,
                title = "Account Created",
                message = "Your account has been created, you can login"
            });
        }

        #region private methods
        private UserDTO CreateApplicationUserDTO(User user)
        {
            return new UserDTO
            {
                firstName = user.FirstName,
                lasttName = user.LastName,
                JWT = _JWTService.CreateJWT(user)
            };
        }

        private async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
        #endregion
    }
}
