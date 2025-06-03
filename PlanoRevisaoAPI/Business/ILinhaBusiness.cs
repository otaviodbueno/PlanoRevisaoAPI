using PlanoRevisaoAPI.Models;
using PlanoRevisaoAPI.ModelView;

namespace PlanoRevisaoAPI.Business;

public interface ILinhaBusiness
{
    LinhaModelView AtualizarLinha(LinhaModelView linha);
    List<LinhaModelView> GetLinhas();
    List<LinhaModelView> ListLinhasAtivas();
    List<LinhaModelView> Get(string nome);
    LinhaModelView GetLinhaPorId(int id);
    Linha PostLinha(LinhaModelView linha);
    void DeleteLinhaPorId(int id);
}
