using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Application.Features.Employees.Common;
using EmployeeBonusManagementSystem.Domain.Entities;


namespace EmployeeBonusManagement.Application.Services.Interfaces
{
    public interface IJwtService
    {
	    AuthResponse GenerateToken(EmployeeEntity user, IList<string> roles);
	    string GenerateRefreshToken();
    }
}
