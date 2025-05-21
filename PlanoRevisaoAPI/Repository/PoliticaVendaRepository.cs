using PlanoRevisaoAPI.Context;
using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Repository;

public class PoliticaVendaRepository : Repository<PoliticaVenda>, IPoliticaVendaRepository
{
    public PoliticaVendaRepository(PlanoRevisaoApiContext context) : base(context)
    {
    }
}
