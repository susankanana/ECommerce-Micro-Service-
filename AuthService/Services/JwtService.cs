﻿using AuthService.Models;
using AuthService.Services.IServices;
using AuthService.Utilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Services
{
    public class JwtService : IJwt
    {
        private readonly JwtOptions _jwtOptions;
        public JwtService(IOptions<JwtOptions> options)
        {
            _jwtOptions = options.Value;
        }
        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> Roles)
        {
            
            //key-secret key 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));

            //cred security algorithm
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //payload

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id.ToString()));
            //Adding a list of roles to our payload
            claims.AddRange(Roles.Select(x => new Claim(ClaimTypes.Role, x)));

            var tokendescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                Expires = DateTime.UtcNow.AddHours(3),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = cred
            };

            var token = new JwtSecurityTokenHandler().CreateToken(tokendescriptor);
            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}
