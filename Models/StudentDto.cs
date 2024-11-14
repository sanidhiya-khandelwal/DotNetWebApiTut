using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tutWebApi.Models
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string? StudentName { get; set; }
        public string? Email { get; set; }
    }
}