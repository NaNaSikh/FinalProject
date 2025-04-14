using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Domain.Entities;

namespace EmployeeBonusManagementSystem.Persistence.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
	{
	    private IDbConnection _connection;
	    private IDbTransaction _transaction;

	    public void SetConnection(IDbConnection connection)
	    {
		    _connection = connection;
	    }

	    public void SetTransaction(IDbTransaction transaction)
	    {
		    _transaction = transaction;
	    }

		public async Task<RefreshTokenEntity?> GetEmployeeRefreshTokenByIdAsync(int Id)
	    {
		    const string query = @"
				        SELECT EmployeeId, RefreshToken, ExpirationDate
						FROM RefreshToken WITH (NOLOCK)
						WHERE EmployeeId = @Id;";

		    var ans =   await _connection.QueryFirstOrDefaultAsync<RefreshTokenEntity>(
			    query, new { Id = Id },
				transaction: _transaction,
			    commandType: CommandType.Text
		    );
		    if (ans == null) return null;

		    return ans;
	    }

	    public async Task UpdateRefreshTokenAsync(RefreshTokenEntity refreshToken)
	    {
		    var query = "UPDATE RefreshToken SET RefreshToken = @RefreshToken , ExpirationDate = @ExpirationDate WHERE EmployeeId = @EmployeeId";

		    await _connection.ExecuteAsync(
			    query, new {
					RefreshToken = refreshToken.RefreshToken,
					ExpirationDate = refreshToken.ExpirationDate,
					EmployeeId = refreshToken.EmployeeId },
					transaction: _transaction,
					commandType: CommandType.Text
				);

	    }

	    public async Task AddNewRefreshTokenAsync(RefreshTokenEntity refreshToken)
	    {

		    var query = @"
                			INSERT INTO RefreshToken (EmployeeId, RefreshToken , ExpirationDate)
                			VALUES (@EmployeeId, @RefreshToken , @ExpirationDate );";

		    await _connection.ExecuteAsync(
			    query, new { 
					RefreshToken = refreshToken.RefreshToken,
					ExpirationDate = refreshToken.ExpirationDate, 
					EmployeeId = refreshToken.EmployeeId },
					transaction: _transaction
				);


		}
	}
}
