using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Domain.Entities
{
    public class ErrorLogsEntity
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public int UserId { get; set; }
		public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }

	}
}
