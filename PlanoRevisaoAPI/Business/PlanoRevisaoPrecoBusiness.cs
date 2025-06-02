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

    public List<PlanoRevisaoPrecoModelView> PostPrecoTodasRegioes(List<PlanoRevisaoPrecoModelView> listPrecosPorRegiao)
    {
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

        var regioes = planoRevisaoPrecoModelView?.CdEmpresaRegiao?.Split(',').Select(int.Parse);

        foreach(var regiao in regioes)
        {
            var tipoPrecoRegiaoExiste = _planoRevisaoPrecoRepository.Get(x => x.ID_PLANO_REVISAO_TIPO == planoRevisaoPrecoModelView.IdPlanoRevisaoTipo && x.ID_EMPRESA_REGIAO == regiao);

            if (tipoPrecoRegiaoExiste == null)
                throw new Exception("Preço já cadastrado para essa região neste plano de revisao!");

            var planoRevisaoPreco = Map(planoRevisaoPrecoModelView, regiao);
            _planoRevisaoPrecoRepository.Create(planoRevisaoPreco);
        }

        return planoRevisaoPrecoModelView;
    }

    public List<PlanoRevisaoPrecoGetModelView> ListPrecosVigentes() 
    {
        var planosPrecosVigentes = _planoRevisaoPrecoRepository.ListPrecosVigentes();

        if (planosPrecosVigentes == null || !planosPrecosVigentes.Any())
        {
            throw new Exception("Nenhum preço vigente encontrado.");
        }

        return planosPrecosVigentes.Select(MapGet) // Remover os objetos "duplicados", de mesmo cdRegiao
                                        .GroupBy(p => new {
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

        ValidarAtualizacao(planoPreco);

        var regioes = _empresaRegiaoRepository.Get(x => x.CD_GRUPO_REGIAO == planoPreco.CdEmpresaRegiao).ToList();

        var valoresPorRevisaoPorMesmaRegiao = _planoRevisaoPrecoRepository.Get(x => x.ID_PLANO_REVISAO_TIPO == planoPreco.IdPlanoRevisaoTipo && regioes.Select(r => r.ID_EMPRESA_REGIAO).Contains(x.ID_EMPRESA_REGIAO));

        foreach(var item in valoresPorRevisaoPorMesmaRegiao)
        {
            var planoAtualizar = MapAtualizar(planoPreco, item.ID_EMPRESA_REGIAO, item.ID_PLANO_REVISAO_PRECO);
            _planoRevisaoPrecoRepository.Update(planoAtualizar);
        }

        return planoPreco;

    }

    private void ValidarAtualizacao(PlanoRevisaoPrecoModelView planoPreco) 
    {
        var planoRevisaoPreco = _planoRevisaoPrecoRepository.GetByRegiaoAndTipo(planoPreco.IdPlanoRevisaoTipo, planoPreco.CdEmpresaRegiao);

        if (planoRevisaoPreco is null)
            throw new Exception("Plano de revisão preço não encontrado.");

        var planoRevisaoAtual = _planoRevisaoTipoRepository.Get(x => x.ID_PLANO_REVISAO_TIPO == planoRevisaoPreco.ID_PLANO_REVISAO_TIPO).FirstOrDefault();
        var planoRevisaoAlterado = _planoRevisaoTipoRepository.Get(x => x.ID_PLANO_REVISAO_TIPO == planoPreco.IdPlanoRevisaoTipo).FirstOrDefault();

        if (planoRevisaoAtual?.ID_PLANO_REVISAO != planoRevisaoAlterado?.ID_PLANO_REVISAO)
            throw new Exception("Não é possível alterar o tipo de plano de revisão, pois ele pertence a outro plano de revisão.");

        if (planoRevisaoAlterado?.IN_REEMBOLSAR.ToUpper() != "S")
            throw new Exception("O tipo de plano de revisão selecionado não é reembolsável.");
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

    private PlanoRevisaoPrecoGetModelView MapGet(PlanoRevisaoPrecoEntity planoRevisaoPreco)
    {
        return new PlanoRevisaoPrecoGetModelView
        {
            IdPlanoRevisaoTipo = planoRevisaoPreco.IdPlanoRevisaoTipo,
            CdEmpresaRegiao = planoRevisaoPreco.CdEmpresaRegiao,
            NuValor = planoRevisaoPreco.NuValor,
            DtVigenciaInicial = planoRevisaoPreco.DtVigenciaInicial,
            DtVigenciaFinal = planoRevisaoPreco.DtVigenciaFinal
        };
    }
}
