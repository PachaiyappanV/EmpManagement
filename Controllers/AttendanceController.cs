using EmpManagement.Data;
using EmpManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
public class AttendanceViewModel
{
    public List<Employee>? Employees { get; set; }
    public List<Attendance>? TodayAttendance { get; set; }
}


public class AttendanceDTO
{
    public int EmployeeId { get; set; }
    public bool IsPresent { get; set; }
}

public class AttendanceController : Controller
{
    private readonly AppDbContext _context;
    public AttendanceController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        try
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
        catch (Exception)
        {
            TempData["error"] = "Something went wrong while loading data.";
            return View(new AttendanceViewModel());
        }
    }


    [HttpPost]
    public async Task<IActionResult> Mark([FromBody] AttendanceDTO data)
    {
        try
        {
            if (data == null || data.EmployeeId <= 0)
            {
                return BadRequest("Invalid data");
            }

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
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while marking attendance.");
        }
    }

  [HttpGet]
  public async Task<IActionResult> History(int id, int? month, int? year)
    {
        try
        {
            var employee = await _context.Employees
                .Include(e => e.Attendances)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
                return NotFound();

            var attendances = employee.Attendances?.AsQueryable();

            if (month.HasValue && year.HasValue)
            {
                attendances = attendances
                    .Where(a => a.Date.Month == month && a.Date.Year == year)
                    .OrderByDescending(a => a.Date);
            }
            else
            {
                attendances = attendances?.OrderByDescending(a => a.Date);
            }

            ViewBag.SelectedMonth = month;
            ViewBag.SelectedYear = year;
            ViewBag.MonthList = Enumerable.Range(1, 12);
            ViewBag.YearList = Enumerable.Range(DateTime.Now.Year, 6);

            ViewBag.Employee = employee;
            return View(attendances?.ToList());
        }
        catch (Exception)
        {
            TempData["error"] = "Something went wrong while loading data.";
            return View(new List<Attendance>());
        }
    }


}