using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Persistence.Services
{
    public class UserContextService : IUserContextService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserContextService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public int GetUserId()
		{
			var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("Id");

			if (userIdClaim == null)
			{
				throw new UnauthorizedAccessException("User ID not found in token.");
			}

			return int.Parse(userIdClaim.Value);
		}
	}
}
