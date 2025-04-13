
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EmployeeBonusManagement.Application.Services.Interfaces;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.RefreshToken;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.Login;
using EmployeeBonusManagementSystem.Domain.Entities;
using EmployeeBonusManagementSystem.Persistence;
using Microsoft.AspNetCore.Identity;


namespace EmployeeBonusManagement.Application.Services
{
	public class AuthService : IAuthService
	{

		private readonly IEmployeeRepository _employeeRepository;
		private readonly IJwtService _jwtService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IAuthService _authService;

		public AuthService(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork , IJwtService jwtService, IAuthService authService)
		{
			_employeeRepository = employeeRepository;
			_unitOfWork = unitOfWork;
			_jwtService = jwtService;
			_authService = authService;
		}

		public async Task<AuthResponse> LoginAsync(LoginDto loginDto)
		{
			using (var transaction = _unitOfWork.BeginTransaction()) 
			{
				try
				{
					var user = await _employeeRepository.GetByEmailAsync(loginDto.Email);

					if (user == null)
					{
						Console.WriteLine("User not found.");
						_unitOfWork.Rollback();
						return new AuthResponse { Success = false };
					}

					if (!await _authService.CheckPasswordAsync(user, loginDto.Password))
					{
						Console.WriteLine("Invalid password.");
						_unitOfWork.Rollback();
						return new AuthResponse { Success = false };
					}
					var roles = await _employeeRepository.GetUserRolesAsync(user.Id);
					var response = await _jwtService.GenerateTokenAsync(user, roles ); 
					_unitOfWork.Commit(); 
					return response;
				}
				catch (Exception ex)
				{
					_unitOfWork.Rollback();
					throw new Exception("Login failed: " + ex.Message);
				}
			}
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


		public async Task<bool> CheckPasswordAsync(EmployeeEntity user, string enteredPassword)
		{
			if (user == null || string.IsNullOrEmpty(user.Password))
			{
				return false;
			}

			var hasher = new PasswordHasher<EmployeeEntity>(); 
			var result = hasher.VerifyHashedPassword(user, user.Password, enteredPassword);
			Console.WriteLine($"{result}");

			if (result == PasswordVerificationResult.SuccessRehashNeeded)
			{

				var newHashedPassword = hasher.HashPassword(user, enteredPassword);
				user.Password = newHashedPassword;

				return true;
			}

			return result == PasswordVerificationResult.Success;
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

