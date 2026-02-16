using System.ComponentModel.DataAnnotations;

namespace AnkietyPPK.Models
{
    public class Vote
    {
        public int Id { get; set; }

        //klucz obcy do opcji, na którą oddano głos
        public int OptionId { get; set; }

        //nawigacja do opcji
        public Option Option { get; set; } = null!;

        //id użytkownika, który oddał głos (z Identity)
        [Required]
        public string RespondentUserId { get; set; } = string.Empty;

        [Display(Name = "Data oddania głosu")]
        public DateTime VotedAt { get; set; } = DateTime.Now;
    }
}