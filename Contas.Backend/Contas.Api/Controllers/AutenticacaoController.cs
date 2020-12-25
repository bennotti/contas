using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Contas.Api.Controllers
{
    using Contas.Api.Services;
    using Contas.Core.Helpers;
    using Contas.Core.Services.Interfaces;
    using Contas.Core.ViewModels;
    using Contas.Core.ViewModels.Authentication;
    using Contas.Core.ViewModels.Usuarios;
    using Microsoft.AspNetCore.Authorization;

    [ApiExplorerSettings(GroupName = @"Autenticacao")]
    [Route("api/autenticacao")]
    [ApiController]
    [AllowAnonymous]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IAutenticacaoServices _authenticationServices;

        public AutenticacaoController(IAutenticacaoServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Authenticate([FromBody] AutenticacaoRequestViewModel user)
        {
            var resultado = await _authenticationServices.Login(user, ipAddress());

            return Ok(resultado);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public IActionResult RemoverToken()
        {
            return Ok(new ResultBaseViewModel { Result = true, Msg= "" });
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        private string tokenRequest()
        {
            if (Request.Headers.ContainsKey("Authorization"))
                return Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            else
                return null;
        }
    }
}