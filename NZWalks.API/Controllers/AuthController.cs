using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTOs.Auth;
using NZWalks.API.Models.DTOs.Register;
using NZWalks.API.Repositories.Token;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        #region  Ctor

        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        #endregion

        #region  Actions
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if (identityResult.Succeeded)
            {
                identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                if (identityResult.Succeeded)
                {
                    return Ok("user created successfully!");
                }
            }
            return BadRequest("something went wrong!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    var  roles  = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        //create token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken,
                        };
                    return Ok(response);
                    }
                }
            }
            return BadRequest("username or password is incorrect");
        }
        #endregion
    }
}
