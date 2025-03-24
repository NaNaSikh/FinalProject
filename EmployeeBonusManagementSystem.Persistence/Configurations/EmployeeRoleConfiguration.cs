using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace EmployeeBonusManagementSystem.Persistence.Configurations
{
	class EmployeeRoleConfiguration : IEntityTypeConfiguration<EmployeeRoleEntity>
	{
		public void Configure(EntityTypeBuilder<EmployeeRoleEntity> builder)
		{
			builder.ToTable("EmployeeRole");

			builder.HasKey(b => b.Id);

			builder.HasOne<EmployeeEntity>()
				.WithMany()
				.HasForeignKey(b => b.EmployeeId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne<RolesEntity>()
				.WithMany()
				.HasForeignKey(b => b.RoleId)
				.OnDelete(DeleteBehavior.Restrict);

		}
	}
}
