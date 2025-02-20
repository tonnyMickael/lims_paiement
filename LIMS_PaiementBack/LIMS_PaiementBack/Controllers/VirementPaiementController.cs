using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddConfirmationVirement([FromBody] PaiementDto espece)
        {
            if (espece == null)
            {
                return BadRequest("Les données du delai sont invalides.");
            }
            await _virementPaiement.AddVirementPaiement(espece);
            return Ok(espece);
        }
    }
}
