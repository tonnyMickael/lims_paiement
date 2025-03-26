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

    }
}
