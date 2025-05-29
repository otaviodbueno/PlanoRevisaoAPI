using PlanoRevisaoAPI.ModelView;

namespace PlanoRevisaoAPI.Business;

public interface IPlanoRevisaoPrecoBusiness
{
    List<PlanoRevisaoPrecoModelView> ListPrecosVigentes();
    PlanoRevisaoPrecoModelView PostPrecoRevisoes(PlanoRevisaoPrecoModelView planoRevisaoPrecoModelView);
}
