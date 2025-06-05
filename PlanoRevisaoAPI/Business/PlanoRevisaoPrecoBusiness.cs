using PlanoRevisaoAPI.Entity;
using PlanoRevisaoAPI.Models;
using PlanoRevisaoAPI.ModelView;
using PlanoRevisaoAPI.Repository;

namespace PlanoRevisaoAPI.Business;

public class PlanoRevisaoPrecoBusiness : IPlanoRevisaoPrecoBusiness
{
    private readonly IPlanoRevisaoPrecoRepository _planoRevisaoPrecoRepository;
    private readonly IPlanoRevisaoTipoRepository _planoRevisaoTipoRepository;
    private readonly IPlanoRevisaoRepository _planoRevisaoRepository;
    private readonly IEmpresaRegiaoRepository _empresaRegiaoRepository;

    public PlanoRevisaoPrecoBusiness(IPlanoRevisaoTipoRepository planoRevisaoTipoRepository,
                                    IPlanoRevisaoPrecoRepository planoRevisaoPrecoRepository,
                                    IPlanoRevisaoRepository planoRevisaoRepository,
                                    IEmpresaRegiaoRepository empresaRegiaoRepository)
    {
        _planoRevisaoTipoRepository = planoRevisaoTipoRepository;
        _planoRevisaoPrecoRepository = planoRevisaoPrecoRepository;
        _planoRevisaoRepository = planoRevisaoRepository;
        _empresaRegiaoRepository = empresaRegiaoRepository;
    }

    public List<PlanoRevisaoPrecoModelView> PostPrecoRegioes(List<PlanoRevisaoPrecoModelView> listPrecosPorRegiao)
    {
        VerificaRegiaoDuplicada(listPrecosPorRegiao);

        foreach (var planoRevisaoPreco in listPrecosPorRegiao)
        {
            PostPrecoRevisoes(planoRevisaoPreco);
        }

        return listPrecosPorRegiao;
    }

    public PlanoRevisaoPrecoModelView PostPrecoRevisoes(PlanoRevisaoPrecoModelView planoRevisaoPrecoModelView)
    {
        var planoRevisaoTipo = _planoRevisaoTipoRepository.Get(x => x.ID_PLANO_REVISAO_TIPO == planoRevisaoPrecoModelView.IdPlanoRevisaoTipo && x.IN_REEMBOLSAR == "S");

        if (planoRevisaoTipo is null)
        {
            throw new Exception("Tipo de plano de revisão não encontrado ou não é reembolsável.");
        }

        ValidaValor(planoRevisaoPrecoModelView);

        var regioes = planoRevisaoPrecoModelView?.CdEmpresaRegiao?.Split(',').Select(s => int.Parse(s.Trim())).ToList();

        foreach (var regiao in regioes)
        {
            var tipoPrecoRegiaoExiste = _planoRevisaoPrecoRepository.Get(x => x.ID_PLANO_REVISAO_TIPO == planoRevisaoPrecoModelView.IdPlanoRevisaoTipo && x.ID_EMPRESA_REGIAO == regiao);

            if (tipoPrecoRegiaoExiste.Count() > 0)
                throw new Exception($"Preço já cadastrado para a regiao {regiao} neste plano de revisao!");

            var planoRevisaoPreco = Map(planoRevisaoPrecoModelView, regiao);
            _planoRevisaoPrecoRepository.Create(planoRevisaoPreco);
        }

        return planoRevisaoPrecoModelView;
    }

    public List<PlanoRevisaoPrecoSemIdModelView> ListPrecosVigentes()
    {
        var planosPrecosVigentes = _planoRevisaoPrecoRepository.ListPrecosVigentes();

        if (planosPrecosVigentes == null || !planosPrecosVigentes.Any())
        {
            throw new Exception("Nenhum preço vigente encontrado.");
        }

        return planosPrecosVigentes.Select(MapGet) // Remover os objetos "duplicados", de mesmo cdRegiao
                                        .GroupBy(p => new
                                        {
                                            p.IdPlanoRevisaoTipo,
                                            p.CdEmpresaRegiao,
                                            p.NuValor,
                                            p.DtVigenciaInicial,
                                            p.DtVigenciaFinal
                                        })
                                        .Select(g => g.First())
                                        .ToList();
    }

