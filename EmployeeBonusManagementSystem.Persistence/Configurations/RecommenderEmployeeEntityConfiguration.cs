using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeBonusManagementSystem.Persistence.Configurations;

public class RecommenderEmployeeEntityConfiguration : IEntityTypeConfiguration<RecommenderEmployeeEntity>
{
    public void Configure(EntityTypeBuilder<RecommenderEmployeeEntity> builder)
    {
        builder.ToTable("RecommenderEmployees");

        builder.HasKey(re => re.Id);

        builder.Property(re => re.AssignDate)
            .IsRequired();

        builder.HasOne<EmployeeEntity>()
            .WithMany()
            .HasForeignKey(re => re.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<EmployeeEntity>()
            .WithMany()
            .HasForeignKey(re => re.RecommenderEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
