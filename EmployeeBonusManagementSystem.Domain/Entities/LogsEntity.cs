using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Domain.Common;

namespace EmployeeBonusManagementSystem.Domain.Entities
{
    public class LogsEntity 
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public int UserId { get; set; }
        public string ActionType { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }

	}
}
