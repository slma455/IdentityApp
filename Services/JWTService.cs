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

    //2nd step
    //Configure JWT Service
    public class JWTService
    {
        //_config: Holds application configuration (from appsettings.json)
        private readonly IConfiguration _config;
        //means the same key is used to sign and verify tokens
        public readonly SymmetricSecurityKey _jwtSecurityKey;

        public JWTService(IConfiguration configuration)
        {
            _config = configuration;
            //jwtkey is used for both encripting and decripting the jwt token 
            //SymmetricSecurityKey: مفتاح سري يستخدم لتوقيع وفك توقيع توكنات JWT


            _jwtSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:key"]));

        }
        // دالة لإنشاء توكن JWT للمستخدم
        //This method generates a JWT for the authenticated user.
        public string CreateJWT(User user)
        {
            //: قائمة بالمطالبات (Claims) التي سيحتويها التوكن:

        //    معرف المستخدم(ID) والبريد الإلكتروني
        //بنيجي هنا نعمل ال claim , بيبيقي شايل كل المعلومات 
        //و بعدين بنخزينها في jwt 

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
            //Combines the security key with the signing algorithm
            var credentials = new SigningCredentials(_jwtSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            int expiresInDays = int.Parse(_config["JWT:ExpiresInDays"] ?? "1");

            // is a crucial component in JWT generation that contains all the specifications needed to create a token.
            var tokenDescriptor = new SecurityTokenDescriptor
            {


            Subject = new ClaimsIdentity(userClaims), //The claims identity containing user information
                Expires = DateTime.UtcNow.AddDays(expiresInDays), //Token expiration (important for security)
                SigningCredentials = credentials, //How the token will be signed
                Issuer = _config["JWT:Issuer"] //Who created the token (your API)
            };
            //دا مسؤول عن انشاء التوكنات
            //The class that actually creates JWTs
            var tokenHandler = new JwtSecurityTokenHandler();
            //ينشئ التوكن بناءً على المواصفات
            // بعد ما عملنا التوكن ادنها للjwt
            // Generates the token based on the descriptor

            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            //WriteToken: يحول التوكن إلى سلسلة نصية (string) يمكن إرسالها للعميل
            // Serializes the token to a compact string format
            return tokenHandler.WriteToken(jwt);
        }

    }
}
