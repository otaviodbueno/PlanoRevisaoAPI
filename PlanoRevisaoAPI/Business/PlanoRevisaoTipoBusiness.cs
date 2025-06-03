using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Identity.Client;
using PlanoRevisaoAPI.Models;
using PlanoRevisaoAPI.ModelView;
using PlanoRevisaoAPI.Repository;

namespace PlanoRevisaoAPI.Business;

public class PlanoRevisaoTipoBusiness : IPlanoRevisaoTipoBusiness
{
    private readonly IPlanoRevisaoTipoRepository _planoRevisaoTipoRepository;
    private readonly IPlanoRevisaoRepository _planoRevisaoRepository;
    private readonly ITipoRevisaoRepository _tipoRevisaoRepository;

    public PlanoRevisaoTipoBusiness(IPlanoRevisaoTipoRepository planoRevisaoTipoRepository,
                                    IPlanoRevisaoRepository planoRevisaoRepository,
                                    ITipoRevisaoRepository tipoRevisaoRepository)
    {
        _planoRevisaoTipoRepository = planoRevisaoTipoRepository;
        _planoRevisaoRepository = planoRevisaoRepository;
        _tipoRevisaoRepository = tipoRevisaoRepository;
    }


    public PlanoRevisaoTipoModelView GetById(int idPlanoRevisao)
    {
        var planoRevisao = _planoRevisaoRepository.GetById(idPlanoRevisao);
        var planoRevisaoTipo = _planoRevisaoTipoRepository.GetByIdPlanoRevisao(idPlanoRevisao);

        if (planoRevisaoTipo is null || planoRevisao is null)
            throw new Exception("Plano de revisão inválido ou sem tipos!");

        return MapPorPlanoUnico(planoRevisaoTipo);
    }

    public List<PlanoRevisaoTipoModelView> GetAllPlanosRevisaoTipo()
    {
        var tipos = _planoRevisaoTipoRepository.GetAll();

        if (tipos == null || !tipos.Any())
            throw new Exception("Nenhum plano de revisão encontrado");

        var planosAgrupados = tipos
            .GroupBy(p => p.ID_PLANO_REVISAO)
            .Select(grupo => new PlanoRevisaoTipoModelView
            {
                IdPlanoRevisao = grupo.Key,
                TiposRevisao = grupo.Select(x => new TipoRevisaoRequestModelView
                {
                    IdPlanoRevisaoTipo = x.ID_PLANO_REVISAO_TIPO,
                    IdTipoRevisao = x.ID_TIPO_REVISAO,
                    UnidadeMaoDeObra = x.UNIDADE_MAO_DE_OBRA,
                    InReembolsar = x.IN_REEMBOLSAR
                }).ToList()
            })
            .ToList();

        return planosAgrupados;
    }

    public List<PlanoRevisaoTipoModelView> ListPlanoRevisaoTipoReembolsaveis()
    {
        var todosTipos = GetAllPlanosRevisaoTipo();
        
        foreach(var tipo in todosTipos)
        {
            tipo.TiposRevisao = tipo.TiposRevisao.Where(t => t.InReembolsar == "S").ToList();
        }

        return todosTipos;
    }


    public PlanoRevisaoTipoModelView PostPlano(PlanoRevisaoTipoModelView tipoPlano)
    {
        var planoRevisao = _planoRevisaoRepository.GetById(tipoPlano.IdPlanoRevisao);

        if (planoRevisao is null)
            throw new Exception("Nenhum plano de revisão encontrado para o ID informado");

        List<PlanoRevisaoTipo> listaAdicionar = new List<PlanoRevisaoTipo>();
        foreach (var tipo in tipoPlano?.TiposRevisao)
        {
            ValidateTipoRevisaoByPlano(tipo, tipoPlano, planoRevisao);

            listaAdicionar.Add(new PlanoRevisaoTipo
            {
                ID_PLANO_REVISAO = tipoPlano.IdPlanoRevisao,
                ID_TIPO_REVISAO = tipo.IdTipoRevisao,
                UNIDADE_MAO_DE_OBRA = tipo.UnidadeMaoDeObra,
                IN_REEMBOLSAR = tipo.InReembolsar
            });
        }

        _planoRevisaoTipoRepository.PostListTipoPlanoRevisao(listaAdicionar);

        return MapPorPlanoUnico(listaAdicionar);
    }

    public PlanoRevisaoTipoModelView PostComPlanoBase(int idPlanoNovo, int idPlanoBase)
    {
        VerificaPostByPlanoBase(idPlanoNovo, idPlanoBase);

        var tiposPlanoBase = _planoRevisaoTipoRepository.GetByIdPlanoRevisao(idPlanoBase);

        var tiposModelView = MapPorPlanoUnico(tiposPlanoBase);

        List<PlanoRevisaoTipo> listaAdicionar = new List<PlanoRevisaoTipo>();
        foreach (var tipo in tiposModelView.TiposRevisao)
        {
            listaAdicionar.Add(new PlanoRevisaoTipo
            {
                ID_PLANO_REVISAO = idPlanoNovo,
                ID_TIPO_REVISAO = tipo.IdTipoRevisao,
                UNIDADE_MAO_DE_OBRA = tipo.UnidadeMaoDeObra,
                IN_REEMBOLSAR = tipo.InReembolsar
            });
        }

        _planoRevisaoTipoRepository.PostListTipoPlanoRevisao(listaAdicionar);

        return MapPorPlanoUnico(listaAdicionar);
    }

    public PlanoRevisaoTipoModelView AtualizarPlanoRevisaoTipo(PlanoRevisaoTipoModelView planoRevisaoTipoModelView)
    {
        ValidatePlanoAtualizacao(planoRevisaoTipoModelView);

        var listUpdate = MapPlanos(planoRevisaoTipoModelView);

        foreach (var item in listUpdate)
        {
            _planoRevisaoTipoRepository.Update(item);
        }

        return planoRevisaoTipoModelView;
    }

