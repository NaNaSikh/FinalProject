using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Commands.AddEmployee
{
    public class AddEmploeeResponseDto
    {
	   
		    public bool Success { get; set; }
		    public string Message {get; set;}
		    public int? Id { get; set; }
	}
}
