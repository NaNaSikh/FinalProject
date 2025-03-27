using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.RefreshToken;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.Login;

namespace EmployeeBonusManagement.Application.Services.Interfaces
{
    public interface IAuthService
    {
        
	    Task<AuthResponse> LoginAsync(LoginDto loginDto, IDbTransaction transaction);
	    bool ValidatePassword(string password, out string errorMessage);


    }
}
