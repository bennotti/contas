
using Contas.Core.ViewModels;
using Contas.Core.ViewModels.Contas;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contas.Core.Services.Interfaces
{
    public interface IContasServices
    {
        Task<ContasCollectionResultViewModel> ObterUltimasAtualizacoes();
        Task<ContaResultViewModel> ObterPorId(int contaId);
        Task<ContasCollectionResultViewModel> ObterPesquisar(FiltroContaRequestModel filtro);
        CalcularMultaViewModel CalcularMulta(CalcularMultaRequestModel dados);
        Task<ResultBaseViewModel> NovaConta(NovaContaRequestModel dados);
        Task<ResultBaseViewModel> AlterarConta(int contaId, AlterarContaRequestModel dados);
        Task<ResultBaseViewModel> RemoverConta(int contaId);
    }
}
