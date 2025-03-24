using Dapper;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence.Common;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeBonusManagementSystem.Persistence.Repositories.Common;

internal class SqlCommandRepository
    : ISqlCommandRepository
{
    public async Task<int> SaveData<T>(
     string sql,
     T parameters,
     string connectionId = "Default",
     CommandType commandType = CommandType.StoredProcedure)
    {
        //using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
        using IDbConnection connection = new SqlConnection(connectionId);

        return await connection.ExecuteAsync(
            sql: sql,
            param: parameters,
            commandType: commandType);
    }
}
