using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeBonusManagementSystem.Persistence.Configurations;

public class DepartmentEntityConfiguration : IEntityTypeConfiguration<DepartmentEntity>
{
    public void Configure(EntityTypeBuilder<DepartmentEntity> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .HasMaxLength(255)
            .IsRequired();
        builder.HasIndex(e => e.Name)
	        .IsUnique();

		builder.Property(d => d.CreateDate)
            .IsRequired();

        builder.Property(d => d.IsActive)
            .IsRequired();

        builder.Property(d => d.CreateByUserId)
            .IsRequired();

        builder.HasOne<EmployeeEntity>()
	        .WithMany()
	        .HasForeignKey(e => e.CreateByUserId)
	        .OnDelete(DeleteBehavior.Restrict);
	}
}
