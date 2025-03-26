using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Services.EtatHebdomadaire;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LIMS_PaiementBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EtatHebdomadaireController : ControllerBase
    {
        private readonly IEtatHebdomadaireService _hebdomadaire;

        public EtatHebdomadaireController(IEtatHebdomadaireService hebdomadaire)
        {
            _hebdomadaire = hebdomadaire;
        }

        [HttpPost]
        public async Task<IActionResult> Semaine([FromBody] SemaineDto semaine)
        {
            if (semaine == null)
            {
                return BadRequest("Les données de la demande sont invalides.");
            }
            await _hebdomadaire.SemaineAdd(semaine);

            var reponse = new ApiResponse
            {
                Data = semaine,
                Message = "nouvelle semaine ajouter avc succès",
                IsSuccess = true,
                StatusCode = 200
            };

            return Ok(reponse);
        }

        [HttpGet("semaine")]
        public async Task<IActionResult> MoisSemaine()
        {
            var moisSemaine = await _hebdomadaire.AllGetSemaine();
            return Ok(moisSemaine);
        }

        [HttpGet("transferer")]
        public async Task<IActionResult> VersementHebdomadaire()
        {
            var versement = await _hebdomadaire.GetAllVersement();
            return Ok(versement);
        }
    }
}
