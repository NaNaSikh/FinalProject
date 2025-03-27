using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Commands.RefreshToken
{
    public class RefreshTokenResponseDto
    {
	    public string AccessToken { get; set; }
	    public string RefreshToken { get; set; }
	}
}
