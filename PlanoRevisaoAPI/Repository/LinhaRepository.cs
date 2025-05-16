using PlanoRevisaoAPI.Context;
using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Repository;

public class LinhaRepository : Repository<Linha>, ILinhaRepository
{
    public LinhaRepository(PlanoRevisaoApiContext context) : base(context)
    {
    }
}
