﻿// <auto-generated />
using System;
using EmployeeBonusManagementSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EmployeeBonusManagementSystem.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250326090732_LogsFixeedd")]
    partial class LogsFixeedd
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.BonusConfigurationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreateByUserId")
                        .HasColumnType("int");

                    b.Property<decimal>("MaxBonusAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<double>("MaxBonusPercentage")
                        .HasColumnType("float");

                    b.Property<int>("MaxRecommendationLevel")
                        .HasColumnType("int");

                    b.Property<double>("MinBonusPercentage")
                        .HasColumnType("float");

                    b.Property<double>("RecommendationBonusRate")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("BonusConfigurations", (string)null);
                });

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.BonusEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("CreateByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsRecommenderBonus")
                        .HasColumnType("bit");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("RecommendationLevel")
                        .HasColumnType("int");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CreateByUserId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Bonuses", null, t =>
                        {
                            t.HasCheckConstraint("CK_Bonuses_CreateDate", "CreateDate < GETDATE()");
                        });
                });

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.DepartmentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreateByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("CreateByUserId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Departments", (string)null);
                });

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.EmployeeDepartmentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AssignDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeeDepartments", (string)null);
                });

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.EmployeeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreateByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPasswordChanged")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("PasswordChangeDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PersonalNumber")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<int?>("RecommenderEmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("CreateByUserId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("PersonalNumber")
                        .IsUnique();

                    b.HasIndex("RecommenderEmployeeId");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Employees", null, t =>
                        {
                            t.HasCheckConstraint("CK_Employee_BirthDate", "BirthDate < GETDATE()");

                            t.HasCheckConstraint("CK_Employee_HireDate", "HireDate <= GETDATE()");

                            t.HasCheckConstraint("CK_Employee_Salary", "Salary >= 0");
                        });
                });

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.EmployeeRoleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("RoleId");

                    b.ToTable("EmployeeRole", (string)null);
                });

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.ErrorLogsEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Exception")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ErrorLogs", (string)null);
                });

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.LogsEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ActionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Request")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Response")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Logs", (string)null);
                });

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.RecommenderEmployeeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AssignDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("RecommenderEmployeeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("RecommenderEmployeeId");

                    b.ToTable("RecommenderEmployees", (string)null);
                });

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.RolesEntity", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.BonusEntity", b =>
                {
                    b.HasOne("EmployeeBonusManagementSystem.Domain.Entities.EmployeeEntity", null)
                        .WithMany()
                        .HasForeignKey("CreateByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EmployeeBonusManagementSystem.Domain.Entities.EmployeeEntity", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.DepartmentEntity", b =>
                {
                    b.HasOne("EmployeeBonusManagementSystem.Domain.Entities.EmployeeEntity", null)
                        .WithMany()
                        .HasForeignKey("CreateByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.EmployeeDepartmentEntity", b =>
                {
                    b.HasOne("EmployeeBonusManagementSystem.Domain.Entities.DepartmentEntity", null)
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EmployeeBonusManagementSystem.Domain.Entities.EmployeeEntity", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.EmployeeEntity", b =>
                {
                    b.HasOne("EmployeeBonusManagementSystem.Domain.Entities.EmployeeEntity", null)
                        .WithMany()
                        .HasForeignKey("CreateByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EmployeeBonusManagementSystem.Domain.Entities.DepartmentEntity", null)
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EmployeeBonusManagementSystem.Domain.Entities.EmployeeEntity", null)
                        .WithMany()
                        .HasForeignKey("RecommenderEmployeeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.EmployeeRoleEntity", b =>
                {
                    b.HasOne("EmployeeBonusManagementSystem.Domain.Entities.EmployeeEntity", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EmployeeBonusManagementSystem.Domain.Entities.RolesEntity", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("EmployeeBonusManagementSystem.Domain.Entities.RecommenderEmployeeEntity", b =>
                {
                    b.HasOne("EmployeeBonusManagementSystem.Domain.Entities.EmployeeEntity", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EmployeeBonusManagementSystem.Domain.Entities.EmployeeEntity", null)
                        .WithMany()
                        .HasForeignKey("RecommenderEmployeeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
