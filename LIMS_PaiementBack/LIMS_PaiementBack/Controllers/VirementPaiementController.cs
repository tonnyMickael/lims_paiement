using System.Reflection;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Mvc;
using LIMS_PaiementBack.Models;
using Microsoft.AspNetCore.Http;
using LIMS_PaiementBack.Services;

namespace LIMS_PaiementBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VirementPaiementController : ControllerBase
    {
        private readonly IVirementPaiementService _virementPaiement;

        public VirementPaiementController(IVirementPaiementService virementPaiement)
        {
            _virementPaiement = virementPaiement;
        }

        [HttpGet("{id_etat_decompte}")]
        public async Task<IActionResult> GetInfoVirementPaiement(int id_etat_decompte)
        {
            var response = await _virementPaiement.GetInfoVirementPaiement(id_etat_decompte);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddConfirmationVirement([FromBody] PaiementDto virement)
        {
            if (virement == null)
            {
                return BadRequest("Les données du delai sont invalides.");
            }
            await _virementPaiement.AddVirementPaiement(virement);

            var reponse = new ApiResponse
            {
                Data = virement,
                Message = "Paiement par virement enregistrer, en attente de récéption",
                IsSuccess = true,
                StatusCode = 200
            };

            return Ok(reponse);
        }

        [HttpGet("VirementListe")]
        public async Task<IActionResult> GetAllVirementApayer()
        {
            var response = await _virementPaiement.GetListeVirementApayerAsync();
            return Ok(response);
        }
    }
}
