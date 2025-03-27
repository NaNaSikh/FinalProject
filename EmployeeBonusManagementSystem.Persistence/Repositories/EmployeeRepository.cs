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
        private readonly PasswordHasher<EmployeeEntity> _passwordHasher = new PasswordHasher<EmployeeEntity>();
        private readonly IUnitOfWork _unitOfWork;





        public EmployeeRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<(bool, int)> GetEmployeeExistsByPersonalNumberAsync(string personalNumber)
        {
            if (_unitOfWork.Connection.State != ConnectionState.Open)
            {
                await _unitOfWork.OpenAsync();
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var query = @"
				                SELECT 
				                    Id 
				                FROM
				                    Employees(NOLOCK)
				                WHERE
				                    PersonalNumber = @PersonalNumber;
            ";

                    var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<int>(
                        query,
                        new { PersonalNumber = personalNumber },
                        transaction: transaction,  // Use the transaction from UOW
                        commandType: CommandType.Text
                    );

                    transaction.Commit();

                    return (result > 0, result);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
            }
        }


        public async Task<bool> ExistsByPersonalNumberAsync(string personalNumber)
        {
            var query = "SELECT COUNT(1) FROM Employees WHERE PersonalNumber = @PersonalNumber";
            var count = await _unitOfWork.Connection.ExecuteScalarAsync<int>(query, new { PersonalNumber = personalNumber }, _transaction);
            return count > 0;
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var query = "SELECT COUNT(1) FROM Employees WHERE Email = @Email";
            var count = await _unitOfWork.Connection.ExecuteScalarAsync<int>(query, new { Email = email }, _transaction);
            return count > 0;
        }


        public async Task AddEmployeeAsync(EmployeeEntity employee, string role, IDbTransaction transaction)
        {
            if (_unitOfWork.Connection == null)
                throw new InvalidOperationException("Database connection is not initialized.");

            try
            {
                var query = @"
						INSERT INTO Employees (
							FirstName, LastName, PersonalNumber, BirthDate, Email, Password, Salary, HireDate, 
							UserName, DepartmentId, RecommenderEmployeeId, IsActive, IsPasswordChanged, 
							PasswordChangeDate, CreateByUserId, RefreshToken, CreateDate
						)
						VALUES (
							@FirstName, @LastName, @PersonalNumber, @BirthDate, @Email, @Password, @Salary, @HireDate, 
							@UserName, @DepartmentId, @RecommenderEmployeeId, @IsActive, @IsPasswordChanged, 
							@PasswordChangeDate, @CreateByUserId, @RefreshToken, @CreateDate
						);
						SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var employeeId = await _unitOfWork.Connection.QuerySingleAsync<int>(query, employee, transaction);

                var roleId = role.ToLower() switch
                {
                    "admin" => 1,
                    "user" => 2,
                    _ => throw new ArgumentException($"Invalid role: {role}")
                };

                var roleQuery = @"
                			INSERT INTO EmployeeRole (EmployeeId, RoleId)
                			VALUES (@EmployeeId, @RoleId)";

                await _unitOfWork.Connection.ExecuteAsync(roleQuery, new { EmployeeId = employeeId, RoleId = roleId }, transaction);
                var recommenderQuery = @"
									INSERT INTO RecommenderEmployees (EmployeeId, RecommenderEmployeeId , AssignDate)
									VALUES (@EmployeeId, @RecommenderEmployeeId ,  @AssignDate);";

                await _unitOfWork.Connection.ExecuteAsync(recommenderQuery, new
                {
                    EmployeeId = employeeId,
                    RecommenderEmployeeId = employee.RecommenderEmployeeId,
                    AssignDate = employee.CreateDate
                }, transaction);

                var departmentQuery = @"
									   INSERT INTO EmployeeDepartments (EmployeeId, DepartmentId , AssignDate , IsActive)
									   VALUES (@EmployeeId, @DepartmentId ,  @AssignDate , @IsActive);";

                await _unitOfWork.Connection.ExecuteAsync(departmentQuery, new
                {
                    EmployeeId = employeeId,
                    DepartmentId = employee.DepartmentId,
                    AssignDate = employee.CreateDate,
                    IsActive = 1
                }, transaction);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding employee and role: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GetEmployeePasswordByIdAsync(int Id)
        {
            if (_unitOfWork.Connection.State != ConnectionState.Open)
            {
                await _unitOfWork.OpenAsync();
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var query = @"
				                SELECT 
				                    Password 
				                FROM
				                    Employees(NOLOCK)
				                WHERE
				                    Id = @Id;";

                    var result = await _unitOfWork.Connection.QueryFirstOrDefaultAsync<string>(
                        query,
                        new { Id = Id },
                        transaction: transaction,  // Use the transaction from UOW
                        commandType: CommandType.Text
                    );

                    transaction.Commit();

                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<EmployeeEntity> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Employees WHERE Id = @Id";
            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<EmployeeEntity>(query, new { Id = id }, _transaction);
        }

		//	// Ensure the transaction and connection come from Unit of Work
		//	return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<EmployeeEntity>(
		//		query,
		//		new { Id = id },
		//		_unitOfWork.Transaction
		//	);
		//}


		public async Task<PasswordVerificationResult> CheckPasswordByIdAsync(int id, string enteredPassword)
        {
            var userPassword = await GetEmployeePasswordByIdAsync(id);

            if (string.IsNullOrEmpty(userPassword))
            {
                return PasswordVerificationResult.Failed;
            }

            var hasher = new PasswordHasher<EmployeeEntity>();
            return hasher.VerifyHashedPassword(null, userPassword, enteredPassword);
        }

        public async Task UpdateEmployeePasswordByIdAsync(int Id, string newHashedPassword)
        {

	        string query = "UPDATE Employees SET Password = @PasswordHash WHERE Id = @Id";
            var parameters = new { Id = Id, PasswordHash = newHashedPassword };
            await _unitOfWork.Connection.ExecuteAsync(query, parameters);

        }

        public async Task<EmployeeEntity> GetByEmailAsync(string email)
        {

            if (_transaction == null)
            {
                _transaction = _unitOfWork.BeginTransaction();
            }

            Console.WriteLine($"Connection is null: {_unitOfWork.Connection == null}");
            Console.WriteLine($"Transaction is null: {_transaction == null}");

            if (_unitOfWork?.Connection == null)
            {
                throw new InvalidOperationException("Database connection is not initialized.");
            }



            var query = "SELECT * FROM Employees WHERE Email = @Email";

            return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<EmployeeEntity>(
                query, new { Email = email }, _transaction);
        }

        public async Task<IEnumerable<EmployeeEntity>> GetAllEmployeesAsync()
        {
            var query = "SELECT * FROM Employees";
            return await _unitOfWork.Connection.QueryAsync<EmployeeEntity>(query, transaction: _transaction);
        }

        //public async Task UpdateAsync(EmployeeEntity employee)
        //{
        //    var query = @"
        //        UPDATE Employees SET FirstName = @FirstName, LastName = @LastName, PersonalNumber = @PersonalNumber, BirthDate = @BirthDate, Email = @Email, Password = @Password, Salary = @Salary, HireDate = @HireDate, DepartmentId = @DepartmentId, RecommenderEmployeeId = @RecommenderEmployeeId, IsActive = @IsActive, IsPasswordChanged = @IsPasswordChanged, PasswordChangeDate = @PasswordChangeDate
        //        WHERE Id = @Id";
        //    await _connection.ExecuteAsync(query, employee, _transaction);
        //}

        //public async Task DeleteAsync(EmployeeEntity employee)
        //{
        //    var query = "DELETE FROM Employees WHERE Id = @Id";
        //    await _connection.ExecuteAsync(query, new { Id = employee.Id }, _transaction);
        //}

        public async Task<bool> CheckPasswordAsync(EmployeeEntity user, string enteredPassword)
        {
            if (user == null || string.IsNullOrEmpty(user.Password))
            {
                return false;
            }

            var hasher = new PasswordHasher<EmployeeEntity>(); // Use the correct generic type
            var result = hasher.VerifyHashedPassword(user, user.Password, enteredPassword);
            Console.WriteLine($"{result}");

            if (result == PasswordVerificationResult.SuccessRehashNeeded)
            {

                var newHashedPassword = hasher.HashPassword(user, enteredPassword);
                user.Password = newHashedPassword;

                return true;
            }

            return result == PasswordVerificationResult.Success;
        }

        public async Task<List<string>> GetUserRolesAsync(int employeeId)
        {
            string sql = @"
            SELECT r.RoleName
			FROM EmployeeRole er
			INNER JOIN Roles r ON er.RoleId = r.RoleId
			WHERE er.EmployeeId =  @EmployeeId";

            // Execute the query using Dapper
            var roles = await _unitOfWork.Connection.QueryAsync<string>(
                sql,
                new { EmployeeId = employeeId },
                _unitOfWork.BeginTransaction() // Pass the transaction here
            );

            return roles.ToList();
        }


        public async Task<IEnumerable<EmployeeEntity>> GetEmployeeSalary(int Id)
        {

            var query = @"
			        SELECT FirstName , LastName , Salary 
					FROM Employees WHERE  Id = @Id";

            using var connection = _unitOfWork.Connection; // Remove parentheses
            var salary = await connection.QueryAsync<EmployeeEntity>(query, new { Id = Id });

            return salary.ToList();
        }


        public async Task<IEnumerable<BonusEntity>> GetEmployeeBonus(int Id)
        {

            var query = @"
			        SELECT e.PersonalNumber, b.Amount, b.CreateDate , b.Reason
					FROM Employees e
					INNER JOIN Bonuses b ON e.Id = b.EmployeeId
					WHERE e.Id = @Id";

            using var connection = _unitOfWork.Connection;

            var bonuses = await connection.QueryAsync<BonusEntity>(query, new { Id = Id });


            return bonuses.ToList();
        }

        public async Task<IEnumerable<EmployeeEntity>> GetEmployeeRecomender(int Id)
        {
            //TODO fix this 
            var query = @"
			        SELECT recommender.FirstName, recommender.LastName
					FROM Employees e
					INNER JOIN Employees recommender ON e.RecommenderEmployeeId = recommender.Id
					WHERE e.Id = @Id";

            using var connection = _unitOfWork.Connection;

            var recommender = await connection.QueryAsync<EmployeeEntity>(query, new { Id = Id });


            return recommender.ToList();
        }

		public async Task<EmployeeEntity> GetUserByRefreshTokenAsync(string refreshToken, IDbTransaction transaction)
		{
			var query = "SELECT * FROM Employees WHERE RefreshToken = @RefreshToken";

			// Ensure the transaction and connection come from Unit of Work
			return await _unitOfWork.Connection.QueryFirstOrDefaultAsync<EmployeeEntity>(
				query,
				new { RefreshToken = refreshToken },
				transaction
			);
		}

		public async Task UpdateRefreshTokenAsync(int userId, string newRefreshToken, IDbTransaction transaction)
		{
			var query = "UPDATE Employees SET RefreshToken = @RefreshToken WHERE Id = @UserId";

			// Ensure the transaction and connection come from Unit of Work
			await _unitOfWork.Connection.ExecuteAsync(
				query,
				new { RefreshToken = newRefreshToken, UserId = userId },
				transaction // Pass the transaction here
			);
		}
	}
}

