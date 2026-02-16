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

        //klucz obcy do ankiety, do której należy ta opcja
        public int SurveyId { get; set; }

        //nawigacja do ankiety
        public Survey Survey { get; set; } = null!;

        //relacja 1:N do głosów oddanych na tę opcję
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}