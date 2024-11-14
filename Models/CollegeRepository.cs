using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tutWebApi.Models
{
    public static class CollegeRepository
    {
        public static List<Student> Students { get; set; } = new List<Student>(){
                new Student
                {
                    Id = 1,
                    StudentName = "Sanidhh khandelwal",
                    Email = "test#gmail.com",
                    Address = "Hyd, India"
                },
                new Student
                {
                    Id = 2,
                    StudentName = "bravo jhon",
                    Email = "ntpsn#gmail.com",
                    Address = "bng, India"
                }
        };
    }
}