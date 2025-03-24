using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Commands.ChaingePassword
{
	public record ChangePasswordCommand(string currentPassword, string newPassword) : IRequest<ChangePasswordDto>;
}
