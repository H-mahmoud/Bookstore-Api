using Book_Store.Models;
using Book_Store.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        public IConfiguration _config { get; }
        public BookStoreContext _context { get; }

        public UsersController(IConfiguration config, BookStoreContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginViewModel login)
        {
            if (ModelState.IsValid) {

                IActionResult response = Unauthorized();
                var user = _context.User.FirstOrDefault(x => x.Email == login.Email && x.Password == login.Password);

                if (user != null)
                {
                    var tokenString = user.JwtToken.Token;

                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(tokenString);
                    var tokenS = jsonToken as JwtSecurityToken;

                    DateTimeOffset expires = DateTimeOffset.FromUnixTimeSeconds(long.Parse(tokenS.Claims.FirstOrDefault(claim => claim.Type == "exp").Value));



                    if (expires < DateTime.UtcNow)
                    {
                        tokenString = GenerateJSONWebToken(user);
                        
                        user.JwtToken.Token = tokenString;
                        _context.SaveChanges();
                    }

                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddDays(7)
                    };

                    Response.Cookies.Append("Token", tokenString, cookieOptions);

                    response = Ok(new { token = tokenString });
                }

                return response;
            }

            return BadRequest("Invalid Data Input");
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                IActionResult response = Unauthorized();
                UserModel user = _context.User.FirstOrDefault(x => x.Email == register.Email);

                if (user == null)
                {
                    UserModel NewUser = new UserModel();
                    NewUser.Id = Guid.NewGuid().ToString();
                    NewUser.Email = register.Email;
                    NewUser.Password = register.Password;

                    JwtTokensModel NewToken = new JwtTokensModel();
                    NewToken.Id = Guid.NewGuid().ToString();
                    NewToken.Token = GenerateJSONWebToken(NewUser);
                    _context.JwtToken.Add(NewToken);

                    NewUser.JwtTokenId = NewToken.Id;
                    _context.User.Add(NewUser);

                    _context.SaveChanges();

                    return Ok();
                }
                else
                {
                    return Unauthorized("Email Already Used");
                }
            }

            return BadRequest("Invalid Data Input");
        }

        private string GenerateJSONWebToken(UserModel User)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecretKey:Jwt"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim("Id", User.Id),
                new Claim("Email", User.Email),
            };

            var token = new JwtSecurityToken(
              _config["SecretKey:Issuer"],
              _config["SecretKey:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
