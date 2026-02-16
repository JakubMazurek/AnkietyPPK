using System.ComponentModel.DataAnnotations;

namespace AnkietyPPK.Models
{
    public class Survey
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł ankiety jest wymagany.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Tytuł musi mieć od 3 do 200 znaków.")]
        [Display(Name = "Tytuł ankiety")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Opis")]
        [StringLength(1000, ErrorMessage = "Opis może mieć maksymalnie 1000 znaków.")]
        public string? Description { get; set; }

        [Display(Name = "Data utworzenia")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Czy aktywna")]
        public bool IsActive { get; set; } = true;

        //id użytkownika, który utworzył ankietę (z Identity)
        [Required]
        public string CreatedByUserId { get; set; } = string.Empty;

        //relacja 1:N do opcji w ankiecie
        public ICollection<Option> Options { get; set; } = new List<Option>();
    }
}