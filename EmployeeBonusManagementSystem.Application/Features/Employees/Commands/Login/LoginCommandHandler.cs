using EmployeeBonusManagementSystem.Application.Features.Employees.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EmployeeBonusManagement.Application.Services.Interfaces;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Commands.Login
{
    internal class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
	{
		private readonly IAuthService _authService;

		public LoginCommandHandler(IAuthService authService)
		{
			_authService = authService;
		}

		public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
		{
			return await _authService.LoginAsync(request.LoginDto);
		}
	}
}
