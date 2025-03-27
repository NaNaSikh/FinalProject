using EmployeeBonusManagement.Application.Services.Interfaces;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.Login;
using EmployeeBonusManagementSystem.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Commands.RefreshToken
{
	internal class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponseDto>
	{
		private readonly IJwtService _jwtService;
		private readonly IUnitOfWork _unitOfWork;

		public RefreshTokenCommandHandler(IJwtService jwtService, IUnitOfWork unitOfWork)
		{
			_jwtService = jwtService;
			_unitOfWork = unitOfWork;
		}

		public async Task<RefreshTokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
		{
			using var transaction = _unitOfWork.BeginTransaction();  // Start the transaction here

			try
			{
				var response = await _jwtService.RefreshAccessTokenAsync(request.refreshToken, transaction);

				_unitOfWork.Commit();  // Commit the transaction after successful operation
				return response;
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();  // Rollback the transaction on failure
				Console.WriteLine($"Error in RefreshTokenCommandHandler: {ex}");
				return null;
			}
		}
	}

}
