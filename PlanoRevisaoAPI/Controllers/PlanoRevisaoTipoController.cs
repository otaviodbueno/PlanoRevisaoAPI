using Microsoft.AspNetCore.Mvc;
using PlanoRevisaoAPI.Business;
using PlanoRevisaoAPI.ModelView;

namespace PlanoRevisaoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanoRevisaoTipoController : ControllerBase
    {
        private readonly IPlanoRevisaoTipoBusiness _planoRevisaoTipoBusiness;

        public PlanoRevisaoTipoController(IPlanoRevisaoTipoBusiness planoRevisaoTipoBusiness)
        {
            _planoRevisaoTipoBusiness = planoRevisaoTipoBusiness;
        }

        [HttpGet("{id}")]
        public ActionResult<List<PlanoRevisaoTipoModelView>> GetPorIdPlanoRevisao(int id)
        {
            var tiposPlanoRevisao = _planoRevisaoTipoBusiness.GetById(id);
            return Ok(tiposPlanoRevisao);
        }
    }
}
