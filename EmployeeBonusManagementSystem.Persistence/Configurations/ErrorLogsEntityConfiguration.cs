using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeBonusManagementSystem.Persistence.Configurations
{
    class ErrorLogsEntityConfiguration : IEntityTypeConfiguration<ErrorLogsEntity>
	{
		public void Configure(EntityTypeBuilder<ErrorLogsEntity> builder)
		{

			builder.ToTable("ErrorLogs");

			builder.HasKey(b => b.Id);

			builder.Property(b => b.UserId)
				.IsRequired();
			builder.Property(b => b.TimeStamp)
				.IsRequired();
			builder.Property(b => b.Level)
				.IsRequired();
			builder.Property(b => b.Message)
				.IsRequired();
			builder.Property(b => b.Exception)
				.IsRequired();

			
		}
	}
}
