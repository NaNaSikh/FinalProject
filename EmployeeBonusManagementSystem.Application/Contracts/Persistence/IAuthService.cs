using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.Login;
using EmployeeBonusManagementSystem.Application.Features.Employees.Common;

namespace EmployeeBonusManagement.Application.Services.Interfaces
{
    public interface IAuthService
    {
        
	    Task<AuthResponse> LoginAsync(LoginDto loginDto);
	    bool ValidatePassword(string password, out string errorMessage);

    }
}