    public PlanoRevisaoPrecoModelView AtualizaValores(PlanoRevisaoPrecoModelView planoPreco)
    {
        /// A Api recebe apenas o cdGrupoRegiao e o id do tipo de revisao, são dados suficientes para selecionar os outros objetos que serão atualizados
        /// Mesmo recebendo apenas um objeto, poderá atualizar mais, já que um grupo de região pode ter mais de um plano de revisão preço associado a ele
        /// Exemplo o cd "1,2,5"

        var regioes = _empresaRegiaoRepository.Get(x => x.CD_GRUPO_REGIAO == planoPreco.CdEmpresaRegiao).ToList();

        var planoRevisaoPrecoByRegiaoAndTipo = _planoRevisaoPrecoRepository.Get(x => x.ID_PLANO_REVISAO_TIPO == planoPreco.IdPlanoRevisaoTipo
                                                                && regioes.Select(r => r.ID_EMPRESA_REGIAO).Contains(x.ID_EMPRESA_REGIAO));

        if (regioes.Count() != planoRevisaoPrecoByRegiaoAndTipo.Count())
            throw new Exception($"Regiões inválidas para o tipo {planoPreco.IdPlanoRevisaoTipo} de região {planoPreco.CdEmpresaRegiao}. Verifique!");

        ValidarAtualizacao(planoPreco);

        foreach (var item in planoRevisaoPrecoByRegiaoAndTipo)
        {
            var planoAtualizar = MapAtualizar(planoPreco, item.ID_EMPRESA_REGIAO, item.ID_PLANO_REVISAO_PRECO);
            _planoRevisaoPrecoRepository.Update(planoAtualizar);
        }

        return planoPreco;

    }

    public List<PlanoRevisaoPrecoModelView> AtualizaValoresList(List<PlanoRevisaoPrecoModelView> planoPreco)
    {
        VerificaRegioesUpdate(planoPreco);

        foreach (var item in planoPreco)
        {
            AtualizaValores(item);
        }

        return planoPreco;
    }

    public void DeletaPrecoRevisaoPorTipo(int planoRevisaoTipo)
    {
        var planosRevisaoPreco = _planoRevisaoPrecoRepository.Get(x => x.ID_PLANO_REVISAO_TIPO == planoRevisaoTipo).ToList();

        if (planosRevisaoPreco is null)
            throw new Exception("Plano de revisão preço não encontrado.");

        foreach (var item in planosRevisaoPreco)
        {
            _planoRevisaoPrecoRepository.Delete(item);
        }

    }

    private void ValidarAtualizacao(PlanoRevisaoPrecoModelView planoPreco)
    {
        var planoRevisaoPreco = _planoRevisaoPrecoRepository.GetByRegiaoAndTipo(planoPreco.IdPlanoRevisaoTipo, planoPreco.CdEmpresaRegiao).FirstOrDefault(); // Pegar apenas um, porque se vier mais, são iguais, mudando apenas a PK, que não é usada aqui

        if (planoRevisaoPreco is null)
            throw new Exception("Plano de revisão preço não encontrado.");

        var planoRevisaoAtual = _planoRevisaoTipoRepository.Get(x => x.ID_PLANO_REVISAO_TIPO == planoRevisaoPreco.ID_PLANO_REVISAO_TIPO).FirstOrDefault();
        var planoRevisaoAlterado = _planoRevisaoTipoRepository.Get(x => x.ID_PLANO_REVISAO_TIPO == planoPreco.IdPlanoRevisaoTipo).FirstOrDefault();

        if (planoRevisaoAtual?.ID_PLANO_REVISAO != planoRevisaoAlterado?.ID_PLANO_REVISAO)
            throw new Exception("Não é possível alterar o tipo de plano de revisão, pois ele pertence a outro plano de revisão.");

        if (planoRevisaoAlterado?.IN_REEMBOLSAR.ToUpper() != "S")
            throw new Exception("O tipo de plano de revisão selecionado não é reembolsável.");

        ValidaValor(planoPreco);
    }

    private void ValidaValor(PlanoRevisaoPrecoModelView plano)
    {
        if (plano.NuValor == 0)
            throw new Exception($"O valor do tipo de plano revisao {plano.IdPlanoRevisaoTipo} deve ser maior que zero!");
    }

    private void ValidaRegioes(List<int> regioes)
    {
        var regioesBanco = _empresaRegiaoRepository.GetAll().Select(er => er.ID_EMPRESA_REGIAO);

        if (regioes.Any(r => !regioesBanco.Contains(r)))
            throw new Exception("Uma ou mais regiões informadas não existem na base de dados.");

        if (!regioesBanco.OrderBy(x => x).SequenceEqual(regioes.OrderBy(x => x)))
            throw new Exception("Uma ou mais regiões não foram informadas. Verifique!");
    }

