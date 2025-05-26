using Microsoft.AspNetCore.Mvc;
using PlanoRevisaoAPI.Business;
using PlanoRevisaoAPI.ModelView;

namespace PlanoRevisaoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TipoRevisaoController : ControllerBase
{
    private ITipoRevisaoBusiness _tipoRevisaoBusiness;

    public TipoRevisaoController(ITipoRevisaoBusiness tipoRevisaoBusiness)
    {
        _tipoRevisaoBusiness = tipoRevisaoBusiness;
    }

    [HttpGet]
    [Route("GetTipoRevisaoAtivas")]
    public ActionResult<List<TipoRevisaoModelView>> GetTipoRevisaoAtivas()
    {
        var tiposRevisaoAtivas = _tipoRevisaoBusiness.GetTiposRevisaoAtivas();
        return Ok(tiposRevisaoAtivas);
    }
}
