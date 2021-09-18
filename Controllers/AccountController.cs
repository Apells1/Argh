using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using PicNic.Models;
using Microsoft.AspNetCore.Authorization;

namespace PicNic.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IConfiguration _config;
        private UserManager<AppUser> _userManager;

        public TokenController(UserManager<AppUser> userManager, IConfiguration config)
        {
            _config = config;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<object>  RequestToken([FromBody]UserLogin login)
        {
            IActionResult response = Unauthorized();
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(login.Username);
                if (user != null)
                {
                    var result = await _userManager.CheckPasswordAsync(user, login.Password);
                    if (result)
                    {
                        response = Ok(new { token = BuildToken(user) });
                        // Check for role
                        // if (await _userManager.IsInRoleAsync(user, _config["Jwt:Role"]))
                        // {
                        //     response = Ok(new { token = BuildToken(user) });
                        // }
                        // else
                        // {
                        //     // 403 Forbidden
                        //     response = Forbid();
                        // }
                    }
                }
            }

            return response;
        }

        private string BuildToken(AppUser user)
        {
            var claims = new List<Claim> {
                //new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id)
                //new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                //new Claim(JwtRegisteredClaimNames.UniqueName, user.Email)
            };

            var userRoles = _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles.Result)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                null, // issuer
                null, // audience
                claims,
                expires: DateTime.Now.AddDays(Int16.Parse(_config["Jwt:ValidFor"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 