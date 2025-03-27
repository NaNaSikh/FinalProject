﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EmployeeBonusManagement.Application.Services.Interfaces;
using EmployeeBonusManagementSystem.Persistence;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Commands.Login
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
			using var transaction = _unitOfWork.BeginTransaction();  // Start the transaction here

			try
			{
				var response = await _authService.LoginAsync(request.LoginDto, transaction);

				_unitOfWork.Commit();  // Commit the transaction after successful operation
				return response;
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();  // Rollback the transaction on failure
				Console.WriteLine($"Error in LoginCommandHandler: {ex}");
				return null;
			}
		}
	}

}
