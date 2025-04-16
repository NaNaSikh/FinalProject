using Dapper;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence.Common;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeBonusManagementSystem.Persistence.Repositories.Common;

public class SqlCommandRepository
    : ISqlCommandRepository
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

    public async Task<int> SaveData<T>(string sql, T parameters,
                                     string connectionId = "Default",
                                     CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection connection = new SqlConnection(connectionId);
        return await connection.ExecuteAsync(
            sql: sql,
            param: parameters,
            commandType: commandType);
    }
}
