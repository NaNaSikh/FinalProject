using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Abstractions;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Bcpg;

namespace EmployeeBonusManagementSystem.Persistence.Repositories
{
    public class LoggingRepository : ILoggingRepository
    {
	    private readonly ILogger<LoggingRepository> _logger;
	    private readonly IUnitOfWork _unitOfWork;


		public LoggingRepository(ILogger<LoggingRepository> logger, IUnitOfWork unitOfWork)
	    {
		    _logger = logger;
		    _unitOfWork = unitOfWork;

		}

		public async Task LogInformationAsync(LogsEntity logsEntity)
		{
			_logger.LogInformation("Logging Action: {ActionType} by User {UserId} at {TimeStamp}",
				logsEntity.ActionType, logsEntity.UserId, logsEntity.TimeStamp);

			var query = @"INSERT INTO Logs (TimeStamp, UserId, ActionType, Request, Response) 
                  VALUES (@TimeStamp, @UserId, @ActionType, @Request , @Response);";

			if (_unitOfWork.Connection.State != ConnectionState.Open)
			{
				await _unitOfWork.OpenAsync();
			}

			try
			{
				await _unitOfWork.Connection.ExecuteAsync(query, new
				{
					TimeStamp = logsEntity.TimeStamp,
					UserId = logsEntity.UserId,
					ActionType = logsEntity.ActionType,
					Request = logsEntity.Request,
					Response = logsEntity.Response
				}, _unitOfWork.Transaction);  // ✅ Use existing transaction

				// ✅ Do NOT commit or rollback here, let the UoW handle it
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error adding Logging Information: {Message}", ex.Message);
				throw;
			}
		}


		public async Task LogErrorInformationAsync  (ErrorLogsEntity errorlogsEntity)
	    {
			_logger.LogInformation("Logging Error: {Exception} by User {UserId} at {TimeStamp}",
				errorlogsEntity.Exception, errorlogsEntity.UserId, errorlogsEntity.TimeStamp);

			var query = @"INSERT INTO ErrorLogs (TimeStamp, UserId, Level, Message, Exception) 
                  VALUES (@TimeStamp, @UserId, @Level, @Message , @Exception);";

			if (_unitOfWork.Connection.State != ConnectionState.Open)
			{
				await _unitOfWork.OpenAsync();
			}

			try
			{
				await _unitOfWork.Connection.ExecuteAsync(query, new
				{
					TimeStamp = errorlogsEntity.TimeStamp,
					UserId = errorlogsEntity.UserId,
					Level = errorlogsEntity.Level,
					Message = errorlogsEntity.Message,
					Exception = errorlogsEntity.Exception
				}, _unitOfWork.Transaction);  

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error adding Logging Information: {Message}", ex.Message);
				throw;
			}

		}

	}
}
