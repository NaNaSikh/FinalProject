using EmployeeBonusManagementSystem.Domain.Common;

namespace EmployeeBonusManagementSystem.Domain.Entities;

public class RecommenderEmployeeEntity : BaseEntity
{
    public int EmployeeId { get; set; }
    public int RecommenderEmployeeId { get; set; }
    public DateTime AssignDate { get; set; }

}
