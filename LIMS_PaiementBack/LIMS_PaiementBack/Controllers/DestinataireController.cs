using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Services;
using LIMS_PaiementBack.Services.Depart;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace LIMS_PaiementBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DestinataireController : ControllerBase
    {
        private readonly IDestinataireService _destinataire;

        public DestinataireController(IDestinataireService destinataire)
        {
            _destinataire = destinataire;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDestinataire(int position, int pagesize)
        {
            var response = await _destinataire.GetAllDestinataire(position, pagesize);
            return Ok(response);
        }

        [HttpPost("new")]
        public async Task<IActionResult> AddNewContrat([FromBody] DestinataireDto destinataire)
        {
            if (destinataire == null)
            {
                return BadRequest("Les données du destinataire sont invalides.");
            }
            await _destinataire.AddDestinataire(destinataire);

            var reponse = new ApiResponse
            {
                Data = destinataire,
                Message = "nouvelle destinataire ajouter",
                IsSuccess = true,
                StatusCode = 200
            };
            return Ok(reponse);
        }

        [HttpGet("{id_destinataire}")]
        public async Task<IActionResult> GetModificationDeDestinatare(int id_destinataire)
        {
            var response = await _destinataire.GetModification(id_destinataire);
            return Ok(response);
        }

        [HttpPut("{Id_destinataire}")]
        public async Task<IActionResult> ModificationDestinataire(int Id_destinataire, [FromBody] DestinataireDto destinataire)
        {
            if (destinataire == null)
            {
                return BadRequest("Les données du contrat sont invalides.");
            }
            await _destinataire.ModifierDestinataire(Id_destinataire,destinataire);

            var reponse = new ApiResponse
            {
                Data = destinataire,
                Message = "nouvelle destinataire ajouter",
                IsSuccess = true,
                StatusCode = 200
            };

            return Ok(reponse);
        }
    }
}
