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
    public PlanoRevisaoPrecoBusiness(IPlanoRevisaoTipoRepository planoRevisaoTipoRepository, 
                                    IPlanoRevisaoPrecoRepository planoRevisaoPrecoRepository, 
                                    IPlanoRevisaoRepository planoRevisaoRepository)
    {
        _planoRevisaoTipoRepository = planoRevisaoTipoRepository;
        _planoRevisaoPrecoRepository = planoRevisaoPrecoRepository;
        _planoRevisaoRepository = planoRevisaoRepository;
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

        var regioes = planoRevisaoPrecoModelView?.CdEmpresaRegiao?.Split(',');

        foreach(var regiao in regioes)
        {
            var tipoPrecoRegiaoExiste = _planoRevisaoPrecoRepository.Get(x => x.ID_PLANO_REVISAO_TIPO == planoRevisaoPrecoModelView.IdPlanoRevisaoTipo && x.ID_EMPRESA_REGIAO == int.Parse(regiao)) != null;

            if (tipoPrecoRegiaoExiste)
                throw new Exception("Preço já cadastrado para essa região neste plano de revisao!");

            var planoRevisaoPreco = Map(planoRevisaoPrecoModelView, regiao);
            _planoRevisaoPrecoRepository.Create(planoRevisaoPreco);
        }

        return planoRevisaoPrecoModelView;
    }

    public List<PlanoRevisaoPrecoModelView> ListPrecosVigentes()
    {
        var planosPrecosVigentes = _planoRevisaoPrecoRepository.ListPrecosVigentes();

        // adicionar validações

        if (planosPrecosVigentes == null || !planosPrecosVigentes.Any())
        {
            throw new Exception("Nenhum preço vigente encontrado.");
        }

        return planosPrecosVigentes.Select(Map).ToList();
    }

    private PlanoRevisaoPreco Map(PlanoRevisaoPrecoModelView plano, string regiao)
    {
        return new PlanoRevisaoPreco
        {
            ID_PLANO_REVISAO_TIPO = plano.IdPlanoRevisaoTipo,
            ID_EMPRESA_REGIAO = int.Parse(regiao),
            NU_VALOR = plano.NuValor,
            DT_VIGENCIA_INICIAL = plano.DtVigenciaInicial,
            DT_VIGENCIA_FINAL = plano.DtVigenciaFinal
        };
    }

    private PlanoRevisaoPrecoModelView Map(PlanoRevisaoPrecoEntity planoRevisaoPreco)
    {
        return new PlanoRevisaoPrecoModelView
        {
            IdPlanoRevisaoPreco = planoRevisaoPreco.IdPlanoRevisaoPreco,
            IdPlanoRevisaoTipo = planoRevisaoPreco.IdPlanoRevisaoTipo,
            CdEmpresaRegiao = planoRevisaoPreco.CdEmpresaRegiao,
            NuValor = planoRevisaoPreco.NuValor,
            DtVigenciaInicial = planoRevisaoPreco.DtVigenciaInicial,
            DtVigenciaFinal = planoRevisaoPreco.DtVigenciaFinal
        };
    }
}
