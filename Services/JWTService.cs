using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webApplication.Models;

namespace webApplication.Services
{
    public class JWTService
    {
        private readonly IConfiguration _config;
        public readonly SymmetricSecurityKey _jwtSecurityKey;

        public JWTService(IConfiguration configuration)
        {
            _config = configuration;
            //jwtkey is used for both encripting and decripting the jwt token 
            //SymmetricSecurityKey: مفتاح سري يستخدم لتوقيع وفك توقيع توكنات JWT


            _jwtSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:key"]));

        }
        // دالة لإنشاء توكن JWT للمستخدم
        public string CreateJWT(User user)
        {
            //: قائمة بالمطالبات (Claims) التي سيحتويها التوكن:

        //    معرف المستخدم(ID) والبريد الإلكتروني


            var userClaims = new List<Claim>
            {

                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Surname,user.LastName),
                new Claim("my own claim name","this is the value")
            };
            // System.IdentityModel.Tokens.Jwt;
            //SecurityAlgorithms.HmacSha256Signature ==> the most secure algorithm so far 
            var credentials = new SigningCredentials(_jwtSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(int.Parse(_config["JWT:ExpiresInDays"])),
                SigningCredentials = credentials,
                Issuer = _config["JWT:Issuer"]
            };
            //دا مسؤول عن انشاء التوكنات
            var tokenHandler = new JwtSecurityTokenHandler();
            //ينشئ التوكن بناءً على المواصفات
            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            //WriteToken: يحول التوكن إلى سلسلة نصية (string) يمكن إرسالها للعميل
            return tokenHandler.WriteToken(jwt);
        }

    }
}
