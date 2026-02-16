using AnkietyPPK.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnkietyPPK.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //konfiguracja relacji Survey -> Options (1:N)
            modelBuilder.Entity<Option>()
                .HasOne(o => o.Survey)
                .WithMany(s => s.Options)
                .HasForeignKey(o => o.SurveyId)
                .OnDelete(DeleteBehavior.Cascade);

            //konfiguracja relacji Option -> Votes (1:N)
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Option)
                .WithMany(o => o.Votes)
                .HasForeignKey(v => v.OptionId)
                .OnDelete(DeleteBehavior.Cascade);

            //indeks zapobiegający wielokrotnemu głosowaniu
            //respondent może zagłosować tylko raz w danej ankiecie
            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.OptionId, v.RespondentUserId });
        }
    }
}