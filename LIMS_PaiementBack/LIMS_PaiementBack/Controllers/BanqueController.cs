using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Services;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace LIMS_PaiementBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BanqueController : ControllerBase
    {
        private readonly IBanqueService _banque;

        public BanqueController(IBanqueService banque)
        {
            _banque = banque;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBanque(int position, int pagesize)
        {
            var response = await _banque.AllBanqueAsync(position, pagesize);
            return Ok(response);
        }

        [HttpPost("new")]
        public async Task<IActionResult> AddNewBanque([FromBody] BanqueDto banque)
        {
            if (banque == null)
            {
                return BadRequest("Les données du destinataire sont invalides.");
            }
            await _banque.AddNewBanqueAsync(banque);

            var reponse = new ApiResponse
            {
                Data = banque,
                Message = "nouvelle banque ajouter",
                IsSuccess = true,
                StatusCode = 200
            };
            return Ok(reponse);
        }

        [HttpGet("{id_banque}")]
        public async Task<IActionResult> GetModificationDeBanque(int id_banque)
        {
            var response = await _banque.GetModificationBanqueAsync(id_banque);
            return Ok(response);
        }

        [HttpPut("{id_banque}")]
        public async Task<IActionResult> ModificationBanque(int id_banque, [FromBody] BanqueDto banque)
        {
            if (banque == null)
            {
                return BadRequest("Les données du contrat sont invalides.");
            }
            await _banque.ModificationBanqueAsync(id_banque,banque);

            var reponse = new ApiResponse
            {
                Data = banque,
                Message = "nouvelle banque ajouter",
                IsSuccess = true,
                StatusCode = 200
            };

            return Ok(reponse);
        }
    }
}
