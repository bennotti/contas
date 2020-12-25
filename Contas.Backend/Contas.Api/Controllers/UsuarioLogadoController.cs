using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contas.Api.Controllers
{
    using Contas.Core.Helpers;
    using Contas.Core.Services.Interfaces;
    using Contas.Core.ViewModels;
    using Contas.Core.ViewModels.Usuarios;

    [ApiExplorerSettings(GroupName = @"Usuario Logado")]
    [Route("api/meusDados")]
    [ApiController]
    [AuthorizeCustom]
    public class UsuarioLogadoController : ControllerBase
    {
        private readonly IUsuariosServices _usuarioServices;

        public UsuarioLogadoController(IUsuariosServices usuarioServices)
        {
            _usuarioServices = usuarioServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Route("")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> ObterUsuarioLogado()
        {
            return Ok(await _usuarioServices.ObterUsuarioLogado());
        }

    }
}
