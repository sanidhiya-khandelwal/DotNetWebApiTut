using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace tutWebApi.Data
{
    public class CollegeDBContext : DbContext
    {
        //step 2:constructor for db connection
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options) : base(options)
        {

        }
        //step 1: create table dataset
        DbSet<Student> Students { get; set; }

        /*after overriding the below method and adding the dummy data, write below commands
            -  dotnet ef migrations add AddDummyData
            -  dotnet ef database update
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(new List<Student>()
            {
                new Student
                {
                    Id = 1,
                    StudentName = "Venkat",
                    Address = "India,Hyd",
                    Email = "Venkat@gmail.com"
                },
                new Student
                {
                    Id = 2,
                    StudentName = "Venkat1",
                    Address = "India,Hyd1",
                    Email = "Venkat1@gmail.com"
                },
            }
            );
        }
    }
}