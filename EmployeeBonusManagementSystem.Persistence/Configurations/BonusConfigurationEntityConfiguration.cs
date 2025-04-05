using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeBonusManagementSystem.Persistence.Configurations;

public class BonusConfigurationEntityConfiguration : IEntityTypeConfiguration<BonusConfigurationEntity>
{
    public void Configure(EntityTypeBuilder<BonusConfigurationEntity> builder)
    {
        builder.ToTable("BonusConfigurations");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.MaxBonusAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(b => b.MaxBonusPercentage)
            .IsRequired();

        builder.Property(b => b.MinBonusPercentage)
            .IsRequired();

        builder.Property(b => b.MaxRecommendationLevel)
            .IsRequired();

        builder.Property(b => b.RecommendationBonusRate)
            .IsRequired();

        builder.Property(b => b.CreateByUserId)
          .IsRequired();
    }
}
	