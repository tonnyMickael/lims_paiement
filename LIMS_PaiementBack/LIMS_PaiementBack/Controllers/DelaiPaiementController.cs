using System.Reflection;
using LIMS_PaiementBack.Utils;
using LIMS_PaiementBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LIMS_PaiementBack.Services;

namespace LIMS_PaiementBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DelaiPaiementController : ControllerBase
    {
        private readonly IDelaiService _service;

        public DelaiPaiementController(IDelaiService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddDelaiProuver([FromBody] DelaiDto delai)
        {
            if (delai == null)
            {
                return BadRequest("Les données du delai sont invalides.");
            }
            await _service.AddDelaiAsync(delai);

            var reponse = new ApiResponse
            {
                Data = delai,
                Message = "délai de paiement enregistrer",
                IsSuccess = true,
                StatusCode = 200
            };

            return Ok(reponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDelaiPaiement()
        {           
            var response = await _service.GetDelaiPaiement();
            return Ok(response);
        }

        [HttpGet("DelaiAccorder/{id_etat_decompte}")]
        public async Task<IActionResult> GetDelaiForClient(int id_etat_decompte)
        {
            var response = await _service.GetDelaiAccorder(id_etat_decompte);
            return Ok(response);
        }

        [HttpGet("DelaiApayer")]
        public async Task<IActionResult> GetDelaiApayer()
        {
            var response = await _service.GetDelaiApayer();
            return Ok(response);
        }

        [HttpPut("PaiementDirect/{id_etat_decompte}/{modepaiement}")]
        public async Task<IActionResult> PaiementDelaiDirect(int id_etat_decompte, int modepaiement)
        {
            await _service.PaiementDelaiDirectAsync(id_etat_decompte, modepaiement);
            var reponse = new ApiResponse
            {
                Message = "paiement effectuer avec succès",
                IsSuccess = true,
                StatusCode = 200
            };
            return Ok(reponse);
        }

        [HttpPut("PaiementParChangement/{id_etat_decompte}/{modepaiement}")]
        public async Task<IActionResult> PaiementDelaiParChangement(int id_etat_decompte, int modepaiement)
        {
            await _service.PaiementDelaiParChangementAsync(id_etat_decompte, modepaiement);
            var reponse = new ApiResponse
            {
                Message = "paiement effectuer avec succès",
                IsSuccess = true,
                StatusCode = 200
            };
            return Ok(reponse);
        }

        [HttpGet("DelaiEnAttente")]
        public async Task<IActionResult> GetDelaiEnAttente()
        {
            var response = await _service.GetDelaiEnAttenteAsync();
            return Ok(response);
        }

        [HttpGet("PrestationApayer")]
        public async Task<IActionResult> GetPrestationApayer()
        {
            var response = await _service.GetPrestationApayerAsync();
            return Ok(response);
        }
    }
}
