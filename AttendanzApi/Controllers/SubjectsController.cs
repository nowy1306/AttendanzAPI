using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanzApi.Dtos;
using AttendanzApi.Models;
using AttendanzApi.Interfaces;
using AutoMapper;

namespace AttendanzApi.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class SubjectsController : ControllerBase
    {
        private readonly ILogger<SubjectsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<SubjectModel> _subjects;

        public SubjectsController(
            ILogger<SubjectsController> logger,
            IMapper mapper,
            IRepository<SubjectModel> subjects)
        {
            _logger = logger;
            _mapper = mapper;
            _subjects = subjects;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(long id)
        {
            var subject = _subjects.GetById(id);
            if (subject == null)
                return NotFound();

            var dto = _mapper.Map(subject, new SubjectDto());

            return Ok(dto);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var subjects = _subjects.GetAll();
            var dtos = subjects.Select(subject => _mapper.Map(subject, new SubjectDto()));

            return Ok(dtos);
        }

        [HttpPost]
        public IActionResult Post([FromBody] SubjectDto dto)
        {
            var subject = _mapper.Map(dto, new SubjectModel());
            _subjects.Insert(subject);

            return Ok(_mapper.Map(subject, new SubjectDto()));
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put(long id, [FromBody] SubjectDto dto)
        {
            var subject = _subjects.GetById(id);
            if (subject == null)
                return NotFound();

            _mapper.Map(dto, subject);
            _subjects.Update(subject);

            return Ok(_mapper.Map(subject, new SubjectDto()));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(long id)
        {
            var subject = _subjects.GetById(id);
            if (subject == null)
                return NotFound();

            _subjects.Delete(subject);

            return Ok();
        }
    }
}
