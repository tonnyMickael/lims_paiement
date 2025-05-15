using Microsoft.EntityFrameworkCore;

namespace LIMS_PaiementBack.Entities
{
    public class DbContextEntity : DbContext
    {
        public DbContextEntity(DbContextOptions options) : base(options) { }

        //table existant dans la base de donnée
        public DbSet<ClientEntity> Client { get; set; }
        public DbSet<DepartEntity> Depart { get; set; }
        public DbSet<PrestationEntity> Prestation { get; set; }
        public DbSet<EchantillonEntity> Echantillon { get; set; }
        public DbSet<TypeTravauxEntity> Type_travaux { get; set; }
        public DbSet<DestinataireEntity> Destinataire { get; set; }
        public DbSet<EtatDecompteEntity> Etat_decompte { get; set; }
        public DbSet<TypeEchantillonEntity> Type_echantillon { get; set; }
        public DbSet<Details_etat_decompte_Entity> Details_etat_decompte { get; set; }
        public DbSet<Type_travaux_type_echantillonEntity> Type_travaux_type_echantillon { get; set; }

        //mes table à créer dans la base de donnée 
        public DbSet<SemaineEntity> Semaine { get; set; }
        public DbSet<PaiementEntity> Paiement { get; set; }
        public DbSet<DelaiEntity> DelaiPaiement { get; set; }
        public DbSet<SousContratEntity> SousContrats { get; set; }
        public DbSet<DemandeEntity> DemandeNoteDebit { get; set; }
        public DbSet<EtatJournalierEntity> EtatJournalier { get; set; }
        public DbSet<OrdreDeVirementEntity> OrdreDeVirement { get; set; }
        public DbSet<ReceptionEspeceEntity> ReceptionEspece { get; set; }
        public DbSet<ReceptionMobileEntity> ReceptionMobile { get; set; }
        public DbSet<EtatHebdomadaireEntity> EtatHebdomadaire { get; set; }

        // public DbSet<EtatPrestationEntity> Etat_prestation { get; set; }
        //public DbSet<PartenaireEntity> Partenaire { get; set; }
        // public DbSet<ContratPartenaireEntity> ContratPartenaire {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //configuration pour ne pas modifier la structure de la base
            modelBuilder.Entity<ClientEntity>().ToTable("Client");
            modelBuilder.Entity<DepartEntity>().ToTable("Depart");
            modelBuilder.Entity<PrestationEntity>().ToTable("Prestation");
            modelBuilder.Entity<EchantillonEntity>().ToTable("Echantillon");
            modelBuilder.Entity<TypeTravauxEntity>().ToTable("Type_travaux");
            modelBuilder.Entity<DestinataireEntity>().ToTable("Destinataire");
            modelBuilder.Entity<EtatDecompteEntity>().ToTable("Etat_decompte");
            modelBuilder.Entity<TypeEchantillonEntity>().ToTable("Type_echantillon");
            modelBuilder.Entity<Details_etat_decompte_Entity>().ToTable("Details_etat_decompte");
            modelBuilder.Entity<Type_travaux_type_echantillonEntity>().ToTable("Type_travaux_type_echantillon").HasKey(e => new { e.id_type_echantillon, e.id_type_travaux });
            
            //modelBuilder.Entity<EtatPrestationEntity>().ToTable("Etat_prestation");

            //nouvelle relation
            modelBuilder.Entity<DemandeEntity>()
                .HasOne(e => e.etatDecompte)
                .WithMany()
                .HasForeignKey(e => e.id_etat_decompte)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaiementEntity>()
                .HasOne(e => e.etatdecompte)
                .WithMany()
                .HasForeignKey(e => e.id_etat_decompte)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EtatJournalierEntity>()
                .HasOne(e => e.etatDecompte)
                .WithMany()
                .HasForeignKey(e => e.id_etat_decompte)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DepartEntity>()
                .HasOne(e => e.Destinataire)
                .WithMany(m => m.departs)
                .HasForeignKey(e => e.idDestinataire)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DelaiEntity>()
                .HasOne(e => e.Paiement)
                .WithMany(m => m.Delais)
                .HasForeignKey(e => e.idPaiement)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrdreDeVirementEntity>()
                .HasOne(e => e.paiement)
                .WithMany(m => m.ordreDeVirements)
                .HasForeignKey(e => e.idPaiement)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReceptionEspeceEntity>()
                .HasOne(e => e.Paiement)
                .WithMany(m => m.receptionEspeces)
                .HasForeignKey(e => e.idPaiement)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReceptionMobileEntity>()
                .HasOne(e => e.Paiement)
                .WithMany(m => m.receptionMobiles)
                .HasForeignKey(e => e.idPaiement)
                .OnDelete(DeleteBehavior.Restrict);
                
            /*
                // modelBuilder.Entity<SousContratEntity>()
                //     .HasOne(e => e.paiement)
                //     .WithMany(m => m.sousContrat)
                //     .HasForeignKey(e => e.idPaiement)
                //     .OnDelete(DeleteBehavior.Restrict);

                // modelBuilder.Entity<SousContratEntity>()
                //     .HasOne(e => e.partenaire)
                //     .WithMany(m => m.sousContrat)
                //     .HasForeignKey(e => e.idPartenaire)
                //     .OnDelete(DeleteBehavior.Restrict);

                // modelBuilder.Entity<SousContratEntity>()
                //     .HasOne(e => e.client)
                //     .WithMany(m => m.sousContrat)
                //     .HasForeignKey(e => e.id_client)
                //     .OnDelete(DeleteBehavior.Restrict);

                // modelBuilder.Entity<ContratPartenaireEntity>()
                //     .HasOne(e => e.partenaire)
                //     .WithMany(m => m.contrat)
                //     .HasForeignKey(e => e.idPartenaire)
                //     .OnDelete(DeleteBehavior.Restrict);
                // modelBuilder.Entity<ContratPartenaireEntity>()
            */

            modelBuilder.Entity<EtatHebdomadaireEntity>()
                .HasOne(e => e.semaine)
                .WithMany(m => m.Hebdomadaire)
                .HasForeignKey(e => e.idSemaine)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EtatHebdomadaireEntity>()
                .HasOne(e => e.journalier)
                .WithMany(m => m.hebdomadaires)
                .HasForeignKey(e => e.idEtatJournalier)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}