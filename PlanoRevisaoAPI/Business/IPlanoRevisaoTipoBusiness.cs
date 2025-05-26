using PlanoRevisaoAPI.ModelView;

namespace PlanoRevisaoAPI.Business;

public interface IPlanoRevisaoTipoBusiness
{
    PlanoRevisaoTipoModelView GetById(int idPlanoRevisao);
    List<PlanoRevisaoTipoModelView> GetAllPlanosRevisaoTipo();
    PlanoRevisaoTipoModelView AtualizarPlanoRevisaoTipo(PlanoRevisaoTipoModelView planoRevisaoTipoModelView);
    PlanoRevisaoTipoModelView PostPlano(PlanoRevisaoTipoModelView tipoPlano);
    PlanoRevisaoTipoModelView PostComPlanoBase(int idPlanoNovo, int idPlanoBase);
    PlanoRevisaoTipoModelView DeleteTipoPlanoRevisao(int idPlanoRevisao);
}
