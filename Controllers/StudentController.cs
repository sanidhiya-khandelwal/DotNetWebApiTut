using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using tutWebApi.Models;
using tutWebApi.Data;

namespace tutWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly CollegeDBContext _dbContext;

        public StudentController(ILogger<StudentController> logger, CollegeDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<StudentDto>> GetStudentName()
        {
            _logger.LogInformation("All students method called");
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
            // var students = _dbContext.Students.Select(item => new StudentDto()
            // {
            //     Id = item.Id,
            //     StudentName = item.StudentName,
            //     Email = item.Email,
            //     Address = item.Address
            // });

            // Method 3:  after  using EF we have EF crud operations
            // var students = _dbContext.Students; //this will read all the values inside Students (Student model), to manipulate somethinig we can go for DTO and remove in controller itself some values that u dont want or add some values if u want to calculate something
            var students = _dbContext.Students.ToList(); //above and below line both are same
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
                _logger.LogWarning("Bad Request");
                return BadRequest($"{id} less than or equla to 0 not allowed");
            }

            var student = _dbContext.Students.Where(n => n.Id == id).FirstOrDefault();
            if (student == null)
            {
                _logger.LogError("Student not found with given id");
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
                _logger.LogWarning("Bad Request");
                return BadRequest($"{name} should not be null");
            }
            var student = _dbContext.Students.Where(n => n.StudentName == name).FirstOrDefault();
            if (student == null)
            {
                _logger.LogError("Student with given name not found");
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
                _logger.LogWarning("Bad Request");
                return BadRequest("Id should be greater than 0");
            }
            var student = _dbContext.Students.Where(n => n.Id == id).FirstOrDefault();
            if (student == null)
            {
                _logger.LogError("Studnt with given id not found");
                return NotFound($"Student with id = {id} is not present");
            }
            _dbContext.Students.Remove(student);

            _dbContext.SaveChanges();
            return Ok(true);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDto> CreateStudent([FromBody] StudentDto model)
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }
            if (model == null)
            {
                _logger.LogWarning("Bad Request");
                return BadRequest("The input can not be null");
            }

            // if (model.AddmissionDate < DateTime.Now)
            // {
            //     ModelState.AddModelError("AddmissionDate Error", "Addmission date shold be greater than current date");
            // }
           
            //since now id is autoincremented we dont need this now
            // int newId = CollegeRepository.Students.LastOrDefault().Id + 1;

            Student student = new Student
            {
                StudentName = model.StudentName,
                Email = model.Email,
                Address = model.Address
            };
            _dbContext.Students.Add(student);
            _dbContext.SaveChanges();

            model.Id = student.Id;
            return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudent([FromBody] StudentDto model)
        {
            if (model == null || model.Id <= 0)
            {
                _logger.LogWarning("Bad Request");
                return BadRequest();
            }
            var existingStudent = _dbContext.Students.Where(s => s.Id == model.Id).FirstOrDefault();

            if (existingStudent == null)
            {
                _logger.LogError("Student not found");
                return NotFound();
            }
            existingStudent.StudentName = model.StudentName;
            existingStudent.Address = model.Address;
            existingStudent.Email = model.Email;

            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDto> patchDocument)
        {
            if (patchDocument == null || id <= 0)
            {
                _logger.LogWarning("Bad Request");
                return BadRequest();
            }
            var existingStudent = _dbContext.Students.Where(s => s.Id == id).FirstOrDefault();

            if (existingStudent == null)
            {
                _logger.LogError("Student not found");
                return NotFound();
            }

            var studentDto = new StudentDto
            {
                Id = existingStudent.Id,
                StudentName = existingStudent.StudentName,
                Email = existingStudent.Email,
                Address = existingStudent.Address
            };
            //ModelState has errors so we can use it to throw bad request
            patchDocument.ApplyTo(studentDto, ModelState);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Bad Request");
                return BadRequest(ModelState);
            }
            existingStudent.StudentName = studentDto.StudentName;
            existingStudent.Address = studentDto.Address;
            existingStudent.Email = studentDto.Email;

            _dbContext.SaveChanges();
            return NoContent();
        }
    }
}