using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Services;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace LIMS_PaiementBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionVirementPaiementController : ControllerBase
    {
        private readonly IReceptionVirementPaiementService _receptionVirementPaiement;

        public ReceptionVirementPaiementController(IReceptionVirementPaiementService receptionVirementPaiement)
        {
            _receptionVirementPaiement = receptionVirementPaiement;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaiementVirement()
        {
            var paiementEspece = await _receptionVirementPaiement.GetVirementAPayer();
            return Ok(paiementEspece);
        }

        [HttpPost]
        public async Task<IActionResult> VirementRecu([FromBody] RecuDto recu)
        {
            if (recu == null)
            {
                return BadRequest("Les données du recu sont invalides.");
            }
            await _receptionVirementPaiement.AddVirementPaiementRecu(recu);

            var reponse = new ApiResponse
            {
                Data = recu,
                Message = "Paiement en espéce enregistrer, en attente de récéption",
                IsSuccess = true,
                StatusCode = 200
            };

            return Ok(reponse);
        }
    }
}
