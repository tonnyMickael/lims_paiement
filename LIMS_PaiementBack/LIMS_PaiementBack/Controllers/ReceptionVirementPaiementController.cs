using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(recu);
        }
    }
}
