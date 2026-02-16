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
    public class SurveyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SurveyController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Survey — lista wszystkich ankiet
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(User);

            var surveys = await _context.Surveys
                .Include(s => s.Options)
                    .ThenInclude(o => o.Votes)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            var viewModel = surveys.Select(s => new SurveyListItemViewModel
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
                IsActive = s.IsActive,
                OptionCount = s.Options.Count,
                TotalVotes = s.Options.Sum(o => o.Votes.Count),
                HasVoted = s.Options.Any(o => o.Votes.Any(v => v.RespondentUserId == currentUserId))
            }).ToList();

            return View(viewModel);
        }

        // GET: Survey/Create — formularz tworzenia ankiety (tylko Ankieter)
        [Authorize(Roles = "Ankieter")]
        public IActionResult Create()
        {
            var model = new CreateSurveyViewModel
            {
                Options = new List<string> { "", "" } // minimum 2 opcje
            };
            return View(model);
        }

        // POST: Survey/Create — zapis nowej ankiety
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Ankieter")]
        public async Task<IActionResult> Create(CreateSurveyViewModel model)
        {
            // Usunięcie pustych opcji
            model.Options = model.Options
                .Where(o => !string.IsNullOrWhiteSpace(o))
                .ToList();

            if (model.Options.Count < 2)
            {
                ModelState.AddModelError("Options", "Ankieta musi mieć co najmniej 2 opcje.");
            }

            if (!ModelState.IsValid)
            {
                if (model.Options.Count < 2)
                    model.Options = new List<string> { "", "" };
                return View(model);
            }

            var survey = new Survey
            {
                Title = model.Title,
                Description = model.Description,
                CreatedByUserId = _userManager.GetUserId(User)!,
                CreatedAt = DateTime.Now,
                IsActive = true,
                Options = model.Options.Select(text => new Option { Text = text }).ToList()
            };

            _context.Surveys.Add(survey);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Ankieta została utworzona pomyślnie!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Survey/Results/5 — wyniki ankiety (wykres)
        public async Task<IActionResult> Results(int id)
        {
            var survey = await _context.Surveys
                .Include(s => s.Options)
                    .ThenInclude(o => o.Votes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (survey == null)
                return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            var totalVotes = survey.Options.Sum(o => o.Votes.Count);

            var viewModel = new SurveyResultsViewModel
            {
                SurveyId = survey.Id,
                Title = survey.Title,
                Description = survey.Description,
                TotalVotes = totalVotes,
                HasVoted = survey.Options.Any(o => o.Votes.Any(v => v.RespondentUserId == currentUserId)),
                Options = survey.Options.Select(o => new OptionResultViewModel
                {
                    OptionId = o.Id,
                    Text = o.Text,
                    VoteCount = o.Votes.Count,
                    Percentage = totalVotes > 0 ? Math.Round((double)o.Votes.Count / totalVotes * 100, 1) : 0
                }).ToList()
            };

            return View(viewModel);
        }

        // GET: Survey/Delete/5 — usuwanie ankiety (tylko Ankieter)
        [Authorize(Roles = "Ankieter")]
        public async Task<IActionResult> Delete(int id)
        {
            var survey = await _context.Surveys
                .Include(s => s.Options)
                    .ThenInclude(o => o.Votes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (survey == null)
                return NotFound();

            return View(survey);
        }

        // POST: Survey/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Ankieter")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var survey = await _context.Surveys.FindAsync(id);
            if (survey != null)
            {
                _context.Surveys.Remove(survey);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Ankieta została usunięta.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Survey/Votes/5 — lista głosów (tylko Ankieter)
        [Authorize(Roles = "Ankieter")]
        public async Task<IActionResult> Votes(int id)
        {
            var survey = await _context.Surveys
                .Include(s => s.Options)
                    .ThenInclude(o => o.Votes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (survey == null)
                return NotFound();

            return View(survey);
        }
    }
}