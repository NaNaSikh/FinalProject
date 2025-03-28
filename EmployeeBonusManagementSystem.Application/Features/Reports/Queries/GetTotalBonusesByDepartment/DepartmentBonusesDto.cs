namespace EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalBonusesByDepartment;

public class DepartmentBonusesDto
{
    public required string Name { get; set; }
    public decimal TotalBonuses { get; set; }
}
