using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AutoMapper.Configuration;
using Book_Store.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Book_Store.Authorization
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = context.HttpContext.Request.Cookies["Token"];

            if (token == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = jsonToken as JwtSecurityToken;
                
                DateTimeOffset expires = DateTimeOffset.FromUnixTimeSeconds(long.Parse(tokenS.Claims.FirstOrDefault(claim => claim.Type == "exp").Value));

                if (expires < DateTime.UtcNow)
                {
                    context.HttpContext.Response.Cookies.Delete("Token");
                    context.Result = new JsonResult(new { message = "Token Expired" }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
        }   
    }
}
