using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EmployeeBonusManagementSystem.Application.Contracts.Persistence
{
    public interface ICheckPasswordService
    {
	    Task<bool> CheckPasswordAsync(EmployeeEntity user, string password);
		Task<PasswordVerificationResult> CheckPasswordByIdAsync(int id, string enteredPassword);
		bool ValidatePassword(string password, out string errorMessage);


	}
}
