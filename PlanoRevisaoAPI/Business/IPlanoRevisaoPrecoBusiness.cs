using PlanoRevisaoAPI.ModelView;

namespace PlanoRevisaoAPI.Business;

public interface IPlanoRevisaoPrecoBusiness
{
    List<PlanoRevisaoPrecoSemIdModelView> ListPrecosVigentes();
    PlanoRevisaoPrecoModelView PostPrecoRevisoes(PlanoRevisaoPrecoModelView planoRevisaoPrecoModelView);
    List<PlanoRevisaoPrecoModelView> PostPrecoRegioes(List<PlanoRevisaoPrecoModelView> listPrecosPorRegiao);
    PlanoRevisaoPrecoModelView AtualizaValores(PlanoRevisaoPrecoModelView planoPreco);
    List<PlanoRevisaoPrecoModelView> AtualizaValoresList(List<PlanoRevisaoPrecoModelView> planoPreco);
    void DeletaPrecoRevisaoPorTipo(int planoRevisaoTipo);
}
