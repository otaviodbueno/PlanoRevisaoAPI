using PlanoRevisaoAPI.Context;
using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Repository;

public class PlanoRevisaoTipoRepository : Repository<PlanoRevisaoTipo>, IPlanoRevisaoTipoRepository
{
    public PlanoRevisaoTipoRepository(PlanoRevisaoApiContext context) : base(context)
    {
    }
}
