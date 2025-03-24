using Dapper;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace EmployeeBonusManagementSystem.Persistence.Repositories.Common;

internal class SqlQueryRepository(
    IConfiguration config)
    : ISqlQueryRepository
{
    public async Task<IEnumerable<T>> LoadData<T, U>(
    string sql,
    U parameters,
    string connectionId = "Default",
    CommandType commandType = CommandType.StoredProcedure)
    {
        //using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
        using IDbConnection connection = new SqlConnection(connectionId);

        return await connection.QueryAsync<T>(
            sql: sql,
            param: parameters,
            commandType: commandType);
    }


    public async Task<T> LoadDataFirstOrDefault<T, U>(
        string sql,
        U parameters,
        string connectionId = "Default",
        CommandType commandType = CommandType.StoredProcedure)
    {
        //using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
        using IDbConnection connection = new SqlConnection(connectionId);

        return await connection.QueryFirstOrDefaultAsync<T>(
            sql: sql,
            param: parameters,
            commandType: commandType);
    }

    public async Task<IEnumerable<T>> LoadMultipleData<T, U>(
    string sql,
    U parameters,
    string connectionId = "Default",
    CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection connection = new SqlConnection(connectionId);

        return await connection.QueryAsync<T>(
            sql: sql,
            param: parameters,
            commandType: commandType);
    }

}


