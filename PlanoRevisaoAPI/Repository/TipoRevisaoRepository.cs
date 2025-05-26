using PlanoRevisaoAPI.Context;
using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Repository;

public class TipoRevisaoRepository : Repository<TipoRevisao>, ITipoRevisaoRepository
{
    public TipoRevisaoRepository(PlanoRevisaoApiContext context) : base(context)
    {
    }
}
