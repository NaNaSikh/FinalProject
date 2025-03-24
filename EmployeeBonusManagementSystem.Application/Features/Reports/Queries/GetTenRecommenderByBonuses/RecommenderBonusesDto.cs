namespace EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenRecommenderByBonuses;

public class RecommenderBonusesDto
{
    public required string RecommenderName { get; set; }
    public int TotalRecommendedBonuses { get; set; }
    public decimal TotalBonusAmount { get; set; }
}
