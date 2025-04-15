using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.Login;
using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EmployeeBonusManagement.Application.Services.Interfaces
{
    public interface IAuthService
    {
	    Task<AuthResponse> LoginAsync(LoginDto loginDto);
    }
}
