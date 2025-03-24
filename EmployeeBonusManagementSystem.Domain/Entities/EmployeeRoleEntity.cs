using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Domain.Common;

namespace EmployeeBonusManagementSystem.Domain.Entities
{
    public class EmployeeRoleEntity : BaseEntity
    {
         public int EmployeeId { get; set; }
         public int RoleId { get; set; }
    }
}
