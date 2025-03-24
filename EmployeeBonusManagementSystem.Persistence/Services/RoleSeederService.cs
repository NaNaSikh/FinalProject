using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Domain.Entities;
using EmployeeBonusManagementSystem.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeBonusManagement.Application.Services
{
	public class RoleSeederService
	{
		public async Task SeedRolesAsync(ApplicationDbContext context)
		{
			var roleNames = Enum.GetValues(typeof(Role))
				.Cast<Role>()
				.Select(r => r.ToString())
				.ToArray();

			var existingRoles = await context.Roles
				.Select(r => r.RoleName)
				.ToListAsync();

			var newRoles = roleNames.Except(existingRoles) // Get roles that are not in DB
				.Select(roleName => new RolesEntity { RoleName = roleName });

			if (newRoles.Any())
			{
				await context.Roles.AddRangeAsync(newRoles);
				await context.SaveChangesAsync();
			}
		}
	}

}

