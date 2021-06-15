using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MultimediaCenter.Data;
using MultimediaCenter.Models;
using MultimediaCenter.ViewModels;
using MultimediaCenter.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MultimediaCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;


        public AuthenticationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> RegisterUser(RegisterRequest registerRequest)
        {

            var user = new ApplicationUser
            {
                Email = registerRequest.Email,
                UserName = registerRequest.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (result.Succeeded)
            {
                return Ok(new RegisterResponse { ConfirmationToken = user.SecurityStamp });
            }
            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("confirm")]

        public async Task<ActionResult> ConfirmUser(ConfirmUserRequest confirmUserRequest)
        {
            var toConfirm = _context.ApplicationUsers
                .Where(u => u.Email == confirmUserRequest.Email && u.SecurityStamp == confirmUserRequest.ConfirmationToken).
                FirstOrDefault();
            if (toConfirm != null)
            {
                toConfirm.EmailConfirmed = true;
                _context.Entry(toConfirm).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                {
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
                    //new Claim(JwtRegisteredClaimNames.)
                };
                    var signinKey = new SymmetricSecurityKey(
                      Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

                    int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

                    var token = new JwtSecurityToken(
                      issuer: _configuration["Jwt:Site"],
                      audience: _configuration["Jwt:Site"],
                      expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                      signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256),
                      claims: claims
                    );

                    return Ok(
                      new
                      {
                          token = new JwtSecurityTokenHandler().WriteToken(token),
                          expiration = token.ValidTo
                      });
                }

            }
            return Unauthorized();
        }

    }
}
