using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using bagit_api.Data;
using bagit_api.Models;
using bagit_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace bagit_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly BagItDbContext _context;
        private readonly SecurityService _security;
        private readonly IConfiguration _configuration;
        public UserController(BagItDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _security = new SecurityService(Options.Create(new HashingOptions()));
            
        }

        [HttpGet]
        public string Get()
        {
            return "BagIt API v1.2";
        }
        

        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> RegisterUser([FromBody] User user)
        {
            try
            {
                List<string> errors = VerifyRegistration(user);

                if (errors.Count != 0)
                {
                    return BadRequest(errors[0]);
                }

                // Hash, Salt and Pepper Password
                IOptions<HashingOptions> options = Options.Create(new HashingOptions());
                SecurityService security = new SecurityService(options);
                user.Password = security.Hash(user.Password);

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                
                return Ok();
            }
            catch (Exception e)
            {
                return Problem();
            }
        }
        
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] User _user)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == _user.Email);
            if (user != null && _security.Check(user.Password, _user.Password))
            {
                var claim = new[] {
                    new Claim("id", user.UserId.ToString())
                };
                var signinKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

                int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Site"],
                    audience: _configuration["Jwt:Site"],
                    claims: claim,
                    expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                    signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(
                    new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
            }
            return Unauthorized();

        }

        private List<string> VerifyRegistration(User user)
        {
            List<string> errors = new List<string>();

            if (isUsernameTaken(user.Username))
            {
                errors.Add("Username is already taken.");
            }
            
            if (isEmailTaken(user.Email))
            {
                errors.Add("Email is already taken.");
            }

            return errors;
        }
        
        private bool isUsernameTaken(string username)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == username);

            return user != null;
        }
        
        private bool isEmailTaken(string email)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == email);

            return user != null;
        }
    }
}
