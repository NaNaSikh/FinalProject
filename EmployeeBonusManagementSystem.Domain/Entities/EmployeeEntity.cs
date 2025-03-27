using EmployeeBonusManagementSystem.Domain.Common;

namespace EmployeeBonusManagementSystem.Domain.Entities;

public class EmployeeEntity : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    // [StringLength(11, MinimumLength = 11, ErrorMessage = "Personal Number must be 11 digits.")]
    public string PersonalNumber { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Email { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsPasswordChanged { get; set; }
    public DateTime? PasswordChangeDate { get; set; }
    public int DepartmentId { get; set; }
    public int? RecommenderEmployeeId { get; set; }
    public bool IsActive { get; set; }
    public int CreateByUserId { get; set; }
    public DateTime CreateDate { get; set; }

    //
    public string RefreshToken { get; set; }

    //public LogsEntity Logs { get; set; }
    //public ErrorLogsEntity ErrorLogs { get; set; }

}
