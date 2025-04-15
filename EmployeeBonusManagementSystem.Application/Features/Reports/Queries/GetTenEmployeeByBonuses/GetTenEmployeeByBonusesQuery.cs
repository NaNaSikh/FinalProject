using MediatR;
using System;
using System.Collections.Generic;

    namespace EmployeeBonusManagementSystem.Application.Features.Reports.Queries.GetTenEmployeeByBonuses;

public record GetTenEmployeeByBonusesQuery(
    DateTime StartDate,
    DateTime EndDate)
    : IRequest<List<EmployeeBonusesDto>>;


