using PlanoRevisaoAPI.Models;
using PlanoRevisaoAPI.ModelView;
using PlanoRevisaoAPI.Repository;

namespace PlanoRevisaoAPI.Business;

public class LinhaBusiness : ILinhaBusiness
{
    private readonly ILinhaRepository _linhaRepository;
    private readonly IPlanoRevisaoRepository _planoRevisaoRepository;
    private readonly IPlanoRevisaoTipoRepository _planoRevisaoTipoRepository;
    public LinhaBusiness(ILinhaRepository linhaRepository, 
                        IPlanoRevisaoRepository planoRevisaoRepository,
                        IPlanoRevisaoTipoRepository planoRevisaoTipoRepository)
    {
        _linhaRepository = linhaRepository;
        _planoRevisaoRepository = planoRevisaoRepository;
        _planoRevisaoTipoRepository = planoRevisaoTipoRepository;
    }

    public List<LinhaModelView> GetLinhas()
    {
        return _linhaRepository.GetAll().Select(x => Map(x)).ToList();
    }

    public List<LinhaModelView> Get(string nome)
    {
        var linha = _linhaRepository.Get(x => x.NM_LINHA == nome).ToList();

        if (linha is null)
        {
            throw new Exception("Linha não encontrada");
        }
        
        return linha.Select(x => Map(x)).ToList();
    }

    public LinhaModelView GetLinhaPorId(int id)
    {
        var linha = _linhaRepository.GetById(id);
        if (linha is null)
        {
            throw new Exception("Linha não encontrada");
        }
        return Map(linha);
    }


    public List<LinhaModelView> ListLinhasAtivas()
    {
        var linhasAtivas = _linhaRepository.Get(x => x.IN_ATIVO == true).ToList();
        if (linhasAtivas is null || !linhasAtivas.Any())
        {
            throw new Exception("Nenhuma linha ativa encontrada");
        }
        return linhasAtivas.Select(Map).ToList();
    }

    public Linha PostLinha(LinhaModelView linha)
    {
        try
        {
            if (linha == null)
                return null;

            var linhaCriacao = Map(linha);

            return _linhaRepository.Create(linhaCriacao);
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

            var linhaPlanoRevisao = _planoRevisaoRepository.Get(x => x.ID_LINHA == id).ToList();

            if (linhaPlanoRevisao.Any())
            {
                throw new Exception("Não é possível excluir a linha, pois ela está associada a um ou mais planos de revisão.");
            }

            _linhaRepository.DeleteById(id);
        }
        catch(Exception ex)
        {
            throw;
        }
    }

    public LinhaModelView AtualizarLinha(LinhaModelView linha)
    {
        try
        {
            var linhaExiste = _linhaRepository.Get(x => x.ID_LINHA == linha.IdLinha).Any();

            if(!linhaExiste)
            {
                throw new Exception("Linha não encontrada");
            }

            var linhaAtualizacao = Map(linha);

            _linhaRepository.Update(linhaAtualizacao);

            return linha;
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao atualizar linha: " + ex.Message);
        }
    }

    public Linha Map(LinhaModelView linha)
    {
        return new Linha
        {
            ID_LINHA = linha.IdLinha,
            CD_LINHA = linha.CdLinha,
            NM_LINHA = linha.NmLinha,
            IN_ATIVO = linha.InAtivo == 1 ? true : false
        };
    }

    public LinhaModelView Map(Linha linha)
    {
        return new LinhaModelView
        {
            IdLinha = linha.ID_LINHA,
            CdLinha = linha.CD_LINHA,
            NmLinha = linha.NM_LINHA,
            InAtivo = linha.IN_ATIVO == true ? (short)1 : (short)0
        };
    }
}
