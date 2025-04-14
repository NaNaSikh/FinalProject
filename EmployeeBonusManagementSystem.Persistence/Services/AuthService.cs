
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EmployeeBonusManagement.Application.Services.Interfaces;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.RefreshToken;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.Login;
using EmployeeBonusManagementSystem.Domain.Entities;
using EmployeeBonusManagementSystem.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace EmployeeBonusManagement.Application.Services
{
	public class AuthService : IAuthService
	{

		private readonly IEmployeeRepository _employeeRepository;
		private readonly IJwtService _jwtService;
		private readonly ILogger<AuthService> _logger;
		private readonly ICheckPasswordService _checkPasswordService;

		public AuthService(IEmployeeRepository employeeRepository , IJwtService jwtService, ILogger<AuthService> logger ,ICheckPasswordService checkPasswordService)
		{
			_employeeRepository = employeeRepository;
			_jwtService = jwtService;
			_logger = logger;
			_checkPasswordService = checkPasswordService;
		}

		public async Task<AuthResponse> LoginAsync(LoginDto loginDto)
		{
			var user = await _employeeRepository.GetByEmailAsync(loginDto.Email);

			if (user == null)
			{
				_logger?.LogInformation($"Login failed for user '{loginDto.Email}': User not found.");
				return new AuthResponse { Success = false, Message = "Invalid credentials. Email is not correct " };
			}

			if (!await _checkPasswordService.CheckPasswordAsync(user, loginDto.Password))
			{
				_logger?.LogInformation($"Login failed for user '{loginDto.Email}': Invalid password.");
				return new AuthResponse { Success = false, Message = "Invalid credentials. Password is not correct" };
			}

			var roles = await _employeeRepository.GetUserRolesAsync(user.Id);
			var response = await _jwtService.GenerateTokenAsync(user, roles);

			return response;
		}

		public bool ValidatePassword(string password, out string errorMessage)
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




		public async Task<PasswordVerificationResult> CheckPasswordByIdAsync(int id, string enteredPassword)
		{
			var userPassword = await _employeeRepository.GetEmployeePasswordByIdAsync(id);

			if (string.IsNullOrEmpty(userPassword))
			{
				return PasswordVerificationResult.Failed;
			}

			var hasher = new PasswordHasher<EmployeeEntity>();
			return hasher.VerifyHashedPassword(null, userPassword, enteredPassword);
		}

	}
}

