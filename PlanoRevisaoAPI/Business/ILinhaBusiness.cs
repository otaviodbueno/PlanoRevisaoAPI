using PlanoRevisaoAPI.Models;

namespace PlanoRevisaoAPI.Business;

public interface ILinhaBusiness
{
    Linha PostLinha(Linha linha);
    List<Linha> GetLinhas();
    List<Linha> Get(string nome);
}
