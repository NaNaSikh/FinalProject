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

	    public JwtService(IRefreshTokenRepository refreshTokenRepository,IConfiguration config, IUnitOfWork unitOfWork,IEmployeeRepository employeeRepository , IUserContextService userContextService)
	    {
		    _refreshTokenRepository = refreshTokenRepository;
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
				expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["Jwt:ExpirationMinutes"])),
				signingCredentials: creds
			);

			try
			{
				var refreshToken = await _refreshTokenRepository.GetEmployeeRefreshTokenByIdAsync(user.Id);

				var newRefreshToken = GenerateRefreshToken(user.Id);

				if (refreshToken == null)
				{
					Console.WriteLine("No refresh token found — creating a new one.");
					await _refreshTokenRepository.AddNewRefreshTokenAsync(newRefreshToken);
					refreshToken = newRefreshToken;

				}
				else if (refreshToken.ExpirationDate <= DateTime.Now)
				{

					await _refreshTokenRepository.UpdateRefreshTokenAsync(newRefreshToken);
					refreshToken = newRefreshToken;
				}
				_unitOfWork.Commit();

				return new AuthResponse
				{
					Success = true,
					AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
					RefreshToken = refreshToken.RefreshToken,
					UserEmail = user.Email,
					Roles = roles.ToList()
				};
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();
				Console.WriteLine($"Error while generating Token {ex}");
				//TODO fix this 
				return null;
			}
		}

		public RefreshTokenEntity GenerateRefreshToken(int Id)
		{
			var refreshToken = new RefreshTokenEntity
			{
				EmployeeId = Id,
				ExpirationDate = DateTime.UtcNow.AddDays(Convert.ToInt32(_config["RefreshToken:ExpirationDays"])),
				RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
			};

			return refreshToken;
		}
    

		public async Task<RefreshTokenResponseDto> RefreshAccessTokenAsync(string refreshToken, IDbTransaction transaction = null)
		{
			var user = await _employeeRepository.GetUserByRefreshTokenAsync(refreshToken);

			if (user == null)
			{
				return null; 
			}

			var roles = await _employeeRepository.GetUserRolesAsync(user.Id);

			var newAccessToken = await GenerateTokenAsync(user, roles, transaction);

			var newRefreshToken = GenerateRefreshToken(user.Id);

			await _refreshTokenRepository.UpdateRefreshTokenAsync(newRefreshToken);

			return new RefreshTokenResponseDto
			{
				AccessToken = newAccessToken.AccessToken,
				RefreshToken = newRefreshToken.RefreshToken
			};
		}

	}
}
