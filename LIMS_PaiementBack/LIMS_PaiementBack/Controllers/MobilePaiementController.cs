using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Services;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LIMS_PaiementBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MobilePaiementController : ControllerBase
    {
        private readonly IMobilePaiementService _mobilePaiement;

        public MobilePaiementController(IMobilePaiementService mobilePaiement)
        {
            _mobilePaiement = mobilePaiement;
        }

        [HttpGet("{id_etat_decompte}")]
        public async Task<IActionResult> GetInfoMobilePaiement(int id_etat_decompte)
        {
            var response = await _mobilePaiement.GetInfoMobilePaiement(id_etat_decompte);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddConfirmationMobile([FromBody] PaiementDto mobile)
        {
            if (mobile == null)
            {
                return BadRequest("Les données du delai sont invalides.");
            }
            await _mobilePaiement.AddMobilePaiement(mobile);

            var reponse = new ApiResponse
            {
                Data = mobile,
                Message = "Paiement par mobile enregistrer, en attente de récéption",
                IsSuccess = true,
                StatusCode = 200
            };

            return Ok(reponse);
        }
    }
}
