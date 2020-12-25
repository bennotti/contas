using Contas.Core.Helpers;
using Contas.Core.Services.Interfaces;
using Contas.Core.ViewModels;
using Contas.Core.ViewModels.Contas;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contas.Api.Controllers
{
    [ApiExplorerSettings(GroupName = @"Contas")]
    [Route("api/contas")]
    [ApiController]
    [AuthorizeCustom]
    public class ContasController : ControllerBase
    {
        private readonly IContasServices _contasServices;

        public ContasController(IContasServices contasServices)
        {
            _contasServices = contasServices;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> ObterUltimasAtualizacoes()
        {
            var resultado = await _contasServices.ObterUltimasAtualizacoes();
            return resultado.Result ? Ok(resultado) : BadRequest(resultado);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{contaId}")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> ObterContaPorId([FromRoute]int contaId)
        {
            var resultado = await _contasServices.ObterPorId(contaId);
            return resultado.Result ? Ok(resultado) : BadRequest(resultado);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("pesquisar")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> ObterPesquisar([FromQuery]FiltroContaRequestModel filtro)
        {
            if (filtro == null) filtro = FiltroContaRequestModel.Default;

            var resultado = await _contasServices.ObterPesquisar(filtro);
            return resultado.Result ? Ok(resultado) : BadRequest(resultado);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("calcularMulta")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public IActionResult CalcularMulta([FromBody] CalcularMultaRequestModel dados)
        {
            if (dados == null || !ModelState.IsValid)
            {
                return BadRequest(new { result = false, msg = "Dados Inválidos!" });
            }
            var resultado = _contasServices.CalcularMulta(dados);
            return resultado.Result ? Ok(resultado) : BadRequest(resultado);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> NovaConta([FromBody]NovaContaRequestModel dados)
        {
            if (dados == null || !ModelState.IsValid)
            {
                return BadRequest(new { result = false, msg = "Dados Inválidos!" });
            }
            var resultado = await _contasServices.NovaConta(dados);
            return resultado.Result ? Ok(resultado) : BadRequest(resultado);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("{contaId}")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> AlterarConta([FromRoute] int contaId,[FromBody] AlterarContaRequestModel dados)
        {
            if (dados == null || !ModelState.IsValid)
            {
                return BadRequest(new { result = false, msg = "Dados Inválidos!" });
            }
            var resultado = await _contasServices.AlterarConta(contaId, dados);
            return resultado.Result ? Ok(resultado) : BadRequest(resultado);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{contaId}")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> RemoverConta([FromRoute] int contaId)
        {
            var resultado = await _contasServices.RemoverConta(contaId);
            return resultado.Result ? Ok(resultado) : BadRequest(resultado);
        }
    }
}
