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

    public PlanoRevisaoPrecoModelView PostPrecoRevisoes(PlanoRevisaoPrecoModelView planoRevisaoPrecoModelView)
    {
        var planoRevisaoTipo = _planoRevisaoTipoRepository.Get(x => x.ID_PLANO_REVISAO_TIPO == planoRevisaoPrecoModelView.IdPlanoRevisaoTipo && x.IN_REEMBOLSAR == "S");

        if (planoRevisaoTipo is null)
        {
            throw new Exception("Tipo de plano de revisão não encontrado ou não é reembolsável.");
        }

        var regioes = planoRevisaoPrecoModelView.CdEmpresaRegiao.Split(',');

        foreach(var regiao in regioes)
        {
            var planoRevisaoPreco = Map(planoRevisaoPrecoModelView, regiao);
            _planoRevisaoPrecoRepository.Create(planoRevisaoPreco);
        }

        return planoRevisaoPrecoModelView;
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
}
