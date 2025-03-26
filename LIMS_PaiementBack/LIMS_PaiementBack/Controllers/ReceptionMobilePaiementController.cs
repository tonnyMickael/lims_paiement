using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Services;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace LIMS_PaiementBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceptionMobilePaiementController : ControllerBase
    {
        private readonly IReceptionMobilePaiementService _receptionMobilePaiement;

        public ReceptionMobilePaiementController(IReceptionMobilePaiementService receptionMobilePaiement)
        {
            _receptionMobilePaiement = receptionMobilePaiement;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaiementMobile()
        {
            var paiementEspece = await _receptionMobilePaiement.GetMobileAPayer();
            return Ok(paiementEspece);
        }

        [HttpPost]
        public async Task<IActionResult> MobileRecu([FromBody] RecuDto recu)
        {
            if (recu == null)
            {
                return BadRequest("Les données du recu sont invalides.");
            }
            await _receptionMobilePaiement.AddMobilePaiementRecu(recu);

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
