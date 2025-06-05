using PlanoRevisaoAPI.Models;
using PlanoRevisaoAPI.ModelView;

namespace PlanoRevisaoAPI.Business
{
    public interface IPlanoRevisaoBusiness
    {
        List<PlanoRevisaoModelView> GetPlanosRevisao();
        PlanoRevisaoModelView GetPlanoRevisaoPorId(int id);
        List<PlanoRevisaoModelView> ListPlanosVigentes();
        PlanoRevisao PostPlanoRevisao(PlanoRevisaoModelView planoRevisao);
        PlanoRevisaoModelView AtualizarPlanoRevisao(PlanoRevisaoModelView planoRevisao);
        PlanoRevisaoModelView DeletarPlanoRevisao(int id);
    }
}
