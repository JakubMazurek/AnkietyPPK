using System.ComponentModel.DataAnnotations;

namespace AnkietyPPK.Models.ViewModels
{
    // ViewModel do tworzenia nowej ankiety
    public class CreateSurveyViewModel
    {
        [Required(ErrorMessage = "Tytuł ankiety jest wymagany.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Tytuł musi mieć od 3 do 200 znaków.")]
        [Display(Name = "Tytuł ankiety")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Opis")]
        [StringLength(1000)]
        public string? Description { get; set; }

        [Display(Name = "Opcje odpowiedzi")]
        public List<string> Options { get; set; } = new List<string> { "", "" };
    }

    // ViewModel do wyświetlania wyników ankiety
    public class SurveyResultsViewModel
    {
        public int SurveyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalVotes { get; set; }
        public bool HasVoted { get; set; }
        public List<OptionResultViewModel> Options { get; set; } = new();
    }

    public class OptionResultViewModel
    {
        public int OptionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public int VoteCount { get; set; }
        public double Percentage { get; set; }
    }

    // ViewModel do głosowania
    public class VoteViewModel
    {
        public int SurveyId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        [Required(ErrorMessage = "Musisz wybrać jedną opcję.")]
        [Display(Name = "Twój wybór")]
        public int? SelectedOptionId { get; set; }

        public List<Option> Options { get; set; } = new();
    }

    // ViewModel do listy ankiet
    public class SurveyListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public int OptionCount { get; set; }
        public int TotalVotes { get; set; }
        public bool HasVoted { get; set; }
    }

    // ViewModel do logowania
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format email.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }

    // ViewModel do rejestracji
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format email.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Hasło musi mieć co najmniej 6 znaków.")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane.")]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasła nie są identyczne.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}