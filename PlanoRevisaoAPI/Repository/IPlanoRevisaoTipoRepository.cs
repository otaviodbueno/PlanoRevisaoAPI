using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Repository;

public interface IPlanoRevisaoTipoRepository :IRepository<PlanoRevisaoTipo>
{
    List<PlanoRevisaoTipo> GetByIdPlanoRevisao(int idPlanoRevisao);
    void PostListTipoPlanoRevisao(List<PlanoRevisaoTipo> listaAdd);
}
