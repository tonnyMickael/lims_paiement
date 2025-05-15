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
    public class SousContratController : ControllerBase
    {
        private readonly ISousContratService _sousContratService;

        public SousContratController(ISousContratService sousContratService)
        {
            _sousContratService = sousContratService;
        }

        [HttpGet("ListeSousContrat")]
        public async Task<IActionResult> GetListeContrat() 
        {
            var list = await _sousContratService.GetAllListeSousContrat();
            return Ok(list);
        }

        [HttpGet("ListeSousContratApayer")]
        public async Task<IActionResult> GetListeSousContratApayer()
        {
            var list = await _sousContratService.GetListeSousContratApayerAsync();
            return Ok(list);
        }

        [HttpPut("UpdateSousContrat/{id_etat_decompte}")]
        public async Task<IActionResult> UpdateSousContrat(int id_etat_decompte)
        {
            await _sousContratService.UpdateEtatPaiementSousContrat(id_etat_decompte);
            var isSuccess = true; // Assuming success is determined elsewhere or always true

            if (isSuccess)
            {
                return Ok(new { Message = "Update successful" });
            }
            else
            {
                return BadRequest(new { Message = "Update failed" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRefusPaeiemnt([FromBody] SousContratDto contrat)
        {
            if (contrat== null)
            {
                return BadRequest("Les données du contrat sont invalides.");
            }
            await _sousContratService.AddSousContratAsync(contrat);

            var reponse = new ApiResponse
            {
                Data = contrat,
                Message = "Paiement sous contrat enregistrer, en attente de la confirmation à la date prévus par le contrat.",
                IsSuccess = true,
                StatusCode = 200
            };

            return Ok(reponse);
        }
    }
}
