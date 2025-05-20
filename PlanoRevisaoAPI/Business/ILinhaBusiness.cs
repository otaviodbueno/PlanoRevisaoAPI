using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Business;

public interface ILinhaBusiness
{
    Linha PostLinha(Linha linha);
    List<Linha> GetLinhas();
    List<Linha> Get(string nome);
    Linha GetLinhaPorId(int id);
    public Linha AtualizarLinha(int id);
    void DeleteLinhaPorId(int id);
}
