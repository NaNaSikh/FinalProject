using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace EmployeeBonusManagementSystem.Application.Features.Employees.Commands.RefreshToken
{
    public record RefreshTokenCommand(string refreshToken) :IRequest <RefreshTokenResponseDto>
    {
    }
}
