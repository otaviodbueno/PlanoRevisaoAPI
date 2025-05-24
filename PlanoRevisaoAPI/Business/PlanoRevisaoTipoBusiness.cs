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


    public PlanoRevisaoTipoModelView GetById(int idPlanoRevisao)
    {
        var planoRevisaoTipo = _planoRevisaoTipoRepository.GetByIdPlanoRevisao(idPlanoRevisao);
        return MapPorPlanoUnico(planoRevisaoTipo);
    }


    private PlanoRevisaoTipoModelView MapPorPlanoUnico(List<PlanoRevisaoTipo> planoRevisaoTipo)
    {
        if(planoRevisaoTipo == null || planoRevisaoTipo.Count == 0)
           throw new ArgumentException("A lista de tipos de revisão está vazia.");

        var idPlanoRevisao = planoRevisaoTipo[0].ID_PLANO_REVISAO;

        if (planoRevisaoTipo.Any(x => x.ID_PLANO_REVISAO != idPlanoRevisao))
            throw new InvalidOperationException("Todos os tipos de revisão devem pertencer ao mesmo plano.");

        return new PlanoRevisaoTipoModelView
        {
            IdPlanoRevisao = idPlanoRevisao,
            TiposRevisao = planoRevisaoTipo.Select(x => new TipoRevisaoRequestModelView
            {
                IdTipoRevisao = x.ID_TIPO_REVISAO,
                UnidadeMaoDeObra = x.UNIDADE_MAO_DE_OBRA,
                InReembolsar = x.IN_REEMBOLSAR
            }).ToList()
        };
    }
}
