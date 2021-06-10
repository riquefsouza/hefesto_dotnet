using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using hefesto.admin.Models;
using hefesto.base_hefesto.Models;
using Newtonsoft.Json;
using hefesto.admin.Services;

namespace hefesto_dotnet_webapi.base_hefesto.Controllers
{
    [Route("/auth")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly IAdmUserService _userService;

        public LoginController(IConfiguration config, IAdmUserService userService)
        {
            _configuration = config;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AdmUser _userData)
        {

            if (_userData != null && _userData.Login != null && _userData.Password != null)
            {
                var user = await _userService.Authenticate(_userData.Login, _userData.Password);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("id", user.Id.ToString()),
                    new Claim("name", user.Name),
                    new Claim("email", user.Email)
                   };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], 
                        claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    var stoken = new JwtSecurityTokenHandler().WriteToken(token);    

                    var tokenDTO = new TokenDTO(stoken, "Bearer");

                    return Ok(JsonConvert.SerializeObject(tokenDTO));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

    }
}