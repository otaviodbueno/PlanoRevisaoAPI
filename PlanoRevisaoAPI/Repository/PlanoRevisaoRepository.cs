using PlanoRevisaoAPI.Context;
using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Repository;

public class PlanoRevisaoRepository : Repository<PlanoRevisao>, IPlanoRevisaoRepository
{
    public PlanoRevisaoRepository(PlanoRevisaoApiContext context) : base(context)
    {
    }
}