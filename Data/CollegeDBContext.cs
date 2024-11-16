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
    }
}