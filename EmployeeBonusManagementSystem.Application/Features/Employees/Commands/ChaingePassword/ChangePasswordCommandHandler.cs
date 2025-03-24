using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagement.Application.Services.Interfaces;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.AddEmployee;
using EmployeeBonusManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Commands.ChaingePassword
{
    internal class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordDto>
    {
	    private readonly IEmployeeRepository _employeeRepository;
	    private readonly IHttpContextAccessor _httpContextAccessor;
	    private readonly IAuthService _authService;


		public ChangePasswordCommandHandler (IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor , IAuthService authService)
		{
			_employeeRepository = employeeRepository;
			_httpContextAccessor = httpContextAccessor;
			_authService = authService;

		}

		public async Task<ChangePasswordDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)

		{

			if (_authService.ValidatePassword(request.newPassword, out string message))
			{


				var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("Id");

				if (userIdClaim == null)
				{
					throw new UnauthorizedAccessException("User ID not found in token.");
				}

				int userId = int.Parse(userIdClaim.Value);

				//ვალიდაციებია დასამატებელი აქ 

				var result = await _employeeRepository.CheckPasswordByIdAsync(userId, request.currentPassword);

				if (result == PasswordVerificationResult.Success)
				{

					var hasher = new PasswordHasher<EmployeeEntity>();
					Console.WriteLine($"request new password {request.newPassword}");
					var newHashedPassword = hasher.HashPassword(null, request.newPassword);
					Console.WriteLine($"request new password hashed  {newHashedPassword}");
					await _employeeRepository.UpdateEmployeePasswordByIdAsync(userId, newHashedPassword);
				}

				Console.WriteLine(result);

				return new ChangePasswordDto(true, "Password changed successfully.");
			}
			else
			{
				return new ChangePasswordDto(false, $"new Password is not valid: {message}");
			}
		}
    }
}
