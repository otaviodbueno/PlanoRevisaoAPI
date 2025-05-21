using PlanoRevisaoAPI.Models;
using PlanoRevisaoAPI.ModelView;

namespace PlanoRevisaoAPI.Business;

public interface ILinhaBusiness
{
    Linha AtualizarLinha(LinhaModelView linha);
    List<Linha> GetLinhas();
    List<LinhaModelView> Get(string nome);
    Linha GetLinhaPorId(int id);
    Linha PostLinha(LinhaModelView linha);
    void DeleteLinhaPorId(int id);
}
