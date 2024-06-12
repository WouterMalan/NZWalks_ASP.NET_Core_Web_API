using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public AuthController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }
        //POST api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };

            var identityResult = await this.userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (!identityResult.Succeeded)
            {
                return BadRequest(identityResult.Errors);
            }

            if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
            {
                await this.userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
            }

            return Ok("User created successfully");
        }

        //POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await this.userManager.FindByEmailAsync(loginRequestDto.UserName);

            if (user != null)
            {
                var checkPassword = await this.userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPassword)
                {
                    //Create token


                    return Ok("User logged in successfully");
                }
            }

            return BadRequest("Invalid login attempt");
        }
    }
}
