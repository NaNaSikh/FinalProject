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
		private readonly IRefreshTokenRepository _refreshTokenRepository;

		public JwtService(IRefreshTokenRepository refreshTokenRepository, IConfiguration config,
			IEmployeeRepository employeeRepository)
		{
			_refreshTokenRepository = refreshTokenRepository;
			_config = config;
			_employeeRepository = employeeRepository;
		}

		public async Task<AuthResponse> GenerateTokenAsync(EmployeeEntity user, IList<string> roles)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

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

			
			string refreshTokenValue = GenerateRefreshToken();

			return new AuthResponse
			{
				Success = true,
				AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
				RefreshToken = refreshTokenValue, 
				UserEmail = user.Email,
				Roles = roles.ToList(),
				Message = "Success"
			};
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

			var newAuthResponse = await GenerateTokenAsync(user, roles);

			if (!newAuthResponse.Success)
			{
				return new RefreshTokenResponseDto
				{
					Message = "error Generating new Token "
				};
			}

			var newRefreshTokenValue = GenerateRefreshToken();
			var refreshTokenExpiryDays = Convert.ToInt32(_config["RefreshToken:ExpirationDays"]);

			var newRefreshTokenEntity = new RefreshTokenEntity
			{
				EmployeeId = user.Id,
				ExpirationDate = DateTime.UtcNow.AddDays(refreshTokenExpiryDays),
				RefreshToken = newRefreshTokenValue
			};

			await _refreshTokenRepository.UpdateRefreshTokenAsync(newRefreshTokenEntity);
				
			return new RefreshTokenResponseDto
			{
				AccessToken = newAuthResponse.AccessToken,
				RefreshToken = newRefreshTokenEntity.RefreshToken
			};
			
		}
	}
}
