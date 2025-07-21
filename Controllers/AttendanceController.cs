using EmpManagement.Data;
using EmpManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
public class AttendanceViewModel
{
    public List<Employee> Employees { get; set; }
    public List<Attendance> TodayAttendance { get; set; }
}


public class AttendanceDTO
{
    public int EmployeeId { get; set; }
    public bool IsPresent { get; set; }
}

public class AttendanceController : Controller
{
    private readonly AppDbContext _context;
    public AttendanceController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;

        var employees = await _context.Employees.ToListAsync();
        var todayAttendance = await _context.Attendances
            .Where(a => a.Date == today)
            .ToListAsync();

        var viewModel = new AttendanceViewModel
        {
            Employees = employees,
            TodayAttendance = todayAttendance
        };
        return View(viewModel);
    }

    public IActionResult Mark() =>
        View(new Attendance { Date = DateTime.Today });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Mark([FromBody] AttendanceDTO data)
    {
        if (data == null || data.EmployeeId <= 0)
            return BadRequest("Invalid data");

        var today = DateTime.Today;

        // Check if already marked for today
        var existing = await _context.Attendances
            .FirstOrDefaultAsync(a => a.EmployeeId == data.EmployeeId && a.Date == today);

        if (existing != null)
        {
            existing.IsPresent = data.IsPresent;
            _context.Attendances.Update(existing);
        }
        else
        {
            var attendance = new Attendance
            {
                EmployeeId = data.EmployeeId,
                IsPresent = data.IsPresent,
                Date = today
            };
            await _context.Attendances.AddAsync(attendance);
        }

        await _context.SaveChangesAsync();
        return Content("Attendance marked successfully.");
    }
}