using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeBonusManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeBonusManagementSystem.Persistence.Configurations
{
	public class RolesConfiguration : IEntityTypeConfiguration<RolesEntity>
	{
		public void Configure(EntityTypeBuilder<RolesEntity> builder)
		{
			builder.ToTable("Roles");
			builder.HasKey(b => b.RoleId);

			builder.Property(b => b.RoleName)
				.IsRequired();
		}
	}
}
