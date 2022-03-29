using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using bagit_api.Data;
using bagit_api.Models;
using bagit_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace bagit_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly BagItDbContext _context;
        public UserController(BagItDbContext context)
        {
            _context = context;
        }
        // GET: api/User
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
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
                
                return NoContent();
            }
            catch (Exception e)
            {
                return Problem();
            }
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

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
