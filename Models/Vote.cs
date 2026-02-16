using System.ComponentModel.DataAnnotations;

namespace AnkietyPPK.Models
{
    public class Vote
    {
        public int Id { get; set; }

        // Klucz obcy do opcji
        public int OptionId { get; set; }

        // Nawigacja do opcji
        public Option Option { get; set; } = null!;

        // ID respondenta (użytkownika głosującego)
        [Required]
        public string RespondentUserId { get; set; } = string.Empty;

        [Display(Name = "Data oddania głosu")]
        public DateTime VotedAt { get; set; } = DateTime.Now;
    }
}