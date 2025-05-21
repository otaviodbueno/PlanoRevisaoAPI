using PlanoRevisaoAPI.Models;
using PlanoRevisaoAPI.Repository;

namespace PlanoRevisaoAPI.Business;

public class PlanoRevisaoBusiness : IPlanoRevisaoBusiness
{
    private readonly IPlanoRevisaoRepository _planoRevisaoRepository;
    private readonly ILinhaBusiness _linhaBusiness;
    private readonly IPoliticaVendaRepository _politicaVendaRepository;
    public PlanoRevisaoBusiness(IPlanoRevisaoRepository planoRevisaoRepository, ILinhaBusiness linhaBusinnes, IPoliticaVendaRepository politicaVendaRepository)
    {
        _planoRevisaoRepository = planoRevisaoRepository;
        _linhaBusiness = linhaBusinnes;
        _politicaVendaRepository = politicaVendaRepository;
    }

    public List<PlanoRevisao> GetPlanosRevisao()
    {
        return _planoRevisaoRepository.GetAll().ToList();
    }

    public PlanoRevisao GetPlanoRevisaoPorId(int id)
    {
        var planoRevisao = _planoRevisaoRepository.GetById(id);

        if (planoRevisao is null)
        {
            throw new Exception("Plano de Revisão não encontrado");
        }

        return planoRevisao;
    }

    public PlanoRevisao PostPlanoRevisao(PlanoRevisao planoRevisao)
    {
        try
        {
            ValidaPlanoRevisao(planoRevisao);
            return _planoRevisaoRepository.Create(planoRevisao);

        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao inserir o plano de revisão: " + ex.Message);
        }
    }

    private bool VerificaVigencias(DateTime? dtVigenciaInicial, DateTime? dtVigenciaFinal)
    {
        if (!dtVigenciaInicial.HasValue || !dtVigenciaFinal.HasValue)
            return false;

        if (dtVigenciaFinal < dtVigenciaInicial)
            return false;

        return true;
    }

    private void ValidaPlanoRevisao(PlanoRevisao planoRevisao)
    {
        var planoRevisaoExistente = _planoRevisaoRepository.Get(x => x.DS_PLANO_REVISAO == planoRevisao.DS_PLANO_REVISAO);

        if (planoRevisaoExistente.Any())
        {
            throw new Exception("Plano de Revisão já existe!");
        }

        var linha = _linhaBusiness.GetLinhaPorId(planoRevisao.ID_LINHA);

        if (linha is null)
        {
            throw new Exception("Linha vinculada ao plano não existe!");
        }

        var vigenciasValidas = VerificaVigencias(planoRevisao.DT_VIGENCIA_INICIAL, planoRevisao.DT_VIGENCIA_FINAL);

        if (!vigenciasValidas)
        {
            throw new Exception("Datas de vigências inválidas!");
        }

        var politicaVenda = _politicaVendaRepository.Get(x => x.ID_POLITICA_VENDA == planoRevisao.ID_POLITICA_VENDA);

        if(politicaVenda is null)
        {
            throw new Exception("Politica de venda não existe!");
        }
    }
}
