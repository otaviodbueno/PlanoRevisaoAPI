using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Business
{
    public interface IPlanoRevisaoBusiness
    {
        PlanoRevisao GetPlanoRevisaoPorId(int id);
        PlanoRevisao PostPlanoRevisao(PlanoRevisao planoRevisao);
    }
}
