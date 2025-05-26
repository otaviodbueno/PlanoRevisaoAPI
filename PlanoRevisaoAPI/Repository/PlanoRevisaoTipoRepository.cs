using PlanoRevisaoAPI.Context;
using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Repository;

public class PlanoRevisaoTipoRepository : Repository<PlanoRevisaoTipo>, IPlanoRevisaoTipoRepository
{
    public PlanoRevisaoTipoRepository(PlanoRevisaoApiContext context) : base(context)
    {
    }

    public List<PlanoRevisaoTipo> GetByIdPlanoRevisao(int idPlanoRevisao)
    {
        return _context.PlanoRevisaoTipo
            .Where(x => x.ID_PLANO_REVISAO == idPlanoRevisao)
            .ToList();
    }

    public void PostListTipoPlanoRevisao(List<PlanoRevisaoTipo> listaAdd)
    {
        foreach(var item in listaAdd)
        {
            _context.PlanoRevisaoTipo.Add(item);
        }

        _context.SaveChanges();
    }
}
