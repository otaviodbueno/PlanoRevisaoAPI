using PlanoRevisaoAPI.Models;
using PlanoRevisaoAPI.ModelView;
using PlanoRevisaoAPI.Repository;

namespace PlanoRevisaoAPI.Business;

public class PlanoRevisaoBusiness : IPlanoRevisaoBusiness
{
    private readonly IPlanoRevisaoRepository _planoRevisaoRepository;
    private readonly IPlanoRevisaoTipoRepository _planoRevisaoTipoRepository;
    private readonly ILinhaBusiness _linhaBusiness;
    private readonly IPoliticaVendaRepository _politicaVendaRepository;
    public PlanoRevisaoBusiness(IPlanoRevisaoRepository planoRevisaoRepository,
                                ILinhaBusiness linhaBusinnes,
                                IPoliticaVendaRepository politicaVendaRepository,
                                IPlanoRevisaoTipoRepository planoRevisaoTipoRepository)
    {
        _planoRevisaoRepository = planoRevisaoRepository;
        _linhaBusiness = linhaBusinnes;
        _politicaVendaRepository = politicaVendaRepository;
        _planoRevisaoTipoRepository = planoRevisaoTipoRepository;
    }

    public List<PlanoRevisaoModelView> GetPlanosRevisao()
    {
        var planos = _planoRevisaoRepository.GetAll().ToList();
        if (planos is null || planos.Count == 0)
        {
            throw new Exception("Nenhum plano de revisão encontrado");
        }

        return planos.Select(x => Map(x)).ToList();
    }

    public List<PlanoRevisaoModelView> ListPlanosVigentes()
    {
        var planos = GetPlanosRevisao();

        var planosVigentes = planos.Where(x => x.DtVigenciaInicial <= DateTime.Now && x.DtVigenciaFinal >= DateTime.Now).ToList();

        return planosVigentes;
    }

    public PlanoRevisaoModelView GetPlanoRevisaoPorId(int id)
    {
        var planoRevisao = _planoRevisaoRepository.GetById(id);

        if (planoRevisao is null)
        {
            throw new Exception("Plano de Revisão não encontrado");
        }

        return Map(planoRevisao);
    }

    public PlanoRevisao PostPlanoRevisao(PlanoRevisaoModelView planoRevisao)
    {
        try
        {
            ValidaPlanoRevisao(planoRevisao);
            var planoRevisaoInclusao = Map(planoRevisao);
            return _planoRevisaoRepository.Create(planoRevisaoInclusao);

        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao inserir o plano de revisão: " + ex.Message);
        }
    }

    public PlanoRevisaoModelView AtualizarPlanoRevisao(PlanoRevisaoModelView planoRevisao)
    {
        ValidaPlanoRevisao(planoRevisao, false);

        var planoAtualizacao = Map(planoRevisao);

        bool planoExiste = _planoRevisaoRepository.Get(x => x.ID_PLANO_REVISAO == planoRevisao.IdPlanoRevisao).Any();

        if (!planoExiste)
            throw new Exception("Plano de revisão não existe!");

        var planoAtualizado = _planoRevisaoRepository.Update(planoAtualizacao);

        return Map(planoAtualizado);
    }

    public PlanoRevisaoModelView DeletarPlanoRevisao(int id)
    {
        var planoDeletar = _planoRevisaoRepository.GetById(id);
        if (planoDeletar is null)
        {
            throw new Exception("Plano de Revisão não encontrado para deletar!");
        }

        var planoRevisaoTipo = _planoRevisaoTipoRepository.Get(x => x.ID_PLANO_REVISAO == id).ToList();

        if (planoRevisaoTipo != null && planoRevisaoTipo.Count > 0)
        {
            throw new Exception("Não é possível deletar o plano de revisão, existem tipos vinculados a ele!");
        }

        _planoRevisaoRepository.Delete(planoDeletar);
        return Map(planoDeletar);
    }

    private bool VerificaVigencias(DateTime? dtVigenciaInicial, DateTime? dtVigenciaFinal)
    {
        if (!dtVigenciaInicial.HasValue || !dtVigenciaFinal.HasValue)
            return false;

        if (dtVigenciaFinal < dtVigenciaInicial)
            return false;

        return true;
    }

    private void ValidaPlanoRevisao(PlanoRevisaoModelView planoRevisao, bool insert = true)
    {
        var planoRevisaoExistente = _planoRevisaoRepository.Get(x => x.DS_PLANO_REVISAO == planoRevisao.DsPlanoRevisao);

        if (planoRevisaoExistente.Count() > 0 && insert) // Não deve validar a existência quando for update, sempre irá existir
        {
            throw new Exception("Plano de Revisão já existe!");
        }

        var linha = _linhaBusiness.GetLinhaPorId(planoRevisao.IdLinha);

        if (linha is null)
        {
            throw new Exception("Linha vinculada ao plano não existe!");
        }

        var vigenciasValidas = VerificaVigencias(planoRevisao.DtVigenciaInicial, planoRevisao.DtVigenciaFinal);

        if (!vigenciasValidas)
        {
            throw new Exception("Datas de vigências inválidas!");
        }

        var politicaVenda = _politicaVendaRepository.Get(x => x.ID_POLITICA_VENDA == planoRevisao.IdPoliticaVenda);

        if (politicaVenda is null)
        {
            throw new Exception("Politica de venda não existe!");
        }

        if (planoRevisao.NuMesesGarantia <= 0)
            throw new Exception("Meses de garantia deve ser maior que zero!");
    }

    private PlanoRevisao Map(PlanoRevisaoModelView planorevisao)
    {
        return new PlanoRevisao
        {
            ID_PLANO_REVISAO = planorevisao.IdPlanoRevisao,
            DS_PLANO_REVISAO = planorevisao.DsPlanoRevisao,
            ID_LINHA = planorevisao.IdLinha,
            DT_INCLUSAO = DateTime.Now,
            DT_VIGENCIA_INICIAL = planorevisao.DtVigenciaInicial,
            DT_VIGENCIA_FINAL = planorevisao.DtVigenciaFinal,
            NU_MESES_GARANTIA = planorevisao.NuMesesGarantia,
            ID_POLITICA_VENDA = planorevisao.IdPoliticaVenda
        };
    }

    private PlanoRevisaoModelView Map(PlanoRevisao planorevisao)
    {
        return new PlanoRevisaoModelView
        {
            IdPlanoRevisao = planorevisao.ID_PLANO_REVISAO,
            DsPlanoRevisao = planorevisao.DS_PLANO_REVISAO,
            IdLinha = planorevisao.ID_LINHA,
            DtInclusao = planorevisao.DT_INCLUSAO,
            DtVigenciaInicial = planorevisao.DT_VIGENCIA_INICIAL,
            DtVigenciaFinal = planorevisao.DT_VIGENCIA_FINAL,
            NuMesesGarantia = planorevisao.NU_MESES_GARANTIA,
            IdPoliticaVenda = planorevisao.ID_POLITICA_VENDA
        };
    }
}
