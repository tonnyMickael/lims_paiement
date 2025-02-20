using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LIMS_PaiementBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EspecePaiementController : ControllerBase
    {
        private readonly IEspecePaiementService _especePaiement;

        public EspecePaiementController(IEspecePaiementService especePaiement)
        {
            _especePaiement = especePaiement;
        }

        [HttpGet("{id_etat_decompte}")]
        public async Task<IActionResult> GetInfoEspecePaiement(int id_etat_decompte)
        {
            var response = await _especePaiement.GetInfoEspecePaiement(id_etat_decompte);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddConfirmationEspece([FromBody] PaiementDto espece)
        {
            if (espece == null)
            {
                return BadRequest("Les données du delai sont invalides.");
            }
            await _especePaiement.AddEspecePaiement(espece);
            return Ok(espece);
        }
    }
}
