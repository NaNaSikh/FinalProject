﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Queries.Login
{
    public class AuthResponse
    {
	    public bool Success { get; set; }
	    public string? AccessToken { get; set; }
	    public string? RefreshToken { get; set; }
	    public string? UserEmail { get; set; }
	    public List<string>? Roles { get; set; }
		public string Message { get; set; }
	}
}
