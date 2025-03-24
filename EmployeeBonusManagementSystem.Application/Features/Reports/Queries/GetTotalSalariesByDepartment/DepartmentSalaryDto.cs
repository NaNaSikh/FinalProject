namespace EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalSalariesByDepartment;

public class DepartmentSalaryDto
{
    public required string DepartmentName { get; set; }
    public decimal TotalSalaries { get; set; }
}
