using PlanoRevisaoAPI.ModelView;

namespace PlanoRevisaoAPI.Business;

public interface IPlanoRevisaoPrecoBusiness
{
    List<PlanoRevisaoPrecoGetModelView> ListPrecosVigentes();
    PlanoRevisaoPrecoModelView PostPrecoRevisoes(PlanoRevisaoPrecoModelView planoRevisaoPrecoModelView);
    PlanoRevisaoPrecoModelView AtualizaValores(PlanoRevisaoPrecoModelView planoPreco);
}
