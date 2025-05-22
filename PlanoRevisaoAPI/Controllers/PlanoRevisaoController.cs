using Microsoft.AspNetCore.Mvc;
using PlanoRevisaoAPI.Business;
using PlanoRevisaoAPI.Models;
using PlanoRevisaoAPI.ModelView;


namespace PlanoRevisaoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlanoRevisaoController : ControllerBase
{
    private readonly IPlanoRevisaoBusiness _planoRevisaoBusiness;

    public PlanoRevisaoController(IPlanoRevisaoBusiness planoRevisaoBusiness)
    {
        _planoRevisaoBusiness = planoRevisaoBusiness;
    }

    [HttpGet]
    public ActionResult<PlanoRevisaoModelView> GetPlanos()
    {
        var planosRevisao = _planoRevisaoBusiness.GetPlanosRevisao();
        return Ok(planosRevisao);
    }

    [HttpGet("{id}")]
    public ActionResult<PlanoRevisaoModelView> GetPlanoRevisaoPorId(int id)
    {
        var planoRevisao = _planoRevisaoBusiness.GetPlanoRevisaoPorId(id);
        return Ok(planoRevisao);
    }

    [HttpPost]
    public ActionResult Post(PlanoRevisaoModelView planoRevisao)
    {
        _planoRevisaoBusiness.PostPlanoRevisao(planoRevisao);
        return Created("", planoRevisao);
    }

    [HttpPut]
    public ActionResult<PlanoRevisaoModelView> UpdatePlanoRevisao(PlanoRevisaoModelView planoRevisao)
    {
        var planoAtualizado = _planoRevisaoBusiness.AtualizarPlanoRevisao(planoRevisao);
        return Created("", planoAtualizado);
    }

    [HttpDelete("{id}")]
    public ActionResult<PlanoRevisaoModelView> Delete(int id)
    {
        var planoDeletar = _planoRevisaoBusiness.DeletarPlanoRevisao(id);
        return NoContent();
    }
}
