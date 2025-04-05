using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Domain.Common;

namespace EmployeeBonusManagementSystem.Domain.Entities
{
    public class RefreshTokenEntity :BaseEntity
    {
        public int EmployeeId { get; set;  }
        public string RefreshToken { get; set;}
        public DateTime ExpirationDate { get; set; }
    }
}
