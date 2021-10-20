using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanzApi.Dtos;
using AttendanzApi.Models;
using AttendanzApi.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AttendanzApi.Controllers
{
    [Route("account/groups")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly ILogger<GroupsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<GroupModel> _groups;
        private readonly IRepository<ClassModel> _classes;

        public ClassesController(
            ILogger<GroupsController> logger,
            IMapper mapper,
            IRepository<GroupModel> groups,
            IRepository<ClassModel> classes)
        {
            _logger = logger;
            _mapper = mapper;
            _groups = groups;
            _classes = classes;
        }

        [HttpGet]
        [Route("{groupId}/classes/{classId}")]
        public IActionResult Get(long groupId, long classId)
        {
            var classModel = _classes.GetById(classId);
            if (classModel == null)
                return NotFound();

            if (classModel.GroupId != groupId)
                return NotFound();

            var dto = _mapper.Map(classModel, new ClassDto());

            return Ok(dto);
        }

        [HttpGet]
        [Route("{groupId}/classes")]
        public IActionResult GetAll(long groupId)
        {
            var group = _groups.GetById(groupId, p => p.Classes);
            if (group == null)
                return NotFound();

            var dtos = group.Classes.Select(classModel => _mapper.Map(classModel, new ClassDto()));

            return Ok(dtos);
        }

        [HttpPost]
        [Route("{groupId}/classes")]
        public IActionResult Post(long groupId, [FromBody] ClassDto dto)
        {
            var classModel = _mapper.Map(dto, new ClassModel());
            classModel.GroupId = groupId;
            classModel.Date = DateTime.Now;

            var students = _groups
                .GetById(groupId, p => p.GroupStudents).GroupStudents;

            var presences = students.Select(student => new PresenceModel()
            {
                GroupStudent = student,
                Status = "absent"
            });
            classModel.Presences = Enumerable.ToList(presences);

            _classes.Insert(classModel);

            return Ok(_mapper.Map(classModel, new ClassDto()));
        }

        [HttpPatch]
        [Route("{groupId}/classes/{classId}")]
        public IActionResult Put(long groupId, long classId, [FromBody] ClassDto dto)
        {
            var classModel = _classes.GetById(classId);
            if (classModel == null)
                return NotFound();

            if (classModel.GroupId != groupId)
                return NotFound();

            _mapper.Map(dto, classModel);
            _classes.Update(classModel);

            return Ok(_mapper.Map(classModel, new ClassDto()));
        }

        [HttpDelete]
        [Route("{groupId}/classes/{classId}")]
        public IActionResult Delete(long groupId, long classId)
        {
            var classModel = _classes.GetById(classId);
            if (classModel == null)
                return NotFound();

            if (classModel.GroupId != groupId)
                return NotFound();

            _classes.Delete(classModel);

            return Ok();
        }


    }
}
