using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Contracts.Persistence.Common;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenEmployeeByBonuses;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenRecommenderByBonuses;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalBonuses;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalBonusesByDepartment;
using EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTotalSalariesByDepartment;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Infrastructure.Repositories
{
    public class ReportRepository(
        ISqlQueryRepository sqlQueryRepository,
        IConfiguration configuration)
        : IReportRepository
    {
        // ჯამურად გაცემული ბონუსების რაოდენობა მითითებულ თარიღებში     
        public async Task<TotalBonusesDto> GetTotalBonusesAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var query = @"
                    SELECT 
                        COUNT(*) AS TotalBonuses, 
                        SUM(Amount) AS TotalBonusAmount
                    FROM
                        Bonuses(NOLOCK)
                    WHERE
                        CreateDate BETWEEN @StartDate AND @EndDate;
                ";

                return await sqlQueryRepository.LoadDataFirstOrDefault<TotalBonusesDto, dynamic>(
                    query,
                    new { startDate, endDate },
                    configuration.GetConnectionString("DefaultConnection"),
                    CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        //10 თანამშრომელი, ვინც ყველაზე მეტი ბონუსი მიიღო მითითებულ თარიღში
        public async Task<List<EmployeeBonusesDto>> GetTenEmployeeByBonusesAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await sqlQueryRepository.LoadData<EmployeeBonusesDto, dynamic>(
                   "GetTenEmployeeByBonuses",
                   new { StartDate = startDate, EndDate = endDate },
                   configuration.GetConnectionString("DefaultConnection"),
                   CommandType.StoredProcedure);
                return result.ToList();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //10 თანამშრომელი, ვისი რეკომენდაციითაც ყველაზე მეტი ბონუსი გაიცა მითითებულ თარიღში
        public async Task<List<RecommenderBonusesDto>> GetTenRecommenderByBonusesAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await sqlQueryRepository.LoadData<RecommenderBonusesDto, dynamic>(
                       "GetTenRecommenderByBonuses",
                       new { StartDate = startDate, EndDate = endDate },
                       configuration.GetConnectionString("DefaultConnection"),
                       CommandType.StoredProcedure);
                return result.ToList();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //ჯამურად გაცემული ხელფასების რაოდენობა მითითებულ თარიღში  დეპარტამენტების მიხედვით
        public async Task<List<DepartmentSalaryDto>> GetTotalSalariesByDepartmentAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await sqlQueryRepository.LoadData<DepartmentSalaryDto, dynamic>(
                   "GetTotalSalariesByDepartment",
                   new { StartDate = startDate, EndDate = endDate },
                   configuration.GetConnectionString("DefaultConnection"),
                   CommandType.StoredProcedure);
                return result.ToList();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //ჯამურად გაცემული ბონუსების რაოდენობა მითითებულ თარიღში დეპარტამენტების მიხედვით
        public async Task<List<DepartmentBonusesDto>> GetTotalBonusesByDepartmentAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await sqlQueryRepository.LoadData<DepartmentBonusesDto, dynamic>(
                       "GetTotalBonusesByDepartment",
                       new { StartDate = startDate, EndDate = endDate },
                       configuration.GetConnectionString("DefaultConnection"),
                       CommandType.StoredProcedure);
                return result.ToList();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}