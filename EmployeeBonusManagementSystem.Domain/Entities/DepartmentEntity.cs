using EmployeeBonusManagementSystem.Domain.Common;

namespace EmployeeBonusManagementSystem.Domain.Entities;

public class DepartmentEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public bool IsActive { get; set; }
    public int CreateByUserId { get; set; }

}
