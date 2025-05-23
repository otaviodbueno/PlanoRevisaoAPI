using PlanoRevisaoAPI.Models;
using PlanoRevisaoAPI.ModelView;
using PlanoRevisaoAPI.Repository;

namespace PlanoRevisaoAPI.Business;

public class PlanoRevisaoTipoBusiness : IPlanoRevisaoTipoBusiness
{
    private readonly IPlanoRevisaoTipoRepository _planoRevisaoTipoRepository;

    public PlanoRevisaoTipoBusiness(IPlanoRevisaoTipoRepository planoRevisaoTipoRepository)
    {
        _planoRevisaoTipoRepository = planoRevisaoTipoRepository;
    }


    public PlanoRevisaoTipoModelView GetAll()
    {
        var planoRevisaoTipo = _planoRevisaoTipoRepository.GetAll().OrderBy(x => x.ID_PLANO_REVISAO).ToList();
        return Map(planoRevisaoTipo);
    }


    private PlanoRevisaoTipoModelView Map(List<PlanoRevisaoTipo> planoRevisaoTipo)
    {
        return new PlanoRevisaoTipoModelView
        {
            IdPlanoRevisao = planoRevisaoTipo[0].ID_PLANO_REVISAO,
            TiposRevisao = planoRevisaoTipo.Select(x => new TipoRevisaoRequestModelView
            {
                IdTipoRevisao = x.ID_TIPO_REVISAO,
                UnidadeMaoDeObra = x.UNIDADE_MAO_DE_OBRA,
                InReembolsar = x.IN_REEMBOLSAR
            }).ToList()
        };
    }
}
