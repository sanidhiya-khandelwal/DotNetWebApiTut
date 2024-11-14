using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tutWebApi.Models
{
    public class StudentDto
    {
        public int Id { get; set; }
        [Required]
        public string? StudentName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Address { get; set; }
    }
}