using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Commands.ChaingePassword
{
    public class ChangePasswordDto
    {
	    public bool Success { get; set; }
	    public string Message { get; set; }

	    public ChangePasswordDto(bool success, string message)
	    {
		    Success = success;
		    Message = message;
	    }
	}
}
