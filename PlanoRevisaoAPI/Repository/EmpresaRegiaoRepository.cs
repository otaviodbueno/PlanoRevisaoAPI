using PlanoRevisaoAPI.Context;
using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Repository;

public class EmpresaRegiaoRepository : Repository<EmpresaRegiao>, IEmpresaRegiaoRepository
{
    public EmpresaRegiaoRepository(PlanoRevisaoApiContext context) : base(context)
    {
    }
}
