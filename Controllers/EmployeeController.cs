using EmpManagement.Data;
using EmpManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class EmployeeController : Controller
{
    private readonly AppDbContext _context;
    public EmployeeController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Employees.ToListAsync());

    public IActionResult Create() => View();

    [HttpPost]

   public async Task<IActionResult> Create(Employee employee)
{
    if (!ModelState.IsValid)
        return View(employee);

    try
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    catch (DbUpdateException ex)
    {
        // Log the error (ideally use ILogger, fallback to console if not configured)
        Console.WriteLine("❌ Database update failed: " + ex.Message);

        // Optionally, add a model-level error for user feedback
        ModelState.AddModelError(string.Empty, "Unable to save changes. Please try again later.");

        // Return to view with model state intact
        return View(employee);
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Unexpected error: " + ex.Message);
        ModelState.AddModelError(string.Empty, "An unexpected error occurred.");
        return View(employee);
    }
}



    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        return employee == null ? NotFound() : View(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Employee employee)
    {
        if (!ModelState.IsValid) return View(employee);
        _context.Update(employee);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}