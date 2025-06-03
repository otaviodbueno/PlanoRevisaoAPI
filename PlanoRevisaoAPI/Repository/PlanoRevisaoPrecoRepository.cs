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
                                     IdPlanoRevisaoTipo = preco.ID_PLANO_REVISAO_TIPO,
                                     CdEmpresaRegiao = regiao.CD_GRUPO_REGIAO,
                                     NuValor = preco.NU_VALOR,
                                     DtVigenciaInicial = preco.DT_VIGENCIA_INICIAL,
                                     DtVigenciaFinal = preco.DT_VIGENCIA_FINAL
                                 }).ToList();

        return listPrecosVigentes;
    }

    public List<PlanoRevisaoPreco> GetByRegiaoAndTipo(int idPlanoRevisaoTipo, string cdEmpresaRegiao)
    {
        var planoRevisaoPreco = (from preco in _context.Set<PlanoRevisaoPreco>()
                                 join regiao in _context.Set<EmpresaRegiao>() on preco.ID_EMPRESA_REGIAO equals regiao.ID_EMPRESA_REGIAO
                                 where preco.ID_PLANO_REVISAO_TIPO == idPlanoRevisaoTipo && regiao.CD_GRUPO_REGIAO == cdEmpresaRegiao
                                 select new PlanoRevisaoPreco
                                 {
                                     ID_PLANO_REVISAO_PRECO = preco.ID_PLANO_REVISAO_PRECO,
                                     ID_PLANO_REVISAO_TIPO = preco.ID_PLANO_REVISAO_TIPO,
                                     ID_EMPRESA_REGIAO = preco.ID_EMPRESA_REGIAO,
                                     NU_VALOR = preco.NU_VALOR,
                                     DT_VIGENCIA_INICIAL = preco.DT_VIGENCIA_INICIAL,
                                     DT_VIGENCIA_FINAL = preco.DT_VIGENCIA_FINAL
                                 }).ToList(); 
        return planoRevisaoPreco;
    }
}
