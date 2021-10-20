using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AttendanzApi.Interfaces;
using AttendanzApi.Models;
using AttendanzApi.Dtos;
using AutoMapper;

namespace AttendanzApi.Controllers
{
    [Route("account/groups")]
    [ApiController]
    public class PresencesController : ControllerBase
    {
        private readonly ILogger<GroupsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<GroupStudentModel> _groupStudents;
        private readonly IRepository<PresenceModel> _presences;
        private readonly IRepository<ClassModel> _classes;

        public PresencesController(
            ILogger<GroupsController> logger,
            IMapper mapper,
            IRepository<GroupStudentModel> groupStudents,
            IRepository<PresenceModel> presences,
            IRepository<ClassModel> classes)
        {
            _logger = logger;
            _mapper = mapper;
            _groupStudents = groupStudents;
            _presences = presences;
            _classes = classes;
        }

        [HttpGet]
        [Route("{groupId}/students/{studentId}/presences")]
        public IActionResult GetStudentPresences(long groupId, long studentId)
        {
            var groupStudent = _groupStudents.FirstOrDefault
            (
                (student => student.StudentId == studentId && student.GroupId == groupId), 
                p => p.Presences
            );

            if (groupStudent == null)
                return NotFound();

            var extended = groupStudent.Presences
                .Select(presence => _presences.GetById(presence.Id, p => p.Class));

            var dtos = extended.Select(presence => _mapper.Map(presence, new StudentPresenceDto()));

            return Ok(dtos);
        }


        [HttpGet]
        [Route("{groupId}/classes/{classId}/presences")]
        public IActionResult GetClassPresences(long groupId, long classId)
        {
            var classModel = _classes.FirstOrDefault
            (
                (classModel => classModel.Id == classId && classModel.GroupId == groupId),
                p => p.Presences
            );

            if (classModel == null)
                return NotFound();

            var extended = classModel.Presences
                .Select(presence => _presences.GetById(presence.Id, p => p.GroupStudent.Student));

            var dtos = extended.Select(presence => _mapper.Map(presence, new ClassPresenceDto()));

            return Ok(dtos);
        }


        [HttpPatch]
        [Route("{groupId}/students/{studentId}/presences/{presenceId}")]
        public IActionResult PatchStudentPresence(
            long groupId, 
            long studentId, 
            long presenceId, 
            [FromBody] PresenceDto dto)
        {
            var presence = _presences.GetById(presenceId, p => p.GroupStudent, p => p.Class);
            if (presence == null)
                return NotFound();

            if (presence.GroupStudent.StudentId != studentId || presence.GroupStudent.GroupId != groupId)
                return NotFound();

            _logger.LogInformation(presence.GroupStudent.Id.ToString());
            _logger.LogInformation(presence.GroupStudentId.ToString());
            //_mapper.Map(dto, presence);
            presence.Status = dto.Status;
            _logger.LogInformation(presence.GroupStudent.Id.ToString());
            _logger.LogInformation(presence.GroupStudentId.ToString());
            _presences.Update(presence);

            return Ok(_mapper.Map(presence, new StudentPresenceDto()));
        }

        [HttpPatch]
        [Route("{groupId}/classes/{classId}/presences/{presenceId}")]
        public IActionResult PatchClassPresence(
            long groupId,
            long classId,
            long presenceId,
            [FromBody] PresenceDto dto)
        {
            var presence = _presences.GetById(presenceId, p => p.GroupStudent.Student, p => p.Class);
            if (presence == null)
                return NotFound();

            if (presence.ClassId != classId || presence.GroupStudent.GroupId != groupId)
                return NotFound();

            _logger.LogInformation(presence.GroupStudent.Id.ToString());
            _logger.LogInformation(presence.GroupStudentId.ToString());
            //_mapper.Map(dto, presence);
            presence.Status = dto.Status;
            _logger.LogInformation(presence.GroupStudent.Id.ToString());
            _logger.LogInformation(presence.GroupStudentId.ToString());
            _presences.Update(presence);

            return Ok(_mapper.Map(presence, new ClassPresenceDto()));
        }



    }
}
