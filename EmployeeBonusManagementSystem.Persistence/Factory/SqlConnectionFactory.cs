using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using Microsoft.Data.SqlClient;

namespace EmployeeBonusManagementSystem.Persistence.Factory
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
	    private readonly string _connectionString;

	    public SqlConnectionFactory(string connectionString)
	    {
		    _connectionString = connectionString;
	    }

	    public IDbConnection CreateConnection()
	    {
		    return new SqlConnection(_connectionString);
	    }
    }
}
