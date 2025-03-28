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

	    public JwtService(IConfiguration config, IUnitOfWork unitOfWork,IEmployeeRepository employeeRepository , IUserContextService userContextService)
	    {
		    _config = config;
		    _employeeRepository = employeeRepository;
		    _unitOfWork = unitOfWork;
	    }

		public async Task<AuthResponse> GenerateTokenAsync(EmployeeEntity user, IList<string> roles, IDbTransaction transaction = null)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			using var transactionScope = transaction ?? _unitOfWork.BeginTransaction();  

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
				expires: DateTime.UtcNow.AddHours(Convert.ToInt32(_config["Jwt:ExpirationHours"])),
				signingCredentials: creds
			);

			try
			{
				string refreshToken = GenerateRefreshToken();

				await _employeeRepository.UpdateRefreshTokenAsync(user.Id, refreshToken);  // Pass the transaction here

				_unitOfWork.Commit();  // Commit the transaction if everything goes well

				return new AuthResponse
				{
					Success = true,
					AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
					RefreshToken = refreshToken,
					Expiration = token.ValidTo,
					UserEmail = user.Email,
					Roles = roles.ToList()
				};
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();
				Console.WriteLine($"Error while generating Token {ex}");
				return null;
			}
		}

		public string GenerateRefreshToken()
	    {
		    return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
	    }

		public async Task<RefreshTokenResponseDto> RefreshAccessTokenAsync(string refreshToken, IDbTransaction transaction = null)
		{
			var user = await _employeeRepository.GetUserByRefreshTokenAsync(refreshToken);

			if (user == null)
			{
				return null; // User not found, handle it as needed
			}

			var roles = await _employeeRepository.GetUserRolesAsync(user.Id);

			// Generate new tokens, passing the transaction
			var newAccessToken = await GenerateTokenAsync(user, roles, transaction);
			var newRefreshToken = GenerateRefreshToken();

			// Update the refresh token in the database
			await _employeeRepository.UpdateRefreshTokenAsync(user.Id, newRefreshToken);

			return new RefreshTokenResponseDto
			{
				AccessToken = newAccessToken.AccessToken,
				RefreshToken = newRefreshToken
			};
		}

	}
}
