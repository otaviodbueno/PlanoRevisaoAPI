using PlanoRevisaoAPI.Entity;
using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Repository;

public interface IPlanoRevisaoPrecoRepository : IRepository<PlanoRevisaoPreco>
{
    List<PlanoRevisaoPrecoEntity> ListPrecosVigentes();
    PlanoRevisaoPreco GetByRegiaoAndTipo(int idPlanoRevisaoTipo, string cdEmpresaRegiao);
}
