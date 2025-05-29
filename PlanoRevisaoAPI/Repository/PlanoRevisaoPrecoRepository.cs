using PlanoRevisaoAPI.Context;
using PlanoRevisaoAPI.Entity;
using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Repository;

public class PlanoRevisaoPrecoRepository : Repository<PlanoRevisaoPreco>, IPlanoRevisaoPrecoRepository
{
    public PlanoRevisaoPrecoRepository(PlanoRevisaoApiContext context) : base(context)
    {
    }

    public List<PlanoRevisaoPrecoEntity> ListPrecosVigentes()
    {
        var listPrecosVigentes = (from preco in _context.Set<PlanoRevisaoPreco>()
                                 join regiao in _context.Set<EmpresaRegiao>() on preco.ID_EMPRESA_REGIAO equals regiao.ID_EMPRESA_REGIAO
                                 where preco.DT_VIGENCIA_INICIAL <= DateTime.Now && preco.DT_VIGENCIA_FINAL >= DateTime.Now
                                 select new PlanoRevisaoPrecoEntity
                                 {
                                     IdPlanoRevisaoPreco = preco.ID_PLANO_REVISAO_PRECO,
                                     IdPlanoRevisaoTipo = preco.ID_PLANO_REVISAO_TIPO,
                                     CdEmpresaRegiao = regiao.CD_GRUPO_REGIAO,
                                     NuValor = preco.NU_VALOR,
                                     DtVigenciaInicial = preco.DT_VIGENCIA_INICIAL,
                                     DtVigenciaFinal = preco.DT_VIGENCIA_FINAL
                                 }).ToList();

        return listPrecosVigentes;
    }
}
