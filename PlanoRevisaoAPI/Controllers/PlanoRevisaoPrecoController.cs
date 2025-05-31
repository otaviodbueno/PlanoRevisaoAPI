using Microsoft.AspNetCore.Mvc;
using PlanoRevisaoAPI.Business;
using PlanoRevisaoAPI.ModelView;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlanoRevisaoAPI.Controllers
{
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
        public ActionResult<PlanoRevisaoPrecoModelView> PostValoresRevisaoPorCdRegiao(PlanoRevisaoPrecoModelView planoRevisaoPrecoModelView)
        {
            var post = _planoRevisaoPrecoBusiness.PostPrecoRevisoes(planoRevisaoPrecoModelView);
            return Created("", post);
        }

        [HttpPut]
        public ActionResult<PlanoRevisaoPrecoModelView> AtualizarValores(PlanoRevisaoPrecoModelView planoRevisaoPrecoModelView)
        {
            var put = _planoRevisaoPrecoBusiness.AtualizaValores(planoRevisaoPrecoModelView);
            return Created("", put);
        }
    }
}
