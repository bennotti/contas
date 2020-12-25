using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Contas.Core.Extensions
{
    public static class UserExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claims)
        {
            return claims.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        public static string GetUserId(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public static string GetUserToken(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "access_token")?.Value;
        }

        public static string GetNameComplete(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
        }

        public static string GetEmail(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
        }

        public static string GetCpf(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.User.FindFirst("cpf").Value;
        }
        public static string GetPhone(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.User.FindFirst("phone").Value;
        }
        public static string GetPhotoProfile(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.User.FindFirst("photo").Value;
        }

        public static string GetPhotoProfile(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x=> x.Type == "photo").Value;
        }
    }
}
