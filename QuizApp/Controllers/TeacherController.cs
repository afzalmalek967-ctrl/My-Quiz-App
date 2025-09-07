using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Teacher")]
public class TeacherController : Controller
{
    private readonly ApplicationDbContext _context;

    public TeacherController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    // --- Student Management Actions ---
    public async Task<IActionResult> ManageStudents()
    {
        var students = await _context.Users.Where(u => u.Role == "Student").ToListAsync();
        return View(students);
    }

    [HttpGet]
    public IActionResult AddStudent()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddStudent(User user)
    {
        user.Role = "Student";
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(ManageStudents));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(ManageStudents));
    }

    // --- Quiz Management Actions ---
    public async Task<IActionResult> ManageQuizzes()
    {
        var questions = await _context.Questions.ToListAsync();
        return View(questions);
    }

    [HttpGet]
    public IActionResult AddQuestion()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddQuestion(Question question)
    {
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(ManageQuizzes));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        var question = await _context.Questions.FindAsync(id);
        if (question != null)
        {
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(ManageQuizzes));
    }

    // --- Student Results Actions ---
    public async Task<IActionResult> StudentResults()
    {
        var results = await _context.Results.ToListAsync();
        return View(results);
    }
}