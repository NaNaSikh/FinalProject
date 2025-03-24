using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EmployeeBonusManagement.Application.Services.Interfaces;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.Login;
using EmployeeBonusManagementSystem.Application.Features.Employees.Common;
using EmployeeBonusManagementSystem.Domain.Entities;
using EmployeeBonusManagementSystem.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeBonusManagement.Application.Services
{
	public class AuthService : IAuthService
	{

		private readonly IEmployeeRepository _employeeRepository;
		private readonly IJwtService _jwtService;
		private readonly IUnitOfWork _unitOfWork;

		public AuthService(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork , IJwtService jwtService)
		{
			_employeeRepository = employeeRepository;

			_unitOfWork = unitOfWork;

			_jwtService = jwtService;
		}

		public async Task<AuthResponse> LoginAsync(LoginDto loginDto)
		{

			_unitOfWork.BeginTransaction(); // Ensure a transaction is started

			var user = await _employeeRepository.GetByEmailAsync(loginDto.Email.ToLower());

			if (user == null)
			{
				Console.WriteLine("User not found.");
				_unitOfWork.Rollback(); // Rollback if user is not found
				return new AuthResponse { Success = false };
			}

			if (!await _employeeRepository.CheckPasswordAsync(user, loginDto.Password))
			{
				Console.WriteLine("Invalid password.");
				_unitOfWork.Rollback();
				return new AuthResponse { Success = false };
			}

			var roles = await _employeeRepository.GetUserRolesAsync(user.Id);
			var response = _jwtService.GenerateToken(user, roles);

			_unitOfWork.Commit(); // Commit transaction if successful
			return response;
		}

		public  bool ValidatePassword(string password, out string errorMessage)
		{
			errorMessage = string.Empty;

			if (string.IsNullOrWhiteSpace(password))
			{
				errorMessage = "Password cannot be empty.";
				return false;
			}

			if (password.Length < 8)
			{
				errorMessage = "Password must be at least 8 characters long.";
				return false;
			}

			if (!Regex.IsMatch(password, @"[A-Za-z]"))
			{
				errorMessage = "Password must contain at least one letter.";
				return false;
			}

			if (!Regex.IsMatch(password, @"\d"))
			{
				errorMessage = "Password must contain at least one number.";
				return false;
			}

			return true;
		}
	}
}

