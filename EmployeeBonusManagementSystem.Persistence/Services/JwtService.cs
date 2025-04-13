using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagement.Application.Services.Interfaces;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Employees.Commands.RefreshToken;
using EmployeeBonusManagementSystem.Application.Features.Employees.Queries.Login;
using EmployeeBonusManagementSystem.Domain.Entities;
using EmployeeBonusManagementSystem.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeBonusManagement.Application.Services
{
	public class JwtService : IJwtService
	{
		private readonly IConfiguration _config;
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRefreshTokenRepository _refreshTokenRepository;

		public JwtService(IRefreshTokenRepository refreshTokenRepository, IConfiguration config, IUnitOfWork unitOfWork,
			IEmployeeRepository employeeRepository, IUserContextService userContextService)
		{
			_refreshTokenRepository = refreshTokenRepository;
			_config = config;
			_employeeRepository = employeeRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<AuthResponse> GenerateTokenAsync(EmployeeEntity user, IList<string> roles)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			using var transactionScope = _unitOfWork.BeginTransaction();

			var claims = new List<Claim>
			{
				new Claim("Id", user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email)
			};

			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var token = new JwtSecurityToken(
				issuer: _config["Jwt:Issuer"],
				audience: _config["Jwt:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["Jwt:ExpirationMinutes"])),
				signingCredentials: creds
			);

			try
			{
				var refreshTokenEntity = await _refreshTokenRepository.GetEmployeeRefreshTokenByIdAsync(user.Id);
				var newRefreshTokenValue = GenerateRefreshToken();
				var refreshTokenExpiryDays = Convert.ToInt32(_config["RefreshToken:ExpirationDays"]);

				if (refreshTokenEntity == null)
				{
					Console.WriteLine("No refresh token found - creating a new one.");
					var newRefreshToken = new RefreshTokenEntity
					{
						EmployeeId = user.Id,
						ExpirationDate = DateTime.UtcNow.AddDays(refreshTokenExpiryDays),
						RefreshToken = newRefreshTokenValue
					};
					await _refreshTokenRepository.AddNewRefreshTokenAsync(newRefreshToken);
					refreshTokenEntity = newRefreshToken;
				}
				else if (refreshTokenEntity.ExpirationDate <= DateTime.UtcNow)
				{
					Console.WriteLine("Refresh token expired - updating with a new one.");
					refreshTokenEntity.RefreshToken = newRefreshTokenValue;
					refreshTokenEntity.ExpirationDate = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);
					await _refreshTokenRepository.UpdateRefreshTokenAsync(refreshTokenEntity);
				}
				else
				{
					Console.WriteLine("Existing refresh token is valid.");
				}

				_unitOfWork.Commit();

				return new AuthResponse
				{
					Success = true,
					AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
					RefreshToken = refreshTokenEntity.RefreshToken,
					UserEmail = user.Email,
					Roles = roles.ToList(),
					Message = "Success"
				};
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();
				Console.WriteLine($"Error while generating Token: {ex}");
				return new AuthResponse { Success = false, Message = "Failed to generate authentication token." };
			}
		}

		public string GenerateRefreshToken()
		{
			return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
		}

		public async Task<RefreshTokenResponseDto?> RefreshAccessTokenAsync(string refreshToken)
		{
			var user = await _employeeRepository.GetUserByRefreshTokenAsync(refreshToken);

			if (user == null)
			{
				Console.WriteLine($"Invalid refresh token: {refreshToken}");
				return null;
			}

			var roles = await _employeeRepository.GetUserRolesAsync(user.Id);

			// Generate a new access token
			var newAuthResponse = await GenerateTokenAsync(user, roles);

			if (!newAuthResponse.Success)
			{
				return null; // Or handle the error appropriately
			}

			var newRefreshTokenValue = GenerateRefreshToken();
			var refreshTokenExpiryDays = Convert.ToInt32(_config["RefreshToken:ExpirationDays"]);

			var newRefreshTokenEntity = new RefreshTokenEntity
			{
				EmployeeId = user.Id,
				ExpirationDate = DateTime.UtcNow.AddDays(refreshTokenExpiryDays),
				RefreshToken = newRefreshTokenValue
			};

			using var transactionScope = _unitOfWork.BeginTransaction();
			try
			{
				await _refreshTokenRepository.UpdateRefreshTokenAsync(newRefreshTokenEntity);
				_unitOfWork.Commit();

				return new RefreshTokenResponseDto
				{
					AccessToken = newAuthResponse.AccessToken,
					RefreshToken = newRefreshTokenEntity.RefreshToken
				};
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();
				Console.WriteLine($"Error while refreshing access token: {ex}");
				return null; // Or handle the error appropriately
			}
		}
	}
}
