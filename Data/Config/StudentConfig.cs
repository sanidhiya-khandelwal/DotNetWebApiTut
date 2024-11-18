using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace tutWebApi.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(n => n.StudentName).IsRequired();
            builder.Property(n => n.StudentName).HasMaxLength(250);
            builder.Property(n => n.Address).IsRequired(false).HasMaxLength(500);
            builder.Property(n => n.Email).IsRequired().HasMaxLength(250);

            builder.HasData(
                new List<Student>()
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