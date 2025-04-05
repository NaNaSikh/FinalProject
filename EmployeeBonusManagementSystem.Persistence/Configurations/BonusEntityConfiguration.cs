using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeBonusManagementSystem.Persistence.Configurations;

public class BonusEntityConfiguration : IEntityTypeConfiguration<BonusEntity>
{
		public void Configure(EntityTypeBuilder<BonusEntity> builder)
    {
        builder.ToTable("Bonuses");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.EmployeeId)
	        .IsRequired();

        builder.Property(b => b.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(b => b.Reason)
            .HasMaxLength(500);

        builder.Property(b => b.CreateDate)
            .IsRequired();

        builder.Property(b => b.CreateByUserId)
            .IsRequired();

        builder.Property(b => b.IsRecommenderBonus)
            .IsRequired();

        builder.Property(b => b.RecommendationLevel)
            .IsRequired();

		builder.Property(b => b.TransactionId)
			.HasColumnType("uniqueidentifier")
			.IsRequired();

		builder.HasCheckConstraint("CK_Bonuses_CreateDate", "CreateDate <= GETDATE()");

		builder.HasOne<EmployeeEntity>()
	        .WithMany()
	        .HasForeignKey(e => e.CreateByUserId)
	        .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<EmployeeEntity>()
	        .WithMany()
	        .HasForeignKey(e => e.EmployeeId)
	        .OnDelete(DeleteBehavior.Restrict);
	}
}

	