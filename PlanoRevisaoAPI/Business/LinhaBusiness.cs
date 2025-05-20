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

    public List<Linha> Get(string nome)
    {
        var linha = _linhaRepository.Get(x => x.NM_LINHA == nome).ToList();

        if (linha is null)
        {
            throw new Exception("Linha não encontrada");
        }
        
        return linha;
    }

    public Linha GetLinhaPorId(int id)
    {
        var linha = _linhaRepository.GetById(id);
        if (linha is null)
        {
            throw new Exception("Linha não encontrada");
        }
        return linha;
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

    public void DeleteLinhaPorId(int id)
    {
        try
        {
            var linha = _linhaRepository.GetById(id);

            if(linha is null)
            {
                throw new Exception("Linha não encontrada");
            }

            _linhaRepository.DeleteById(id);
        }
        catch(Exception ex)
        {
            throw;
        }
    }

    public Linha AtualizarLinha(int id)
    {
        try
        {
            var linhaAtualizacao = _linhaRepository.GetById(id);

            if(linhaAtualizacao is null)
            {
                throw new Exception("Linha não encontrada");
            }

            _linhaRepository.Update(linhaAtualizacao);

            return linhaAtualizacao;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao atualizar linha: " + ex.Message);
        }
    }
}
