using PlanoRevisaoAPI.Models;
using PlanoRevisaoAPI.Repository;

namespace PlanoRevisaoAPI.Business;

public class LinhaBusiness : ILinhaBusiness
{
    private readonly ILinhaRepository _linhaRepository;
    public LinhaBusiness(ILinhaRepository linhaRepository)
    {
        _linhaRepository = linhaRepository;
    }

    public List<Linha> GetLinhas()
    {
        return _linhaRepository.GetAll().ToList();
    }

    public Linha PostLinha(Linha linha)
    {
        try
        {
            if (linha == null)
                return null;

            return _linhaRepository.Create(linha);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao criar linha: " + ex.Message);
        }
    }
}
