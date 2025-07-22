using System.ComponentModel.DataAnnotations;

namespace EmpManagement.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Department { get; set; }
    [Required]
    public string? Country { get; set; }

    [Required]
    public string? State { get; set; }




    public ICollection<Attendance>? Attendances { get; set; }
}