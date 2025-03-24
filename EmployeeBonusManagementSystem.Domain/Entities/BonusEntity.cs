using EmployeeBonusManagementSystem.Domain.Common;
using System;

namespace EmployeeBonusManagementSystem.Domain.Entities;

public class BonusEntity : BaseEntity
{
    public int EmployeeId { get; set; }
    public decimal Amount { get; set; }
    public bool IsRecommenderBonus { get; set; }
    public int RecommendationLevel { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public int CreateByUserId { get; set; }
    public Guid TransactionId { get; set; }

}