    public PlanoRevisaoTipoModelView DeleteTipoPlanoRevisao(int idPlanoRevisao)
    {
        var planoRevisaoTipo = _planoRevisaoTipoRepository.GetByIdPlanoRevisao(idPlanoRevisao);

        if (planoRevisaoTipo is null)
            throw new Exception("Nenhum plano de revisão encontrado para deletar");

        foreach (var tipo in planoRevisaoTipo)
        {
            _planoRevisaoTipoRepository.Delete(tipo);
        }

        return MapPorPlanoUnico(planoRevisaoTipo);
    }

    private PlanoRevisaoTipoModelView MapPorPlanoUnico(List<PlanoRevisaoTipo> planoRevisaoTipo)
    {
        if (planoRevisaoTipo == null || planoRevisaoTipo.Count == 0)
            throw new ArgumentException("A lista de tipos de revisão está vazia.");

        var idPlanoRevisao = planoRevisaoTipo[0].ID_PLANO_REVISAO; // qualquer posição da lista o ID_PLANO_REVISAO é o mesmo

        if (planoRevisaoTipo.Any(x => x.ID_PLANO_REVISAO != idPlanoRevisao))
            throw new InvalidOperationException("Todos os tipos de revisão devem pertencer ao mesmo plano.");

        return new PlanoRevisaoTipoModelView
        {
            IdPlanoRevisao = idPlanoRevisao,
            TiposRevisao = planoRevisaoTipo.Select(x => new TipoRevisaoRequestModelView
            {
                IdPlanoRevisaoTipo = x.ID_PLANO_REVISAO_TIPO,
                IdTipoRevisao = x.ID_TIPO_REVISAO,
                UnidadeMaoDeObra = x.UNIDADE_MAO_DE_OBRA,
                InReembolsar = x.IN_REEMBOLSAR
            }).ToList()
        };
    }

    private List<PlanoRevisaoTipo> MapPlanos(PlanoRevisaoTipoModelView planoRevisaoTipoModelView)
    {
        var listTipos = new List<PlanoRevisaoTipo>();

        foreach (var item in planoRevisaoTipoModelView.TiposRevisao)
        {
            listTipos.Add(new PlanoRevisaoTipo
            {
                ID_PLANO_REVISAO_TIPO = item.IdPlanoRevisaoTipo,
                ID_PLANO_REVISAO = planoRevisaoTipoModelView.IdPlanoRevisao,
                ID_TIPO_REVISAO = item.IdTipoRevisao,
                UNIDADE_MAO_DE_OBRA = item.UnidadeMaoDeObra,
                IN_REEMBOLSAR = item.InReembolsar
            });
        }

        return listTipos;
    }

    private void ValidateTipoRevisaoByPlano(TipoRevisaoRequestModelView tipo, PlanoRevisaoTipoModelView tipoPlano, PlanoRevisao planoRevisao)
    {
        var tipoRevisaoExiste = _tipoRevisaoRepository.GetById(tipo.IdTipoRevisao) != null;

        if (!tipoRevisaoExiste)
            throw new Exception($"Nenhum tipo de revisão encontrado para o ID informado: {tipo.IdTipoRevisao}");

        var idsTipoRevisao = tipoPlano.TiposRevisao.Select(t => t.IdTipoRevisao).ToList();

        var tipoRevisaoNaoExiste = _tipoRevisaoRepository.Get(x => !idsTipoRevisao.Contains(x.ID_TIPO_REVISAO)).Any();

        if (tipoRevisaoNaoExiste)
            throw new Exception("Um ou mais tipos de revisão não existem.");

        var tipoRevisaoNoPlano = _planoRevisaoTipoRepository.Get(x => x.ID_TIPO_REVISAO == tipo.IdTipoRevisao && x.ID_PLANO_REVISAO == tipoPlano.IdPlanoRevisao);

        if (tipoRevisaoNoPlano != null && tipoRevisaoNoPlano.Count() != 0)
            throw new Exception($"Tipo de revisão {tipo.IdTipoRevisao} já se encontra no plano {planoRevisao.DS_PLANO_REVISAO}");

    }

    private void ValidatePlanoAtualizacao(PlanoRevisaoTipoModelView planoRevisaoTipoModelView)
    {
        var planoRevisao = _planoRevisaoRepository.GetById(planoRevisaoTipoModelView.IdPlanoRevisao);

        if (planoRevisao is null)
            throw new Exception("Nenhum plano de revisão encontrado para o ID informado");

        var idsTipoRevisao = planoRevisaoTipoModelView.TiposRevisao.Select(t => t.IdTipoRevisao).ToList();

        var tipoRevisaoNaoExiste = _tipoRevisaoRepository.Get(x => !idsTipoRevisao.Contains(x.ID_TIPO_REVISAO)).Any();

        if(tipoRevisaoNaoExiste)
            throw new Exception("Um ou mais tipos de revisão não existem.");
    }


    private void VerificaPostByPlanoBase(int idPlanoNovo, int idPlanoBase)
    {
        var planoNovo = _planoRevisaoRepository.GetById(idPlanoNovo);
        var planoBase = _planoRevisaoRepository.GetById(idPlanoBase);

        if(planoNovo is null || planoBase is null)
            throw new Exception("Um ou ambos os planos de revisão não existem.");

        var tiposPlanoBase = _planoRevisaoTipoRepository.GetByIdPlanoRevisao(idPlanoBase);

        if (tiposPlanoBase is null || !tiposPlanoBase.Any())
            throw new Exception("Os tipos do plano base devem ser cadastrados antes!");
    }
}