using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeBonusManagementSystem.Persistence.Configurations
{
    class LogsEntityConfiguration : IEntityTypeConfiguration<LogsEntity>
	{
		public void Configure(EntityTypeBuilder<LogsEntity> builder)
		{

			builder.ToTable("Logs");

			builder.HasKey(b => b.Id);

			builder.Property(b => b.UserId)
				.IsRequired();
			builder.Property(b => b.TimeStamp)
				.IsRequired();
			builder.Property(b => b.ActionType)
				.IsRequired();
			builder.Property(b => b.Request)
				.IsRequired();
			builder.Property(b => b.Response)
				.IsRequired();

		}

	}
}
