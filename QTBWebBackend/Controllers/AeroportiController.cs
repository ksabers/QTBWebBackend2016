using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QTBWebBackend.Interfaces;
using QTBWebBackend.ViewModels;

namespace QTBWebBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AeroportiController(IAeroportiService servizio) : ControllerBase
    {

        // GET: api/aeroporti
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAeroporti()
        {
            var aeroporti = await servizio.GetAeroportiAsync();
            return Ok(aeroporti);
        }

        // GET: api/aeroporti/5
        [HttpGet("{idAeroporto:long}")]
        [Authorize]
        public async Task<IActionResult> GetAeroporto(long idAeroporto)
        {
            var aeroporto = await servizio.GetAeroportoByIdAsync(idAeroporto);
            if (aeroporto == null)
                return NotFound();

            return Ok(aeroporto);
        }

        // GET: api/aeroporti/tipi
        [HttpGet("tipi")]
        [Authorize]
        public async Task<IActionResult> GetTipiAeroporti()
        {
            var tipi = await servizio.GetTipiAeroportiAsync();
            return Ok(tipi);
        }

        // GET: api/aeroporti/tipi/3
        [HttpGet("tipi/{idTipoAeroporto:long}")]
        [Authorize]
        public async Task<IActionResult> GetTipoAeroporto(long idTipoAeroporto)
        {
            var tipo = await servizio.GetTipoAeroportoByIdAsync(idTipoAeroporto);
            if (tipo == null)
                return NotFound();

            return Ok(tipo);
        }

        // POST: api/aeroporti
        [HttpPost]
        [Authorize(Roles = "Amministratore")] // esempio: solo admin possono creare
        public async Task<IActionResult> PostAeroporto([FromBody] AeroportoViewModel aeroporto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var aeroportoCreato = await servizio.CreateAeroportoAsync(aeroporto);
            if (aeroportoCreato == null)
                return BadRequest("Impossibile creare l'aeroporto.");

            // opzionale: puoi restituire il ViewModel invece dell’entity
            return CreatedAtAction(
                nameof(GetAeroporto),
                new { idAeroporto = aeroportoCreato.Id },
                aeroportoCreato);
        }
    }
}
