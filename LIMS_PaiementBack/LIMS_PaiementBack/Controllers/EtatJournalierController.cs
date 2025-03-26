using LIMS_PaiementBack.Services.EtatJournalier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LIMS_PaiementBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EtatJournalierController : ControllerBase
    {
        private readonly IEtatJournalierService _journalier;

        public EtatJournalierController(IEtatJournalierService journalier)
        {
            _journalier = journalier;
        }

        [HttpGet]
        public async Task<IActionResult> EncaissementJournalier()
        {            
            var encaissement = await _journalier.GetAllEtatJournalier();
            return Ok(encaissement);
        }
    }
}
