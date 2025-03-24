using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetEmployeeRecomendator
{
    public record  GetEmployeeRecommenderQuery() : IRequest<List<GetEmployeeRecommenderDto>>
    {
	    public int UserId { get; set; }
    }
    
}
