using System.ComponentModel.DataAnnotations;

namespace AnkietyPPK.Models
{
    public class Option
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Treść opcji jest wymagana.")]
        [StringLength(300, MinimumLength = 1, ErrorMessage = "Opcja musi mieć od 1 do 300 znaków.")]
        [Display(Name = "Treść opcji")]
        public string Text { get; set; } = string.Empty;

        // Klucz obcy do ankiety
        public int SurveyId { get; set; }

        // Nawigacja do ankiety nadrzędnej
        public Survey Survey { get; set; } = null!;

        // Relacja 1:N — jedna opcja ma wiele głosów
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}