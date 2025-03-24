using System.Data;

namespace EmployeeBonusManagementSystem.Application.Contracts.Persistence.Common;

public interface ISqlQueryRepository
{
    Task<IEnumerable<T>> LoadData<T, U>(
        string sql,
        U parameters,
        string connectionId = "Default",
        CommandType commandType = CommandType.StoredProcedure);

    Task<T> LoadDataFirstOrDefault<T, U>(
        string sql,
        U parameters,
        string connectionId = "Default",
        CommandType commandType = CommandType.StoredProcedure);

    Task<IEnumerable<T>> LoadMultipleData<T, U>(
    string sql,
    U parameters,
    string connectionId = "Default",
    CommandType commandType = CommandType.StoredProcedure);
}
