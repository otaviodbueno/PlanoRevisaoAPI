using Microsoft.AspNetCore.Mvc;
using PlanoRevisaoAPI.Business;
using PlanoRevisaoAPI.Models;
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
    public ActionResult<List<Linha>> Get()
    {
        var linhas = _linhaBusiness.GetLinhas();
        return Ok(linhas);
    }

    [HttpGet("GetPorNome")]
    public ActionResult<List<Linha>> Get(string nome)
    {
        var linhas = _linhaBusiness.Get(nome);
        return Ok(linhas);
    }

    [HttpPost]
    public ActionResult<Linha> Post(Linha linha)
    {
        _linhaBusiness.PostLinha(linha);
        return Ok(linha);
    }

    [HttpPut("id")]
    public ActionResult<Linha> PutLinha(int id)
    {
        var linha = _linhaBusiness.AtualizarLinha(id);
        return Created("", linha);
    }

    [HttpDelete("{id}")]
    public ActionResult DeletarLinhaPorId(int id)
    {
       _linhaBusiness.DeleteLinhaPorId(id);
        return NoContent();
    }

}
