using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Services.Depart;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LIMS_PaiementBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartController : ControllerBase
    {
        private readonly IDepartService _depart;

        public DepartController(IDepartService depart)
        {
            _depart = depart;
        }

        [HttpGet("listeDepart")]
        public async Task<IActionResult> GetDepartsDemande()
        {
            var departs = await _depart.GetAllDeparts();
            return Ok(departs);
        }

        [HttpGet("listeDestinataire")]
        public async Task<IActionResult> GetDestinataire()
        {
            var destinataires = await _depart.GetAllDestinataire();
            return Ok(destinataires);
        }

        [HttpPost("addDepart")]
        //public async Task<IActionResult> AddDepart([FromBody]DepartDto depart)
        public async Task<IActionResult> AddDepart(DepartDto depart)
        {            
            // Console.WriteLine($"Envoi des données entree 1 : {JsonSerializer.Serialize(depart)}");

            if (depart == null)
            {
                return BadRequest("Les données du depart sont invalides.");
            }

            if (string.IsNullOrEmpty(depart.objet))
            {
                // Console.WriteLine($"Envoi des données entree 2 : {JsonSerializer.Serialize(depart)}");
                return BadRequest("L'objet du depart est obligatoire.");
            }
                       
            await _depart.AddDepart(depart);

            var reponse = new ApiResponse
            {
                Data = depart,
                Message = "nouvelle depart ajouter",
                IsSuccess = true,
                StatusCode = 200
            };
            return Ok(reponse);
        }      
    }
}