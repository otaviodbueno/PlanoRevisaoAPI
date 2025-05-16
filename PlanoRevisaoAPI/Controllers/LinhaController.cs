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

    //// GET api/<ValuesController>/5
    //[HttpGet("{id}")]
    //public string Get(int id)
    //{
    //    return "value";
    //}

    [HttpPost]
    public ActionResult<Linha> Post(Linha linha)
    {
        _linhaBusiness.PostLinha(linha);
        return Ok(linha);
    }

    //// PUT api/<ValuesController>/5
    //[HttpPut("{id}")]
    //public void Put(int id, [FromBody] string value)
    //{
    //}

    //// DELETE api/<ValuesController>/5
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}
}
