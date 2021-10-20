using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanzApi.Dtos;
using AttendanzApi.Models;
using AutoMapper;
using AttendanzApi.Interfaces;

namespace AttendanzApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        private readonly ILogger<GroupsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<StudentModel> _students;

        public StudentsController(
            ILogger<GroupsController> logger,
            IMapper mapper,
            IRepository<StudentModel> students)
        {
            _logger = logger;
            _mapper = mapper;
            _students = students;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(long id)
        {
            var student = _students.GetById(id);
            if (student == null)
                return NotFound();

            var dto = _mapper.Map(student, new StudentDto());
            return Ok(dto);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var students = _students.GetAll();
            var dtos = students.Select(student => _mapper.Map(student, new StudentDto()));

            return Ok(dtos);
        }

        [HttpPost]
        public IActionResult Post([FromBody] StudentDto dto)
        {
            var student = _mapper.Map(dto, new StudentModel());
            _students.Insert(student);

            return Ok(_mapper.Map(student, new StudentDto()));
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put(long id, [FromBody] StudentDto dto)
        {
            var student = _students.GetById(id);
            if (student == null)
                return NotFound();

            _mapper.Map(dto, student);
            _students.Update(student);

            return Ok(_mapper.Map(student, new StudentDto()));
        }

        [HttpDelete]
        [Route("id")]
        public IActionResult Delete(long id)
        {
            var student = _students.GetById(id);
            if (student == null)
                return NotFound();

            _students.Delete(student);

            return Ok();
        }
    }
}
