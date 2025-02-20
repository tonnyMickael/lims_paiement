using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LIMS_PaiementBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceptionEspecePaiementController : ControllerBase
    {
        private readonly IReceptionEspecePaiementService _receptionEspecePaiement;

        public ReceptionEspecePaiementController(IReceptionEspecePaiementService receptionEspecePaiement)
        {
            _receptionEspecePaiement = receptionEspecePaiement;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaiementEspece() 
        {
            var paiementEspece = await _receptionEspecePaiement.GetEspeceAPayer();
            return Ok(paiementEspece);
        }

        [HttpPost]
        public async Task<IActionResult> EspeceRecu([FromBody] RecuDto recu) 
        {
            if (recu == null)
            {
                return BadRequest("Les données du recu sont invalides.");
            }
            await _receptionEspecePaiement.AddEspecePaiementRecu(recu);
            return Ok(recu);
        }
    }
}
