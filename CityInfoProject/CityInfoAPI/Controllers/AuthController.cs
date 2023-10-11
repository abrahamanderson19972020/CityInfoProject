using CityInfoAPI.Models.Dtos;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityInfoAPI.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _autheticationService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthenticationService autheticationService, IConfiguration configuration)
        {
            _autheticationService = autheticationService;
            _configuration = configuration;
        }
        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(LoginRequest loginRequest)
        {
            //Step 1: Validate username/password
            var user = _autheticationService.ValidateCredentials(loginRequest.Username, loginRequest.Password);
            if(user == null)
            {
                return Unauthorized();
            }

            //Step 2: Create Token
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"])); //Get security key and transform to byte array
            var signingCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256); //Credentials that is used to sign the token
                                                                                                        //Below we add user info so that token can contain info who the user is with claims
                                                                                                        // A claim contains user-related information                                                                                         
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub",user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.Firstname));
            claimsForToken.Add(new Claim("family_name", user.Lastname));
            claimsForToken.Add(new Claim("address", user.Address));
            // Create JWTSecurtiyToken
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claimsForToken,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: signingCredentials);
            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return Ok(tokenToReturn);

        }
    }
}
