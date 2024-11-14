using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using tutWebApi.Models.Validators;

namespace tutWebApi.Models
{
    public class StudentDto
    {
        [ValidateNever]
        public int Id { get; set; }
        [Required(ErrorMessage = "Student name is required")]
        [StringLength(30)]
        public string? StudentName { get; set; }
        [EmailAddress(ErrorMessage = " Email address is required")]
        public string? Email { get; set; }
        [Required]
        public string? Address { get; set; }
        [Range(10, 20)]
        public int age { get; set; }
        public string? Password { get; set; }
        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
        [DateCheck]
        public DateTime AddmissionDate { get; set; }
    }
}