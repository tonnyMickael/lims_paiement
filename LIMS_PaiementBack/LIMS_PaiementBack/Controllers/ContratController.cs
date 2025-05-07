using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Services;
using LIMS_PaiementBack.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LIMS_PaiementBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContratController : ControllerBase
    {
        /*private readonly IContratService _contrat;

        public ContratController(IContratService contrat)
        {
            _contrat = contrat;
        }             

        //liste de tout les contrat 
        [HttpGet]
        public async Task<IActionResult> GetAllContrat(int position, int pagesize)
        {
            var response = await _contrat.GetContratPartenaire(position, pagesize);
            return Ok(response);
        }

        //ajout de nouveau contrat
        [HttpPost("new")]
        public async Task<IActionResult> AddNewContrat([FromBody] ContratDto contrat)
        {
            if (contrat == null)
            {
                return BadRequest("Les données du contrat sont invalides.");
            }
            await _contrat.AddContratAsync(contrat);

            var reponse = new ApiResponse
            {
                Data = contrat,
                Message = "nouvelle partenaire sous contrat ajouter",
                IsSuccess = true,
                StatusCode = 200
            };
            return Ok(reponse);
        }

        [HttpPost("souscontrat")]
        public async Task<IActionResult> AddSousContratPaiement([FromBody] SousContratDto data)
        {
            if (data == null)
            {
                return BadRequest("Les données du contrat sont invalides.");
            }

            // Récupération des données
            var paiement = data.Paiement;
            var contrat = data.Contrat;

            await _contrat.AddPaiementContratAsync(paiement,contrat);

            var reponse = new ApiResponse
            {
                Data = contrat,
                Message = "paiement sous contrat enregistrer",
                IsSuccess = true,
                StatusCode = 200
            };
            return Ok(reponse);
        }

        //afficher les donnée à modifier
        [HttpGet("{id_partenaire}")]
        public async Task<IActionResult> Get_Contrat_A_Modifier(int id_partenaire)
        {
            var response = await _contrat.Get_contrat_modifier(id_partenaire);
            return Ok(response); 
        }

        //modification de partenariat rupture de contrat
        [HttpPut("{id_partenaire}/{id_contrat}")]
        public async Task<IActionResult> ModificationContrat(int id_partenaire, int id_contrat, [FromBody] ContratDto contrat)
        {
            if (contrat == null)
            {
                return BadRequest("Les données du contrat sont invalides.");
            }
            await _contrat.ModifierContrat(id_partenaire, id_contrat, contrat);

            var reponse = new ApiResponse
            {
                Data = contrat,
                Message = "nouvelle destinataire ajouter",
                IsSuccess = true,
                StatusCode = 200
            };

            return Ok(reponse);
        }*/
    }
}
