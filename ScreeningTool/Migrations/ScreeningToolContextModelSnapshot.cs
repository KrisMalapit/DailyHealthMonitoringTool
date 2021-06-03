﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScreeningTool.Models;

namespace ScreeningTool.Migrations
{
    [DbContext(typeof(ScreeningToolContext))]
    partial class ScreeningToolContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ScreeningTool.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ScreeningTool.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<string>("Name");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("ScreeningTool.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<int>("CompanyId");

                    b.Property<string>("DepartmentHeads");

                    b.Property<string>("Name");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("ScreeningTool.Models.DepartmentHead", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContactNo");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("DepartmentHeads");
                });

            modelBuilder.Entity("ScreeningTool.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age");

                    b.Property<string>("Barangay");

                    b.Property<DateTime>("Birthday");

                    b.Property<string>("City");

                    b.Property<string>("ContactNo");

                    b.Property<string>("ContactPerson");

                    b.Property<string>("ContactPersonNo");

                    b.Property<int>("DepartmentId");

                    b.Property<string>("EmployeeId");

                    b.Property<DateTime>("FirstDose");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("PickUpPoint");

                    b.Property<DateTime>("SecondDose");

                    b.Property<string>("Status");

                    b.Property<int?>("Vaccinated");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("ScreeningTool.Models.Logs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("Status");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("ScreeningTool.Models.QREntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("QRKey");

                    b.Property<int>("ScreenLogId");

                    b.HasKey("Id");

                    b.ToTable("QREntry");
                });

            modelBuilder.Entity("ScreeningTool.Models.QurantineDetector", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateQuaratineSet");

                    b.Property<string>("EmployeeId");

                    b.HasKey("Id");

                    b.ToTable("QurantineDetectors");
                });

            modelBuilder.Entity("ScreeningTool.Models.ScreenLogs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age");

                    b.Property<string>("Barangay");

                    b.Property<string>("City");

                    b.Property<string>("ContactNo");

                    b.Property<string>("ContactPerson");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("EmployeeId");

                    b.Property<string>("EntryStatus");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<int>("Q1");

                    b.Property<int>("Q10");

                    b.Property<int>("Q11");

                    b.Property<int>("Q12");

                    b.Property<int>("Q13");

                    b.Property<int>("Q14");

                    b.Property<int>("Q15");

                    b.Property<int>("Q16");

                    b.Property<int>("Q17");

                    b.Property<int>("Q18");

                    b.Property<int>("Q19");

                    b.Property<int>("Q2");

                    b.Property<int>("Q20");

                    b.Property<int>("Q3");

                    b.Property<int>("Q4");

                    b.Property<int>("Q5");

                    b.Property<int>("Q6");

                    b.Property<int>("Q7");

                    b.Property<int>("Q8");

                    b.Property<int>("Q9");

                    b.Property<string>("QRKey");

                    b.Property<string>("Remarks");

                    b.Property<string>("RemarksQ1");

                    b.Property<string>("RemarksQ10");

                    b.Property<string>("RemarksQ11");

                    b.Property<string>("RemarksQ2");

                    b.Property<string>("RemarksQ3");

                    b.Property<string>("RemarksQ4");

                    b.Property<string>("RemarksQ5");

                    b.Property<string>("RemarksQ6");

                    b.Property<string>("RemarksQ7");

                    b.Property<string>("RemarksQ8");

                    b.Property<string>("RemarksQ9");

                    b.Property<int>("Result");

                    b.Property<decimal>("Temperature");

                    b.Property<DateTime>("TransactionDate");

                    b.Property<string>("WorkPlace");

                    b.HasKey("Id");

                    b.ToTable("ScreenLogs");
                });

            modelBuilder.Entity("ScreeningTool.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password");

                    b.Property<int>("RoleId");

                    b.Property<string>("Status");

                    b.Property<string>("Username")
                        .HasColumnType("VARCHAR(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ScreeningTool.Models.Department", b =>
                {
                    b.HasOne("ScreeningTool.Models.Company", "Companies")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ScreeningTool.Models.Employee", b =>
                {
                    b.HasOne("ScreeningTool.Models.Department", "Departments")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
