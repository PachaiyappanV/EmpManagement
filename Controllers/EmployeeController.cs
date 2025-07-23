using EmpManagement.Data;
using EmpManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class EmployeeController : Controller
{
    private readonly AppDbContext _context;
    public EmployeeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees);
        }
        catch (Exception)
        {
            TempData["error"] = "Something went wrong while loading data.";
            return View(new List<Employee>());
        }
        
    }

    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([Bind("Name,Email,Department,Country,State")]Employee employee)
    {
        if (!ModelState.IsValid){
            return View(employee);
        }

        try
            {
                employee.DateJoined = DateOnly.FromDateTime(DateTime.Now);
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                TempData["success"] = "Employee created successfully!";
                return RedirectToAction("Index");
            }
        catch (DbUpdateException)
            {

                TempData["error"] = "Unable to save changes. Please try again later.";
                return View(employee);
            }
        catch (Exception)
            {
                TempData["error"] = "An unexpected error occurred while creating the employee.";
                return View(employee);
            }
    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        catch (Exception)
        {
            TempData["error"] = "An unexpected error occurred while retrieving employee data.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Employee employee)
    {
        if (!ModelState.IsValid)
        {
            return View(employee);
        }

        try
        {
            _context.Update(employee);
            await _context.SaveChangesAsync();
            TempData["success"] = "Employee updated successfully!";
        }
        catch (DbUpdateException)
        {
            TempData["error"] = "Unable to save changes. Please try again later.";
            return View(employee);
        }
        catch (Exception)
        {
            TempData["error"] = "An unexpected error occurred while updating the employee.";
            return View(employee);
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {

        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        try
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            TempData["success"] = "Employee deleted successfully!";
        }
        catch (Exception)
        {
            TempData["error"] = "An unexpected error occurred while deleting the employee.";
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }

  
}