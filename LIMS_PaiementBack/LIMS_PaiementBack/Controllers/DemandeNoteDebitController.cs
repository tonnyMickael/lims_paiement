using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Services;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LIMS_PaiementBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemandeNoteDebitController : ControllerBase
    {
        private readonly IDemandeService _service;

        public DemandeNoteDebitController(IDemandeService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDemande([FromBody] DemandeDto demande)
        {
            if (demande == null)
            {
                return BadRequest("Les données de la demande sont invalides.");
            }                  
            await _service.AddDemandeAsync(demande);
            return Ok(demande);
        }

        [HttpGet("NoteDoitfaire")]
        public async Task<IActionResult> GetListNoteDoitFaire()
        {
            var Afaire = await _service.GetDemandeListAfaire();
            return Ok(Afaire);
        }

        [HttpGet("{id_etat_decompte}")]
        public async Task<IActionResult> GetDemande(int id_etat_decompte)
        {
            int id = id_etat_decompte;
            var demandes = await _service.GetDemandesAsync(id);
            return Ok(demandes);
        }

        [HttpGet("Liste")]
        public async Task<IActionResult> GetListAllNoteDebit()
        {
            var liste = await _service.GetDemandeListNoteAsync();
            return Ok(liste);
        }

        [HttpGet("Verification")]
        public async Task<IActionResult> GetResponseVerification()
        {
            var listeVerification = await _service.VerificationOublie();
            return Ok(listeVerification);
        }
    }
}
