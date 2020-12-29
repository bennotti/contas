using Contas.Core.Extensions;
using Contas.Core.Helpers;
using Contas.Core.Models;
using Contas.Core.Services.Interfaces;
using Contas.Core.ViewModels;
using Contas.Core.ViewModels.Contas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contas.Core.Services.Commands
{
    public class ContasServices : IContasServices
    {
        private readonly IConfiguration _config;
        private ApplicationDbContext _db;

        public ContasServices(ApplicationDbContext db,
            IConfiguration config)
        {
            _config = config;
            _db = db;
        }

        public async Task<ResultBaseViewModel> AlterarConta(int contaId, AlterarContaRequestModel dados)
        {
            ResultBaseViewModel result = null;
            try
            {
                var conta = await _db.Contas.FirstOrDefaultAsync(p => p.ContaId == contaId && p.Ativo);

                if (conta == null)
                {
                    return new ResultBaseViewModel
                    {
                        Result = false,
                        Msg = "Conta inválida!"
                    };
                }

                var vencimento = dados.Vencimento.ToDatetime();
                var pagamento = dados.Pagamento.ToDatetime();

                if (!vencimento.HasValue)
                {
                    return new ResultBaseViewModel
                    {
                        Result = false,
                        Msg = "Data de vencimento inválido!"
                    };
                }

                if (!pagamento.HasValue)
                {
                    return new ResultBaseViewModel
                    {
                        Result = false,
                        Msg = "Data do pagamento é inválido!"
                    };
                }

                var qntDiasAtraso = RegraNegocioHelper.QuantidadeDias(vencimento.Value, pagamento.Value);

                var valorMulta = RegraNegocioHelper.CalcularMulta(dados.ValorOriginal, qntDiasAtraso);

                conta.Nome = dados.Nome;
                conta.ValorOriginal = dados.ValorOriginal;
                conta.ValorFinal = dados.ValorOriginal + valorMulta;
                conta.ValorMulta = valorMulta;
                conta.Vencimento = vencimento.Value;
                conta.Pagamento = pagamento.Value;
                conta.QntDiasAtraso = qntDiasAtraso;

                _db.Entry(conta).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await _db.SaveChangesAsync();

                result = new ResultBaseViewModel
                {
                    Result = true,
                    Msg = "Conta cadastrada com sucesso."
                };

            }
            catch (Exception ex)
            {
                result = new ResultBaseViewModel
                {
                    Result = false,
                    Msg = ex.ToString()
                };
            }

            return result;
        }

        public CalcularMultaViewModel CalcularMulta(CalcularMultaRequestModel dados)
        {
            CalcularMultaViewModel result = null;
            try
            {
                var vencimento = dados.Vencimento.ToDatetime();
                var pagamento = dados.Pagamento.ToDatetime();

                if (!vencimento.HasValue)
                {
                    return new CalcularMultaViewModel
                    {
                        Result = false,
                        Msg = "Data de vencimento inválido!"
                    };
                }

                if (!pagamento.HasValue)
                {
                    return new CalcularMultaViewModel
                    {
                        Result = false,
                        Msg = "Data do pagamento é inválido!"
                    };
                }

                var qntDiasAtraso = RegraNegocioHelper.QuantidadeDias(vencimento.Value, pagamento.Value);

                var valorMulta = RegraNegocioHelper.CalcularMulta(dados.ValorOriginal, qntDiasAtraso);


                result = new CalcularMultaViewModel
                {
                    Result = true,
                    Msg = "Multa calculada.",
                    ValorMulta = valorMulta,
                    ValorOriginal = dados.ValorOriginal,
                    QntDiasAtraso = qntDiasAtraso,
                    ValorFinal = dados.ValorOriginal + valorMulta
                };

            }
            catch (Exception ex)
            {
                result = new CalcularMultaViewModel
                {
                    Result = false,
                    Msg = ex.ToString()
                };
            }

            return result;
        }

        public async Task<ResultBaseViewModel> NovaConta(NovaContaRequestModel dados)
        {
            ResultBaseViewModel result = null;
            try{
                var vencimento = dados.Vencimento.ToDatetime();
                var pagamento = dados.Pagamento.ToDatetime();

                if (!vencimento.HasValue)
                {
                    return new ResultBaseViewModel
                    {
                        Result = false,
                        Msg = "Data de vencimento inválido!"
                    };
                }

                if (!pagamento.HasValue)
                {
                    return new ResultBaseViewModel
                    {
                        Result = false,
                        Msg = "Data do pagamento é inválido!"
                    };
                }

                var qntDiasAtraso = RegraNegocioHelper.QuantidadeDias(vencimento.Value, pagamento.Value);

                var valorMulta = RegraNegocioHelper.CalcularMulta(dados.ValorOriginal, qntDiasAtraso);

                var conta = new TbContas {
                    Nome = dados.Nome,
                    ValorOriginal = dados.ValorOriginal,
                    ValorFinal = dados.ValorOriginal + valorMulta,
                    ValorMulta = valorMulta,
                    Vencimento = vencimento.Value,
                    Pagamento = pagamento.Value,
                    QntDiasAtraso = qntDiasAtraso
                };

                _db.Contas.Add(conta);

                await _db.SaveChangesAsync();

                result = new ResultBaseViewModel
                {
                    Result = true,
                    Msg = "Conta cadastrada com sucesso."
                };
            }
            catch (Exception ex)
            {
                result = new ResultBaseViewModel
                {
                    Result = false,
                    Msg = ex.ToString()
                };
            }

            return result;
        }

        public async Task<ContasCollectionResultViewModel> ObterPesquisar(FiltroContaRequestModel filtro)
        {
            ContasCollectionResultViewModel result = null;
            try
            {

                var query = _db.Contas.Where(p => p.Ativo);

                if (!string.IsNullOrEmpty(filtro.Nome))
                {
                    query = query.Where(p => p.Nome.ToLower().Contains(filtro.Nome.ToLower()));
                }

                if (filtro.Valor.HasValue && filtro.TipoComparacaoValor != Enums.TipoComparacao.Nenhuma)
                {
                    switch (filtro.TipoComparacaoValor)
                    {
                        case Enums.TipoComparacao.Igual:
                            {
                                query = query.Where(p => p.ValorFinal == filtro.Valor.Value);
                                break;
                            }
                        case Enums.TipoComparacao.MaiorIgual:
                            {
                                query = query.Where(p => p.ValorFinal >= filtro.Valor.Value);
                                break;
                            }
                        case Enums.TipoComparacao.MenorIgual:
                            {
                                query = query.Where(p => p.ValorFinal <= filtro.Valor.Value);
                                break;
                            }
                    }
                }

                var lista = await query.ToListAsync();

                result = new ContasCollectionResultViewModel
                {
                    Result = true,
                    Msg = "Contas recuperadas",
                    TotalRegistros = lista.Count,
                    Contas = lista.Select(p => new ContaResultViewModel
                    {
                        ContaId = p.ContaId,
                        Nome = p.Nome,
                        Pagamento = p.Pagamento.ToString("dd/MM/yyyy"),
                        Vencimento = p.Vencimento.ToString("dd/MM/yyyy"),
                        QntDiasAtraso = p.QntDiasAtraso,
                        ValorFinal = p.ValorFinal,
                        ValorMulta = p.ValorMulta,
                        ValorOriginal = p.ValorOriginal
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                result = new ContasCollectionResultViewModel
                {
                    Result = false,
                    Msg = ex.ToString(),
                    TotalRegistros = 0,
                    Contas = null
                };
            }

            return result;
        }

        public async Task<ContaResultViewModel> ObterPorId(int contaId)
        {
            ContaResultViewModel result = null;
            try
            {
                var dados = await _db.Contas.FirstOrDefaultAsync(p => p.ContaId == contaId && p.Ativo);

                if (dados == null)
                {
                    return new ContaResultViewModel
                    {
                        Result = false,
                        Msg = "Conta inválida!"
                    };
                }

                result = new ContaResultViewModel
                {
                    Result = true,
                    Msg = "Conta Valida!",
                    ContaId = dados.ContaId,
                    Nome = dados.Nome,
                    Pagamento = dados.Pagamento.ToString("dd/MM/yyyy"),
                    Vencimento = dados.Vencimento.ToString("dd/MM/yyyy"),
                    QntDiasAtraso = dados.QntDiasAtraso,
                    ValorFinal = dados.ValorFinal,
                    ValorMulta = dados.ValorMulta,
                    ValorOriginal = dados.ValorOriginal
                };
            }
            catch (Exception ex)
            {
                result = new ContaResultViewModel
                {
                    Result = false,
                    Msg = ex.ToString()
                };
            }

            return result;
        }

        public async Task<ContasCollectionResultViewModel> ObterUltimasAtualizacoes()
        {
            ContasCollectionResultViewModel result = null;
            try
            {
                var lista = await _db.Contas.Where(p => p.Ativo).OrderBy(p => p.DataUltimaAlteracao).Take(10).ToListAsync();

                result = new ContasCollectionResultViewModel {
                    Result = true,
                    Msg = "Contas recuperadas",
                    TotalRegistros = lista.Count,
                    Contas = lista.Select(p => new ContaResultViewModel { 
                        ContaId = p.ContaId,
                        Nome = p.Nome,
                        Pagamento = p.Pagamento.ToString("dd/MM/yyyy"),
                        Vencimento = p.Vencimento.ToString("dd/MM/yyyy"),
                        QntDiasAtraso = p.QntDiasAtraso,
                        ValorFinal = p.ValorFinal,
                        ValorMulta = p.ValorMulta,
                        ValorOriginal = p.ValorOriginal
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                result = new ContasCollectionResultViewModel
                {
                    Result = false,
                    Msg = ex.ToString(),
                    TotalRegistros = 0,
                    Contas = null
                };
            }

            return result;
        }

        public async Task<ResultBaseViewModel> RemoverConta(int contaId)
        {
            ResultBaseViewModel result = null;
            try
            {
                var conta = await _db.Contas.FirstOrDefaultAsync(p => p.ContaId == contaId && p.Ativo);

                if (conta == null)
                {
                    return new ResultBaseViewModel
                    {
                        Result = false,
                        Msg = "Conta inválida!"
                    };
                }

                conta.Ativo = false;

                _db.Entry(conta).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await _db.SaveChangesAsync();

                result = new ResultBaseViewModel
                {
                    Result = true,
                    Msg = "Conta cadastrada com sucesso."
                };
            }
            catch (Exception ex)
            {
                result = new ResultBaseViewModel
                {
                    Result = false,
                    Msg = ex.ToString()
                };
            }

            return result;
        }
    }
}
