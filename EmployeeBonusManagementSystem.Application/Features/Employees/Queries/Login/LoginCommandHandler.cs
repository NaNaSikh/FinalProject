﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EmployeeBonusManagement.Application.Services.Interfaces;
using EmployeeBonusManagementSystem.Persistence;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Queries.Login
{
	internal class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
	{
		private readonly IAuthService _authService;
		private readonly IUnitOfWork _unitOfWork;

		public LoginCommandHandler(IAuthService authService, IUnitOfWork unitOfWork)
		{
			_authService = authService;
			_unitOfWork = unitOfWork;
		}

		public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
		{

			var response = await _authService.LoginAsync(request.LoginDto);
			return response;
			
		}
	}

}
