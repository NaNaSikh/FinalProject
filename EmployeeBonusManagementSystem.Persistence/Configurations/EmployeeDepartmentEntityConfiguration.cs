using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeBonusManagementSystem.Persistence.Configurations;

public class EmployeeDepartmentEntityConfiguration : IEntityTypeConfiguration<EmployeeDepartmentEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeDepartmentEntity> builder)
    {
        builder.ToTable("EmployeeDepartments");

        builder.HasKey(ed => ed.Id);

        builder.Property(ed => ed.AssignDate)
            .IsRequired();

        builder.Property(ed => ed.IsActive)
            .IsRequired();

        builder.HasOne<EmployeeEntity>()
            .WithMany()
            .HasForeignKey(ed => ed.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<DepartmentEntity>()
            .WithMany()
            .HasForeignKey(ed => ed.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
