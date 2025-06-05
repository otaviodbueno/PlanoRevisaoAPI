using Microsoft.AspNetCore.Mvc;
using PlanoRevisaoAPI.Business;
using PlanoRevisaoAPI.ModelView;


namespace PlanoRevisaoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlanoRevisaoPrecoController : ControllerBase
{
    private readonly IPlanoRevisaoPrecoBusiness _planoRevisaoPrecoBusiness;

    public PlanoRevisaoPrecoController(IPlanoRevisaoPrecoBusiness planoRevisaoPrecoBusiness)
    {
        _planoRevisaoPrecoBusiness = planoRevisaoPrecoBusiness;
    }

    [HttpGet("ListPrecosVigentes")]
    public ActionResult<List<PlanoRevisaoPrecoModelView>> GetPrecosVigentes()
    {
        var precosVigentes = _planoRevisaoPrecoBusiness.ListPrecosVigentes();
        return Ok(precosVigentes);
    }

    [HttpPost]
    public ActionResult<List<PlanoRevisaoPrecoModelView>> PostValoresPorRegioes(List<PlanoRevisaoPrecoModelView> listPrecosRegioes)
    {
        var postPrecoRegioes = _planoRevisaoPrecoBusiness.PostPrecoRegioes(listPrecosRegioes);
        return Created("", postPrecoRegioes);
    }

    [HttpPut]
    public ActionResult<List<PlanoRevisaoPrecoModelView>> AtualizaValoresRegioes(List<PlanoRevisaoPrecoModelView> listPrecosRegioes)
    {
        var putPrecoRegioes = _planoRevisaoPrecoBusiness.AtualizaValoresList(listPrecosRegioes);
        return Created("", putPrecoRegioes);
    }

    [HttpDelete("{idPlanoRevisaoTipo}")]
    public ActionResult<PlanoRevisaoPrecoSemIdModelView> DeletaPrecoRevisao(int idPlanoRevisaoTipo)
    {
        _planoRevisaoPrecoBusiness.DeletaPrecoRevisaoPorTipo(idPlanoRevisaoTipo);
        return NoContent();
    }
}
