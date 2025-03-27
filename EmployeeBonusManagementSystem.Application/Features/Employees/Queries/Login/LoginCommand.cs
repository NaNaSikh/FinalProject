using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Queries.Login
{
    public record  LoginCommand : IRequest<AuthResponse>
	{
		public LoginDto LoginDto { get; }

	    public LoginCommand(LoginDto loginDto)
	    {
			LoginDto = loginDto;
	    }
	}
}
