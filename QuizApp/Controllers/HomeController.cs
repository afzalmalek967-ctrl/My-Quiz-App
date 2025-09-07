using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Quiz()
    {
        var questions = await _context.Questions.ToListAsync();
        return View(questions);
    }

    [Authorize(Roles = "Student")]
    [HttpPost]
    public async Task<IActionResult> SubmitQuiz(List<StudentAnswer> answers)
    {
        var allQuestions = await _context.Questions.ToListAsync();
        var totalQuestions = allQuestions.Count;
        var correctAnswers = 0;
        var attemptedQuestions = 0;

        foreach (var answer in answers)
        {
            if (!string.IsNullOrEmpty(answer.SelectedOption))
            {
                attemptedQuestions++;
                var question = allQuestions.FirstOrDefault(q => q.Id == answer.QuestionId);
                if (question != null && question.CorrectAnswer == answer.SelectedOption)
                {
                    correctAnswers++;
                }
            }
        }

        var studentName = User.FindFirst(ClaimTypes.Name)?.Value;
        var newResult = new Result
        {
            StudentName = studentName,
            Score = correctAnswers,
            TotalQuestions = totalQuestions,
            AttemptedQuestions = attemptedQuestions,
            WrongAnswers = attemptedQuestions - correctAnswers,
            DateCompleted = DateTime.Now
        };

        _context.Results.Add(newResult);
        await _context.SaveChangesAsync();

        return RedirectToAction("Results", new { id = newResult.Id });
    }

    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Results(int id)
    {
        var result = await _context.Results.FirstOrDefaultAsync(r => r.Id == id);
        if (result == null)
        {
            return NotFound();
        }
        return View(result);
    }
}

public class StudentAnswer
{
    public int QuestionId { get; set; }
    public string SelectedOption { get; set; }
}