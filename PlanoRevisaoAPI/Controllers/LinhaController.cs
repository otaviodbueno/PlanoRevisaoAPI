using Microsoft.AspNetCore.Mvc;
using PlanoRevisaoAPI.Business;
using PlanoRevisaoAPI.Models;
using PlanoRevisaoAPI.ModelView;
using PlanoRevisaoAPI.Repository;


namespace PlanoRevisaoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LinhaController : ControllerBase
{
    private readonly ILinhaBusiness _linhaBusiness;

    public LinhaController(ILinhaBusiness linhBusiness)
    {
        _linhaBusiness = linhBusiness;
    }

    [HttpGet]
    public ActionResult<List<LinhaModelView>> Get()
    {
        var linhas = _linhaBusiness.GetLinhas();
        return Ok(linhas);
    }

    [HttpGet("GetPorNome")]
    public ActionResult<List<LinhaModelView>> Get(string nome)
    {
        var linhas = _linhaBusiness.Get(nome);
        return Ok(linhas);
    }

    [HttpPost]
    public ActionResult<Linha> Post(LinhaModelView linha)
    {
        _linhaBusiness.PostLinha(linha);
        return Created("", linha);
    }

    [HttpPut]
    public ActionResult<Linha> PutLinha(LinhaModelView linha)
    {
        var linhaAtualizada = _linhaBusiness.AtualizarLinha(linha);
        return Created("", linha);
    }

    [HttpDelete("{id}")]
    public ActionResult DeletarLinhaPorId(int id) //To Do LInha modelView
    {
       _linhaBusiness.DeleteLinhaPorId(id);
        return NoContent();
    }

}
