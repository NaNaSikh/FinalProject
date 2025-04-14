using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto.Generators;

namespace EmployeeBonusManagementSystem.Persistence.Services
{
    public class CheckPasswordService: ICheckPasswordService
	{
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
	}
}
