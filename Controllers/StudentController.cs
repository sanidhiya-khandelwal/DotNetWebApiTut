using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tutWebApi.Models;

namespace tutWebApi.Controllers
{
    [Route("api/[controller]")]
    // [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StudentDto>> GetStudentName()
        {
            //Method 1: loop thru forEach
            // var students = new List<StudentDto>();
            // foreach (var item in CollegeRepository.Students)
            // {
            //     StudentDto obj = new StudentDto()
            //     {
            //         Id = item.Id,
            //         StudentName = item.StudentName,
            //         Email = item.Email
            //     };
            //     students.Add(obj);
            // }

            //Method 2: Loop using LINQ
            var students = CollegeRepository.Students.Select(item => new StudentDto()
            {
                Id = item.Id,
                StudentName = item.StudentName,
                Email = item.Email,
                Address = item.Address
            });

            return Ok(students);
        }

        [HttpGet]
        [Route("{id}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDto> GetStudentById(int id)
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
            var studentDto = new StudentDto
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Email = student.Email,
                Address = student.Address
            };
            return Ok(student);
        }

        [HttpGet("byname/{name:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDto> GetStudentByName(string name)
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
            var studentDto = new StudentDto()
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Email = student.Email,
                Address = student.Address
            };
            return Ok(studentDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDto> CreateStudent([FromBody] StudentDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model == null)
            {
                return BadRequest("The input can not be null");
            }
            int newId = CollegeRepository.Students.LastOrDefault().Id + 1;

            Student student = new Student
            {
                Id = newId,
                StudentName = model.StudentName,
                Email = model.Email,
                Address = model.Address
            };
            CollegeRepository.Students.Add(student);

            model.Id = newId;
            return CreatedAtRoute("GetStudentById", new { id = newId }, model);
        }
    }
}