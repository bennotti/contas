using Contas.Core.Models;
using Contas.Core.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Contas.Core.Handlers
{
    public class SwaggerBasicAuthMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        private readonly IOptions<SwaggerConfiguration> _swaggerConfiguration;

        public SwaggerBasicAuthMiddleware(RequestDelegate next, IOptions<SwaggerConfiguration> swaggerConfig,
            IConfiguration config)
        {
            _next = next;
            _config = config;
            _swaggerConfiguration = swaggerConfig;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Make sure we are hitting the swagger path, and not doing it locally as it just gets annoying :-)
            if (context.Request.Path.StartsWithSegments("/swagger") && IsUsar())
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic "))
                {
                    // Get the encoded username and password
                    var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

                    // Decode from Base64 to string
                    var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                    // Split username and password
                    var username = decodedUsernamePassword.Split(':', 2)[0];
                    var password = decodedUsernamePassword.Split(':', 2)[1];

                    // Check if login is correct
                    if (IsAuthorized(username, password))
                    {
                        await _next.Invoke(context);
                        return;
                    }
                }

                // Return authentication type (causes browser to show login dialog)
                context.Response.Headers["WWW-Authenticate"] = "Basic";

                // Return unauthorized
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        public bool IsAuthorized(string username, string password)
        {
            // Check that username and password are correct
            return username.Equals(_swaggerConfiguration.Value.Usuario, StringComparison.InvariantCultureIgnoreCase)
                && password.Equals(_swaggerConfiguration.Value.Senha);
        }

        public bool IsUsar(){
            return _swaggerConfiguration.Value.UsarAutenticacao;
        }
    }
}
