using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IndentityAPI.Infrastructure;
using IndentityAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IndentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private IdentityDbContext _db;
        private IConfiguration _configuration;
        public IdentityController(IdentityDbContext db, IConfiguration configuration)
        {
            this._db = db;
            this._configuration = configuration;
        }

        //POST /api/identity/auth/register
        [HttpPost("auth/register", Name = "Register")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<dynamic>> RegisterAsync(UserInfo model)
        {
            TryValidateModel(model);
            if (ModelState.IsValid)
            {
                var result = await _db.Users.AddAsync(model);
                await _db.SaveChangesAsync();
                return Created("", new
                {
                    Id = result.Entity.Id,
                    FirstName = result.Entity.FirstName,
                    LastName = result.Entity.LastName,
                    Email = result.Entity.Email
                });

                /*return Created("", new
                {
                    result.Entity.Id,
                    result.Entity.FirstName,
                    result.Entity.LastName,
                    result.Entity.Email
                });*/
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //POST /api/identity/auth/token
        [HttpPost("auth/token", Name = "GetToken")]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]

        public ActionResult<dynamic> GetToken(LoginModel model)
        {
            TryValidateModel(model);
            if (ModelState.IsValid)
            {
                var user = _db.Users.SingleOrDefault(x => x.Email == model.Email && x.Password == model.Password);
                if (user == null)
                {
                    return Unauthorized();
                }
                else
                {

                    return Ok(GenerateToken(user));
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        private dynamic GenerateToken(UserInfo user)
        {
            //JWT.io website

            /* for single audience
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            */

            //For multiple audience
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            //We need to create scope of multiple audience

            claims.Add(new Claim(JwtRegisteredClaimNames.Aud, _configuration.GetValue<string>("JWT:Audience"))); // For Multiple Audience

            //claims.Add(new Claim(JwtRegisteredClaimNames.Aud, "Catlog")); //Another audience


            //claims.Add(new Claim(ClaimTypes.Role, "Manager")); // this is role claim type


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:Secret")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("JWT:Issuer"),
               // audience: _configuration.GetValue<string>("JWT:Audience"), // this is for single audience
                audience: null, // we need to set null for multiple audience
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new
            {
                user.FirstName,
                user.LastName,
                token = tokenString
            };
        }
    }
}