using EmployeeBonusManagementSystem.Domain.Common;

namespace EmployeeBonusManagementSystem.Domain.Entities;

public class EmployeeDepartmentEntity : BaseEntity
{
    public int EmployeeId { get; set; }
    public int DepartmentId { get; set; }
    public DateTime AssignDate { get; set; }
    public bool IsActive { get; set; }

}
