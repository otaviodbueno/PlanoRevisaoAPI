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

        [HttpGet]
        public ActionResult<List<PlanoRevisaoTipoModelView>> GetTiposPlanoRevisao()
        {
            var tiposPlanoRevisao = _planoRevisaoTipoBusiness.GetAllPlanosRevisaoTipo();
            return Ok(tiposPlanoRevisao);
        }

        [HttpGet]
        [Route("ListTiposReembolsaveis")]
        public ActionResult<List<PlanoRevisaoTipoModelView>> ListTiposReembolsaveis()
        {
            var tiposPlanoRevisao = _planoRevisaoTipoBusiness.ListPlanoRevisaoTipoReembolsaveis();
            return Ok(tiposPlanoRevisao);
        }

        [HttpPost]
        public ActionResult<PlanoRevisaoTipoModelView> PostTipoPlanoRevisao(PlanoRevisaoTipoModelView tipos)
        {
            var tiposPlanoRevisao = _planoRevisaoTipoBusiness.PostPlano(tipos);
            return Created("", tiposPlanoRevisao);
        }

        [HttpPost("PostPlanoBase")]
        public ActionResult PostByPlanoBase(PlanoBaseRequestModelView plano)
        {
            var tiposPlanoRevisao = _planoRevisaoTipoBusiness.PostComPlanoBase(plano.idPlanoNovo, plano.idPlanoBase);
            return Created("", tiposPlanoRevisao);
        }

        [HttpPut]
        public ActionResult UpdateTipoPlanoRevisao(PlanoRevisaoTipoModelView planoRevisaoTipoModelView)
        {
            var tiposAtualizados = _planoRevisaoTipoBusiness.AtualizarPlanoRevisaoTipo(planoRevisaoTipoModelView);
            return Created("", tiposAtualizados);
        }

        [HttpDelete("{idPlanoRevisao}")]
        public ActionResult<PlanoRevisaoTipoModelView> DeleteTipoPlanoRevisao(int idPlanoRevisao)
        {
            var tiposPlanoRevisao = _planoRevisaoTipoBusiness.DeleteTipoPlanoRevisao(idPlanoRevisao);
            return NoContent();
        }
    }
}
