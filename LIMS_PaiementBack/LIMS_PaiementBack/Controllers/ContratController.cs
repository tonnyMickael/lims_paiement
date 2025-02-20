using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LIMS_PaiementBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratController : ControllerBase
    {
        private readonly IContratService _contrat;

        public ContratController(IContratService contrat)
        {
            _contrat = contrat;
        }


        //liste de tout les contrat 
        [HttpGet]
        public async Task<IActionResult> GetAllContrat()
        {
            var response = await _contrat.GetContratPartenaire();
            return Ok(response);
        }

        //ajout de nouveau contrat
        [HttpPost]
        public async Task<IActionResult> AddNewContrat([FromBody] ContratDto contrat)
        {
            if (contrat == null)
            {
                return BadRequest("Les données du contrat sont invalides.");
            }
            await _contrat.AddContratAsync(contrat);
            return Ok(contrat);
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
            return Ok(contrat);
        }
    }
}
