using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Domain.Entities;
using EmployeeBonusManagementSystem.Persistence.Repositories.Implementations;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto.Generators;

namespace EmployeeBonusManagementSystem.Persistence.Services
{
    public class CheckPasswordService: ICheckPasswordService
	{
		private readonly IEmployeeRepository _employeeRepository; 

		public CheckPasswordService(IEmployeeRepository employeeRepository)
		{
			_employeeRepository = employeeRepository;
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

	}
}
