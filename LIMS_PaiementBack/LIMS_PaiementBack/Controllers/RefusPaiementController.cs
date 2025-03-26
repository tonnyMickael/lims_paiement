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
    public class RefusPaiementController : ControllerBase
    {
        private readonly IRefusPaiementService _refusPaiementService;

        public RefusPaiementController(IRefusPaiementService refusPaiementService)
        {
            _refusPaiementService = refusPaiementService;
        }

        [HttpGet]
        public async Task<ActionResult> GetListRefus() 
        {
            var list = await _refusPaiementService.GetAllListRefus();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRefusPaeiemnt([FromBody] RefusDto refus)
        {
            if (refus == null)
            {
                return BadRequest("Les données du refus sont invalides.");
            }
            await _refusPaiementService.AddRefusPaiementAsync(refus);

            var reponse = new ApiResponse
            {
                Data = refus,
                Message = "Paiement en espéce enregistrer, en attente de récéption",
                IsSuccess = true,
                StatusCode = 200
            };

            return Ok(reponse);
        }

    }
}
