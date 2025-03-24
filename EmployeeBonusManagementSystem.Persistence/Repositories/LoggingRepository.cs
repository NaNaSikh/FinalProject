using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using Microsoft.Extensions.Logging;

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

		public void LogInformation(string message, params object[] args)
	    {
		    _logger.LogInformation(message, args);
		    //TODO add to database logic here 

		}

		public void LogError(string message, Exception exception, params object[] args)
	    {
		    _logger.LogError(exception, message, args);
		    //TODO add to database logic here 

		}

	}
}
