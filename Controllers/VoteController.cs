using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnkietyPPK.Data;
using AnkietyPPK.Models;
using AnkietyPPK.Models.ViewModels;

namespace AnkietyPPK.Controllers
{
    [Authorize]
    public class VoteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public VoteController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Vote/Cast/5 — formularz głosowania
        public async Task<IActionResult> Cast(int id)
        {
            var survey = await _context.Surveys
                .Include(s => s.Options)
                    .ThenInclude(o => o.Votes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (survey == null)
                return NotFound();

            if (!survey.IsActive)
            {
                TempData["Error"] = "Ta ankieta jest już zamknięta.";
                return RedirectToAction("Index", "Survey");
            }

            var currentUserId = _userManager.GetUserId(User);

            // Sprawdzenie, czy użytkownik już głosował w tej ankiecie
            var hasVoted = survey.Options
                .Any(o => o.Votes.Any(v => v.RespondentUserId == currentUserId));

            if (hasVoted)
            {
                TempData["Error"] = "Już oddałeś głos w tej ankiecie.";
                return RedirectToAction("Results", "Survey", new { id });
            }

            var viewModel = new VoteViewModel
            {
                SurveyId = survey.Id,
                Title = survey.Title,
                Description = survey.Description,
                Options = survey.Options.ToList()
            };

            return View(viewModel);
        }

        // POST: Vote/Cast — oddanie głosu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cast(VoteViewModel model)
        {
            if (model.SelectedOptionId == null)
            {
                ModelState.AddModelError("SelectedOptionId", "Musisz wybrać jedną opcję.");
            }

            var survey = await _context.Surveys
                .Include(s => s.Options)
                    .ThenInclude(o => o.Votes)
                .FirstOrDefaultAsync(s => s.Id == model.SurveyId);

            if (survey == null)
                return NotFound();

            if (!survey.IsActive)
            {
                TempData["Error"] = "Ta ankieta jest już zamknięta.";
                return RedirectToAction("Index", "Survey");
            }

            var currentUserId = _userManager.GetUserId(User)!;

            // Ponowna walidacja — czy użytkownik już głosował
            var hasVoted = survey.Options
                .Any(o => o.Votes.Any(v => v.RespondentUserId == currentUserId));

            if (hasVoted)
            {
                TempData["Error"] = "Już oddałeś głos w tej ankiecie.";
                return RedirectToAction("Results", "Survey", new { id = model.SurveyId });
            }

            if (!ModelState.IsValid)
            {
                model.Title = survey.Title;
                model.Description = survey.Description;
                model.Options = survey.Options.ToList();
                return View(model);
            }

            // Sprawdzenie, czy wybrana opcja należy do tej ankiety
            var selectedOption = survey.Options.FirstOrDefault(o => o.Id == model.SelectedOptionId);
            if (selectedOption == null)
            {
                TempData["Error"] = "Nieprawidłowa opcja.";
                return RedirectToAction("Cast", new { id = model.SurveyId });
            }

            var vote = new Vote
            {
                OptionId = selectedOption.Id,
                RespondentUserId = currentUserId,
                VotedAt = DateTime.Now
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Dziękujemy za oddanie głosu!";
            return RedirectToAction("Results", "Survey", new { id = model.SurveyId });
        }
    }
}