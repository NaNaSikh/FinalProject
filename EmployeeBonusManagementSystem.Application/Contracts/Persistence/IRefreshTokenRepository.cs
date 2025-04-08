using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Domain.Entities;

namespace EmployeeBonusManagementSystem.Application.Contracts.Persistence
{
    public  interface IRefreshTokenRepository
    {
	    Task<RefreshTokenEntity> GetEmployeeRefreshTokenByIdAsync(int Id);
	    Task UpdateRefreshTokenAsync(RefreshTokenEntity refreshToken);
	    Task AddNewRefreshTokenAsync(RefreshTokenEntity refreshToken);


    }
}
