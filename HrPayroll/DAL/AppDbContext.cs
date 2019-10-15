using HrPayroll.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrPayroll.ViewModel;

namespace HrPayroll.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Bonus> Bonus { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyMonthGain> Gains { get; set; }
        public DbSet<CompanyToDepartment> GetCompanyToDepartments { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Holding> Holdings { get; set; }
        public DbSet<HoldingToDepartment> HoldingToDepartments { get; set; }
        public DbSet<OldWorkPlace> oldWorkPlaces { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<Penal> Penals { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Vocation> Vocations { get; set; }
        public DbSet<WorkPlace> WorkPlaces { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseLazyLoadingProxies();
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CompanyToDepartment>().HasKey(x => new { x.CompanyId, x.DepartmentId });
            builder.Entity<HoldingToDepartment>().HasKey(y => new { y.HoldingId, y.DepartmentId });
            //one-to-one
        builder.Entity<AppUser>()
       .HasOne(a => a.Employee)
       .WithOne(b => b.AppUser)
       .HasForeignKey<AppUser>(b => b.EmployeeId);

            //seed default role(AccountControllerde SeedAdmin methodu) and user for Admin

            builder.Entity(typeof(Employee)).HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Mina",
                    Surname = "Farzali",
                    FatherName = "Faiq",
                    Birthdate = new DateTime(1998, 08, 26),
                    Adress = "Sumqayit seher",
                    RegisterAdress = "Sumqayit seher",
                    PassportId = "AZE0091625",
                    PassportExpDate = "2020",
                    Education = "Bakalavr",
                    MartialStatus = MartialStatus.Single,
                    Gender = Gender.Woman,

                });
        }
        public DbSet<HrPayroll.ViewModel.RegisterViewModel> RegisterViewModel { get; set; }
    }
}
