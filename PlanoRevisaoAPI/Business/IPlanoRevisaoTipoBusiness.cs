using PlanoRevisaoAPI.ModelView;

namespace PlanoRevisaoAPI.Business;

public interface IPlanoRevisaoTipoBusiness
{
    PlanoRevisaoTipoModelView GetById(int idPlanoRevisao);
}
