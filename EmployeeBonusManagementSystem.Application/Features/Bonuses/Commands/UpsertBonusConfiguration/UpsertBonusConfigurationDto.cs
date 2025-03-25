namespace EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.UpdateOrInsertBonusConfiguration;

public class UpsertBonusConfigurationDto
{
    public string Status { get; set; } = string.Empty;
    public string? UpdatedFields { get; set; }
}
