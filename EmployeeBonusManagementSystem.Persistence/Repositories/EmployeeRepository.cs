using Dapper;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace EmployeeBonusManagementSystem.Persistence.Repositories.Implementations
{
	public class EmployeeRepository : IEmployeeRepository
	{
		private IDbTransaction _transaction;
		private IDbConnection _connection;
		public void SetConnection(IDbConnection connection)
		{
			_connection = connection;
		}

		public void SetTransaction(IDbTransaction transaction)
		{
			_transaction = transaction;
		}


		public async Task<(bool, int)> GetEmployeeExistsByPersonalNumberAsync(string personalNumber)
		{
			var query = @"
                SELECT
                    Id
                FROM
                    Employees (NOLOCK)
                WHERE
                    PersonalNumber = @PersonalNumber; ";

			var result = await _connection.QueryFirstOrDefaultAsync<int>(
				query,
				new { PersonalNumber = personalNumber },
				transaction: _transaction,
				commandType: CommandType.Text);

			return (result > 0, result);
		}

		public async Task AddEmployeeAsync(EmployeeEntity employee, string role)
		{
			if (_connection == null)
				throw new InvalidOperationException("Database connection is not initialized.");

			try
			{
				var query = @"
                    INSERT INTO Employees (
                        FirstName, LastName, PersonalNumber, BirthDate, Email, Password, Salary, HireDate,
                        UserName,  RecommenderEmployeeId, IsActive, IsPasswordChanged,
                        PasswordChangeDate, CreateByUserId, RefreshToken, CreateDate
                    )
                    VALUES (
                        @FirstName, @LastName, @PersonalNumber, @BirthDate, @Email, @Password, @Salary, @HireDate,
                        @UserName, @RecommenderEmployeeId, @IsActive, @IsPasswordChanged,
                        @PasswordChangeDate, @CreateByUserId, @RefreshToken, @CreateDate
                    );
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

				var employeeId = await _connection.QuerySingleAsync<int>(query, employee, _transaction);


				// fix this 
				int roleId = role.ToLower() switch
				{
					"admin" => 1,
					"user" => 2,
					_ => throw new ArgumentException($"Invalid role: {role}")
				};

				var roleQuery = @"
                    INSERT INTO EmployeeRole (EmployeeId, RoleId)
                    VALUES (@EmployeeId, @RoleId)";

				await _connection.ExecuteAsync(roleQuery, new { EmployeeId = employeeId, RoleId = roleId },
					_transaction);

				if (employee.RecommenderEmployeeId.HasValue)
				{
					var recommenderQuery = @"
                        INSERT INTO RecommenderEmployees (EmployeeId, RecommenderEmployeeId, AssignDate)
                        VALUES (@EmployeeId, @RecommenderEmployeeId, @AssignDate);";

					await _connection.ExecuteAsync(recommenderQuery, new
					{
						EmployeeId = employeeId,
						RecommenderEmployeeId = employee.RecommenderEmployeeId,
						AssignDate = employee.CreateDate
					}, _transaction);
				}

				var departmentQuery = @"
                        INSERT INTO EmployeeDepartment (EmployeeId, DepartmentId, AssignDate, IsActive)
                        VALUES (@EmployeeId, @DepartmentId, @AssignDate, @IsActive);";

				await _connection.ExecuteAsync(departmentQuery, new
				{
					EmployeeId = employeeId,
					DepartmentId = employee.DepartmentId,
					AssignDate = employee.CreateDate,
					IsActive = 1
				}, _transaction);


			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error adding employee and related data: {ex.Message}");
				throw;
			}
		}


		public async Task<string> GetEmployeePasswordByIdAsync(int Id)
        {
			const string query = @"
				        SELECT  Password 
				        FROM Employees WITH (NOLOCK)
				        WHERE Id = @Id;";

			return await _connection.QueryFirstOrDefaultAsync<string>(
				query, new { Id = Id },
				transaction: _transaction,
				commandType: CommandType.Text

			);
		}


        public async Task UpdateEmployeePasswordByIdAsync(int Id, string newHashedPassword)
        {

	        string query = "UPDATE Employees SET Password = @PasswordHash WHERE Id = @Id";
            var parameters = new { Id = Id, PasswordHash = newHashedPassword };
            await _connection.ExecuteAsync(query, parameters);

        }

        public async Task<EmployeeEntity?> GetByEmailAsync(string email)
        {

			const string query = "SELECT * FROM Employees WHERE Email = @Email";

			var employee = await _connection.QueryFirstOrDefaultAsync<EmployeeEntity>(
				query, new { Email = email }, 
				commandType: CommandType.Text ,
				transaction: _transaction);
			return employee;
		}

        public async Task<IEnumerable<EmployeeEntity>> GetAllEmployeesAsync()
        {
			const string query = "SELECT * FROM Employees";
			return await _connection.QueryAsync<EmployeeEntity>(query);
		}
        public async Task<List<string>> GetUserRolesAsync(int employeeId)
        {
			string sql = @"
                SELECT r.RoleName
                FROM EmployeeRole er
                INNER JOIN Roles r ON er.RoleId = r.RoleId
                WHERE er.EmployeeId = @EmployeeId";

			var roles = await _connection.QueryAsync<string>(
				sql,
				new { EmployeeId = employeeId },
				transaction: _transaction 
			);

			return roles.ToList();
		}


        public async Task<IEnumerable<EmployeeEntity>> GetEmployeeSalary(int Id)
        {

			const string query = @"
				            SELECT FirstName, LastName, Salary 
				            FROM Employees 
				            WHERE Id = @Id";

			var result = await _connection.QueryAsync<EmployeeEntity>(query, new { Id = Id } , _transaction);
			return result.ToList();
		}


        public async Task<IEnumerable<BonusEntity>> GetEmployeeBonus(int Id)
        {

			const string query = @"
			            SELECT e.PersonalNumber, b.Amount, b.CreateDate, b.Reason
			            FROM Employees e
			            INNER JOIN Bonuses b ON e.Id = b.EmployeeId
			            WHERE e.Id = @Id";

			var bonuses = await _connection.QueryAsync<BonusEntity>(query, new { Id = Id } , _transaction);
			return bonuses.ToList();
		}

        public async Task<IEnumerable<EmployeeEntity>> GetEmployeeRecommender(int Id)
        {
            var query = @"
			        SELECT recommender.FirstName, recommender.LastName
					FROM Employees e
					INNER JOIN Employees recommender ON e.RecommenderEmployeeId = recommender.Id
					WHERE e.Id = @Id";
            var recommender = await _connection.QueryAsync<EmployeeEntity>(query, new { Id = Id } , transaction: _transaction);

			return recommender.ToList();
        }

		public async Task<EmployeeEntity> GetUserByRefreshTokenAsync(string refreshToken)
		{
			const string query = @"SELECT e.*
								FROM Employees e
								INNER JOIN RefreshToken rt ON e.Id = rt.EmployeeId
								WHERE rt.RefreshToken = @RefreshToken";

			var employee = await _connection.QueryFirstOrDefaultAsync<EmployeeEntity>(
				query, new { RefreshToken = refreshToken },
				transaction: _transaction ,
				commandType: CommandType.Text);

			return employee;
		}

		
	}
}