    private void VerificaRegiaoDuplicada(List<PlanoRevisaoPrecoModelView> listPrecosPorRegiao)
    {
        var listPrecosPorRegiaoPorTipo = listPrecosPorRegiao.GroupBy(p => p.IdPlanoRevisaoTipo).Select(g => g.Key).ToList();

        foreach (var tipo in listPrecosPorRegiaoPorTipo)
        {
            var listRegioes = new List<int>();

            var listPorTipo = listPrecosPorRegiao.Where(p => p.IdPlanoRevisaoTipo == tipo);

            foreach (var item in listPorTipo)
            {
                var regioes = item.CdEmpresaRegiao?.Split(',').Select(s => int.Parse(s.Trim())).ToList();
                if (regioes != null)
                {
                    foreach (var regiao in regioes)
                    {
                        if (listRegioes.Contains(regiao))
                        {
                            throw new Exception($"Região {regiao} duplicada para o tipo de revisão {item.IdPlanoRevisaoTipo}.");
                        }

                        listRegioes.Add(regiao);
                    }
                }
            }

            ValidaRegioes(listRegioes);
        }
    }

    private void VerificaRegioesUpdate(List<PlanoRevisaoPrecoModelView> listPrecosPorRegiao)
    {
        var listPrecosPorRegiaoPorTipo = listPrecosPorRegiao.GroupBy(p => p.IdPlanoRevisaoTipo).Select(g => g.Key).ToList();

        foreach (var tipo in listPrecosPorRegiaoPorTipo)
        {
            var listRegioesRecebidas = new List<int>();
            var regioesRecebidas = new List<int>();
            var regioesExistentesPorTipo = _planoRevisaoPrecoRepository.Get(x => x.ID_PLANO_REVISAO_TIPO == tipo).Select(x => x.ID_EMPRESA_REGIAO).OrderBy(x => x).ToList();

            var listPorTipo = listPrecosPorRegiao.Where(p => p.IdPlanoRevisaoTipo == tipo).ToList();

            foreach (var item in listPorTipo)
            {
                regioesRecebidas = item.CdEmpresaRegiao?.Split(',').Select(s => int.Parse(s.Trim())).ToList();

                if (regioesRecebidas != null)
                {
                    listRegioesRecebidas.AddRange(regioesRecebidas);
                }
            }

            ValidaRegioes(listRegioesRecebidas);

            bool listasSaoIguais = listRegioesRecebidas.OrderBy(x => x).SequenceEqual(regioesExistentesPorTipo);

            if (!listasSaoIguais)
                throw new Exception("Todas as regiões já existentes para o " + tipo + " devem ser informadas. Verifique!");
        }
    }

    private PlanoRevisaoPreco Map(PlanoRevisaoPrecoModelView plano, int regiao)
    {
        return new PlanoRevisaoPreco
        {
            ID_PLANO_REVISAO_PRECO = plano.IdPlanoRevisaoPreco,
            ID_PLANO_REVISAO_TIPO = plano.IdPlanoRevisaoTipo,
            ID_EMPRESA_REGIAO = regiao,
            NU_VALOR = plano.NuValor,
            DT_VIGENCIA_INICIAL = plano.DtVigenciaInicial,
            DT_VIGENCIA_FINAL = plano.DtVigenciaFinal
        };
    }

    private PlanoRevisaoPreco MapAtualizar(PlanoRevisaoPrecoModelView plano, int regiao, int idPlanoRevisaoPreco)
    {
        return new PlanoRevisaoPreco
        {
            ID_PLANO_REVISAO_PRECO = idPlanoRevisaoPreco,
            ID_PLANO_REVISAO_TIPO = plano.IdPlanoRevisaoTipo,
            ID_EMPRESA_REGIAO = regiao,
            NU_VALOR = plano.NuValor,
            DT_VIGENCIA_INICIAL = plano.DtVigenciaInicial,
            DT_VIGENCIA_FINAL = plano.DtVigenciaFinal
        };
    }

    private PlanoRevisaoPrecoSemIdModelView MapGet(PlanoRevisaoPrecoEntity planoRevisaoPreco)
    {
        return new PlanoRevisaoPrecoSemIdModelView
        {
            IdPlanoRevisaoTipo = planoRevisaoPreco.IdPlanoRevisaoTipo,
            CdEmpresaRegiao = planoRevisaoPreco.CdEmpresaRegiao,
            NuValor = planoRevisaoPreco.NuValor,
            DtVigenciaInicial = planoRevisaoPreco.DtVigenciaInicial,
            DtVigenciaFinal = planoRevisaoPreco.DtVigenciaFinal
        };
    }
}
