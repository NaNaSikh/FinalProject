using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Queries.GetAllEmployees
{
    class GetAllEmployeesDto
    {
		    public required string FirstName { get; set; }
		    public required string LastName { get; set; }
		    public required string PersonalNumber { get; set; }
		    public DateTime BirthDate { get; set; }
		    public required string Email { get; set; }
		    public decimal Salary { get; set; }
		    public DateTime HireDate { get; set; }
		    public string UserName { get; set; }
		    public int? RecommenderEmployeeId { get; set; }
		    public String Role { get; set; }
	    
	}
}
