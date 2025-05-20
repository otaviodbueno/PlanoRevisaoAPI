using Microsoft.AspNetCore.Mvc;
using PlanoRevisaoAPI.Business;
using PlanoRevisaoAPI.Models;


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

    [HttpPost]
    public ActionResult Post(PlanoRevisao planoRevisao) // TO DO - Criar PlanoRevisaoModelView
    {
        _planoRevisaoBusiness.PostPlanoRevisao(planoRevisao);
        return Created("", planoRevisao);
    }

}
