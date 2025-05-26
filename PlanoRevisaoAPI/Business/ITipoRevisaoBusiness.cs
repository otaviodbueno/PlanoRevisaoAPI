using PlanoRevisaoAPI.ModelView;

namespace PlanoRevisaoAPI.Business;

public interface ITipoRevisaoBusiness
{
    List<TipoRevisaoModelView> GetTiposRevisaoAtivas();
}
