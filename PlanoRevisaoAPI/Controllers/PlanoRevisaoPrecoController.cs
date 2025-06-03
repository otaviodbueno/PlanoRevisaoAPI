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
        [Route("PostPorCdGrupoUnico")]
        public ActionResult<PlanoRevisaoPrecoModelView> PostValoresRevisaoPorCdRegiao(PlanoRevisaoPrecoModelView planoRevisaoPrecoModelView)
        {
            var postPreco = _planoRevisaoPrecoBusiness.PostPrecoRevisoes(planoRevisaoPrecoModelView);
            return Created("", postPreco);
        }

        [HttpPost]
        public ActionResult<List<PlanoRevisaoPrecoModelView>> PostValoresPorRegioes(List<PlanoRevisaoPrecoModelView> listPrecosRegioes)
        {
            var postPrecoRegioes = _planoRevisaoPrecoBusiness.PostPrecoRegioes(listPrecosRegioes);
            return Created("", postPrecoRegioes);
        }

        [HttpPut]
        public ActionResult<PlanoRevisaoPrecoModelView> AtualizarValores(PlanoRevisaoPrecoModelView planoRevisaoPrecoModelView)
        {
            var putPreco = _planoRevisaoPrecoBusiness.AtualizaValores(planoRevisaoPrecoModelView);
            return Created("", putPreco);
        }

        [HttpDelete("{idPlanoRevisaoTipo}")]
        public ActionResult<PlanoRevisaoPrecoSemIdModelView> DeletaPrecoRevisao(int idPlanoRevisaoTipo)
        {
            _planoRevisaoPrecoBusiness.DeletaPrecoRevisaoPorTipo(idPlanoRevisaoTipo);
            return NoContent();
        }
    }
}
