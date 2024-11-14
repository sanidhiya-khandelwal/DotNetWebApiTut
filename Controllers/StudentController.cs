using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tutWebApi.Models;

namespace tutWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("All")]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<Student>> GetStudentName()
        {
            return Ok(CollegeRepository.Students);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Student> GetStudentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest($"{id} less than or equla to 0 not allowed");
            }

            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();
            if (student == null)
            {
                return NotFound($"The student with {id} is not present");
            }
            return Ok(student);
        }

        [HttpGet("byname/{name:alpha}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Student> GetStudentByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest($"{name} should not be null");
            }
            var student = CollegeRepository.Students.Where(n => n.StudentName == name).FirstOrDefault();
            if (student == null)
            {
                return NotFound($"Student with {name} is not present");
            }
            return Ok(student);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<bool> DeleteStudent(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id should be greater than 0");
            }
            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();
            if (student == null)
            {
                return NotFound($"Student with id = {id} is not present");
            }
            CollegeRepository.Students.Remove(student);
            return Ok(true);
        }
    }
}