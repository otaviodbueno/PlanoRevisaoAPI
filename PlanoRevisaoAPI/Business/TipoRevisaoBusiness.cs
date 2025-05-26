using PlanoRevisaoAPI.Models;
using PlanoRevisaoAPI.ModelView;
using PlanoRevisaoAPI.Repository;

namespace PlanoRevisaoAPI.Business;

public class TipoRevisaoBusiness : ITipoRevisaoBusiness
{
    private readonly ITipoRevisaoRepository _tipoRevisaoRepository;

    public TipoRevisaoBusiness(ITipoRevisaoRepository tipoRevisaoRepository)
    {
        _tipoRevisaoRepository = tipoRevisaoRepository;
    }

    public List<TipoRevisaoModelView> GetTiposRevisaoAtivas()
    {
        var tiposRevisao = _tipoRevisaoRepository.GetAll().Where(t => t.IN_ATIVO == true).ToList();

        return Map(tiposRevisao);
    }

    private List<TipoRevisaoModelView> Map(List<TipoRevisao> tiposRevisao)
    {
        var lista = tiposRevisao.Select(x => new TipoRevisaoModelView
        {
            IdTipoRevisao = x.ID_TIPO_REVISAO,
            NmRevisao = x.NM_REVISAO,
            DtInclusao = x.DT_INCLUSAO,
            InAtivo = x.IN_ATIVO == true ? 1 : 0,
            NuRevisao = x.NU_REVISAO
        }).ToList();

        return lista;
    }
}
