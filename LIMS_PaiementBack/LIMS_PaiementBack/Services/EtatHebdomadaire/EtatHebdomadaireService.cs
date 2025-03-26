using LIMS_PaiementBack.Entities;
using LIMS_PaiementBack.Models;
using LIMS_PaiementBack.Repositories.EtatHebdomadaire;
using LIMS_PaiementBack.Utils;

namespace LIMS_PaiementBack.Services.EtatHebdomadaire
{
    public class EtatHebdomadaireService : IEtatHebdomadaireService
    {
        private readonly IEtatHebdomadaireRepository _hebdomadaireRepository;

        public EtatHebdomadaireService(IEtatHebdomadaireRepository hebdomadaireRepository)
        {
            _hebdomadaireRepository = hebdomadaireRepository;
        }

        public async Task<ApiResponse> AllGetSemaine()
        {
            return await _hebdomadaireRepository.GetAllSemaine();
        }

        public async Task<ApiResponse> GetAllVersement()
        {
            return await _hebdomadaireRepository.GetAllVersementHebdomadaire();
        }

        public async Task SemaineAdd(SemaineDto week)
        {
            var semaine = new SemaineEntity
            {
                DebutSemaine = week.debutSemaine ?? default(DateTime),
                FinSemaine = week.finSemaine ?? default(DateTime)
            };

            await _hebdomadaireRepository.AddSemaine(semaine);
        }
    }
}
