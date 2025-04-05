using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeBonusManagementSystem.Persistence.Configurations
{
    class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
	{
		public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
		{
			builder.ToTable("RefreshToken");

			builder.HasKey(b => b.Id);
			builder.Property(b => b.EmployeeId)
				.IsRequired();
			builder.Property(b => b.RefreshToken)
				.IsRequired();
			builder.Property(b => b.ExpirationDate)
				.IsRequired();

			builder.HasOne<EmployeeEntity>()
				.WithOne() 
				.HasForeignKey<RefreshTokenEntity>(e => e.EmployeeId) 
				.OnDelete(DeleteBehavior.Restrict); 
		}



	}
}
