using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.RefreshToken;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.Login;
using EmployeeBonusManagementSystem.Domain.Entities;


namespace EmployeeBonusManagement.Application.Services.Interfaces
{
    public interface IJwtService
    {
	    Task<AuthResponse> GenerateTokenAsync(EmployeeEntity user, IList<string> roles, IDbTransaction transaction);
	    RefreshTokenEntity GenerateRefreshToken(int Id);
	    Task<RefreshTokenResponseDto> RefreshAccessTokenAsync(string refreshToken , IDbTransaction transaction);

	}
}
