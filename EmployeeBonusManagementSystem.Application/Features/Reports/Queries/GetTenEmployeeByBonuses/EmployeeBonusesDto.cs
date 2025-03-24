namespace EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenEmployeeByBonuses;

public class EmployeeBonusesDto
{
    public required string EmployeeFullName { get; set; }
    public decimal TotalBonusAmount { get; set; }
}
