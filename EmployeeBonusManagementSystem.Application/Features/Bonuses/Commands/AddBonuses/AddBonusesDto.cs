namespace EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.AddBonuses;

public class AddBonusesDto
{
    public int EmployeeId { get; set; }
    public decimal Amount { get; set; }
    public bool IsRecommenderBonus { get; set; }
    public int RecommendationLevel { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public int CreateByUserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PersonalNumber { get; set; } = string.Empty;
}