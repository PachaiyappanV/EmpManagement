using System.ComponentModel.DataAnnotations;

namespace EmpManagement.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Department { get; set; }

    public ICollection<Attendance> Attendances { get; set; }
}