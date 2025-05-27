using PlanoRevisaoAPI.Context;
using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Repository;

public class PlanoRevisaoPrecoRepository : Repository<PlanoRevisaoPreco>, IPlanoRevisaoPrecoRepository
{
    public PlanoRevisaoPrecoRepository(PlanoRevisaoApiContext context) : base(context)
    {
    }
}
