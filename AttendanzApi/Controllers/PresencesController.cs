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
using AttendanzApi.Extensions;

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
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var groupStudent = _groupStudents.FirstOrDefault(
                student => 
                    student.StudentId == studentId && 
                    student.GroupId == groupId && 
                    student.Group.AccountId == accountId, 
                p => p.Presences,
                p => p.Group
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
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var classModel = _classes.FirstOrDefault(
                classModel => 
                    classModel.Id == classId && 
                    classModel.GroupId == groupId &&
                    classModel.Group.AccountId == accountId,
                p => p.Presences,
                p => p.Group
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

            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var presence = _presences.FirstOrDefault(
                presence =>
                    presence.Id == presenceId &&
                    presence.GroupStudent.StudentId == studentId &&
                    presence.GroupStudent.GroupId == groupId &&
                    presence.GroupStudent.Group.AccountId == accountId,
                p => p.GroupStudent,
                p => p.GroupStudent.Group
            );

            if (presence == null)
                return NotFound();

            //_mapper.Map(dto, presence);
            presence.Status = dto.Status;
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
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var presence = _presences.FirstOrDefault(
                presence =>
                    presence.Id == presenceId &&
                    presence.Class.Id == classId &&
                    presence.Class.GroupId == groupId &&
                    presence.Class.Group.AccountId == accountId,
                p => p.Class,
                p => p.Class.Group
            );

            if (presence == null)
                return NotFound();

            //_mapper.Map(dto, presence);
            presence.Status = dto.Status;
            _presences.Update(presence);

            return Ok(_mapper.Map(presence, new ClassPresenceDto()));
        }



    }
}
