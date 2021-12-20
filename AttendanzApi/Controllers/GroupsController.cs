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
using AttendanzApi.Extensions;

namespace AttendanzApi.Controllers
{
    [ApiController]
    [Route("account/[controller]")]
    public class GroupsController : ControllerBase
    {

        private readonly ILogger<GroupsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<AccountModel> _accounts;
        private readonly IRepository<GroupModel> _groups;
        private readonly IRepository<SubjectModel> _subjects;
        private readonly IRepository<StudentModel> _students;
        private readonly IRepository<GroupStudentModel> _groupStudents;

        public GroupsController(
            ILogger<GroupsController> logger,
            IMapper mapper,
            IRepository<AccountModel> accounts,
            IRepository<GroupModel> groups,
            IRepository<SubjectModel> subjects,
            IRepository<StudentModel> students,
            IRepository<GroupStudentModel> groupStudents)
        {
            _logger = logger;
            _mapper = mapper;
            _accounts = accounts;
            _groups = groups;
            _subjects = subjects;
            _students = students;
            _groupStudents = groupStudents;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(long id)
        {
            var group = _groups.GetById(id);
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            if (group?.AccountId != accountId)
                return NotFound();

            var dto = _mapper.Map(group, new GroupDto());

            return Ok(dto);
            
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var account = _accounts.GetById((long)accountId, p => p.Groups);
            if (account == null)
                return NotFound();

            var dtos = account.Groups.Select(group => _mapper.Map(group, new GroupDto()));

            return Ok(dtos);
        }

        [HttpPost]
        public IActionResult Post([FromBody] GroupDto dto)
        {
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var subject = _subjects.GetById(dto.SubjectId);
            if (subject == null)
                return NotFound();

            var group = _mapper.Map(dto, new GroupModel() { AccountId = (long)accountId});
            _groups.Insert(group);

            return Ok(_mapper.Map(group, new GroupDto()));
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Put(long id, [FromBody] GroupDto dto)
        {
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var group = _groups.GetById(id);
            if (group?.AccountId != accountId)
                return NotFound();

            var subject = _subjects.GetById(dto.SubjectId);
            if (subject == null)
                return NotFound();
            
            _mapper.Map(dto, group);
            _groups.Update(group);

            return Ok(_mapper.Map(group, new GroupDto()));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(long id)
        {
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var group = _groups.GetById(id);
            if (group?.AccountId != accountId)
                return NotFound();

            _groups.Delete(group);

            return Ok();
        }

        [HttpGet]
        [Route("{id}/subject")]
        public IActionResult GetSubject(long id)
        {
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var group = _groups.GetById(id, p => p.Subject);
            if (group?.AccountId != accountId)
                return NotFound();

            var subject = _mapper.Map(group.Subject, new SubjectDto());

            return Ok(subject);
        }
        
        [HttpGet]
        [Route("{id}/students")]
        public IActionResult GetStudents(long id)
        {
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var group = _groups.GetById(id, p => p.GroupStudents);
            if (group?.AccountId != accountId)
                return NotFound();

            var groupStudents = group.GroupStudents
                .Select(groupStudent => _groupStudents.GetById(groupStudent.Id, p => p.Student));

            var dtos = groupStudents
                .Select(groupStudent => _mapper.Map(groupStudent, new StudentDto()));

            return Ok(dtos);
        }

        [HttpPut]
        [Route("{groupId}/students/{studentId}")]
        public IActionResult AddStudentToGroup(long groupId, long studentId)
        {
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var group = _groups.GetById(groupId, p => p.GroupStudents, p => p.Classes);
            if (group?.AccountId != accountId)
                return NotFound();

            var student = _students.GetById(studentId);
            if (student == null)
                return NotFound();

            var duplicate = group.GroupStudents
                .FirstOrDefault(groupStudent => groupStudent.StudentId == studentId);
            if (duplicate != null)
                return Conflict();

            var groupStudent = new GroupStudentModel()
            {
                GroupId = groupId,
                StudentId = studentId,
            };
            var presences = group.Classes.Select(classModel => new PresenceModel()
            {
                Status = "absent",
                Class = classModel
            });
            groupStudent.Presences = Enumerable.ToList(presences);

            _groupStudents.Insert(groupStudent);

            return Ok(_mapper.Map(groupStudent, new StudentDto()));
        }

        [HttpDelete]
        [Route("{groupId}/students/{studentId}")]
        public IActionResult DeleteStudentFromGroup(long groupId, long studentId)
        {
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var group = _groups.GetById(groupId, p => p.GroupStudents);
            if (group?.AccountId != accountId)
                return NotFound();

            var student = _students.GetById(studentId);
            if (student == null)
                return NotFound();

            var deleted = group.GroupStudents.FirstOrDefault(student => student.StudentId == studentId);
            if (deleted == null)
                return NotFound();

            _groupStudents.Delete(deleted);

            return Ok();

        }
    }
}
