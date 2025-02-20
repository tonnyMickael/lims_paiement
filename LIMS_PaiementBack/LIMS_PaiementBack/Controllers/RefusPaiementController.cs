using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(refus);
        }

    }
}
