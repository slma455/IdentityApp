using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
        private readonly JWTService _jwtService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(
            JWTService jwtService,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _jwtService = jwtService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token
        /// </summary>
        /// <param name="model">Login credentials</param>
        /// <returns>User information with JWT token</returns>
        [HttpPost("Login")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized("Invalid User");
            }

            if (!user.EmailConfirmed)
            {
                return Unauthorized("Please confirm your email");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid username or password");
            }

            return CreateApplicationUserDTO(user);
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="registerDTO">User registration information</param>
        /// <returns>Registration result</returns>
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Refreshes the user token
        /// </summary>
        /// <returns>User information with new JWT token</returns>
        [HttpGet("RefreshUserToken")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDTO>> RefreshUserToken()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            if (user == null) return Unauthorized("User not found");

            return CreateApplicationUserDTO(user);
        }

   
      

        #region Private methods
        private UserDTO CreateApplicationUserDTO(User user)
        {
            return new UserDTO
            {
                FirstName = user.FirstName,
                LasttName = user.LastName,
                JWT = _jwtService.CreateJWT(user)
            };
        }


        private async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
        #endregion
    }
}