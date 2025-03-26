using EmployeeBonusManagementSystem.Domain.Common;

namespace EmployeeBonusManagementSystem.Domain.Entities;

public class BonusConfigurationEntity : BaseEntity
{
    public decimal MaxBonusAmount { get; set; }
    public double MaxBonusPercentage { get; set; }
    public double MinBonusPercentage { get; set; }

    //public double ActualBonusPercent { get; set; }
    public int MaxRecommendationLevel { get; set; }
    public double RecommendationBonusRate { get; set; }
    public int CreateByUserId { get; set; }

}
