using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagement.Application.Services.Interfaces;
using EmployeeBonusManagementSystem.Application.Features.Employees.Common;
using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeBonusManagement.Application.Services
{
    public class JwtService : IJwtService
    {
	    private readonly IConfiguration _config;

	    public JwtService(IConfiguration config)
	    {
		    _config = config;
	    }

	    public AuthResponse GenerateToken(EmployeeEntity user, IList<string> roles)
	    {
		    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
		    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		    var claims = new List<Claim>
		    {
			    new Claim("Id", user.Id.ToString()),
			    new Claim(ClaimTypes.Email, user.Email)
		    };

		    // Add user roles to claims
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

		    return new AuthResponse
		    {
			    Success = true,
			    AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
			    RefreshToken = GenerateRefreshToken(),
			    Expiration = token.ValidTo,
			    UserEmail = user.Email,
			    Roles = roles.ToList()
		    };
	    }

	    public string GenerateRefreshToken()
	    {
		    return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
	    }
	}
}
