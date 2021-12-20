using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AttendanzApi.Dtos;
using AttendanzApi.Interfaces;
using AttendanzApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using AttendanzApi.Hubs;
using AttendanzApi.Extensions;

namespace AttendanzApi.Controllers
{
    [ApiController]
    public class ControlProcessesController : ControllerBase
    {
        private readonly ILogger<ControlProcessesController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<ControlProcessModel> _processes;
        private readonly IRepository<GroupModel> _groups;
        private readonly IRepository<ScannerModel> _scanners;
        private readonly IRepository<AccountModel> _accounts;
        private readonly IRepository<ClassModel> _classes;
        private readonly IHubContext<ControllProcessHub> _hubContext;

        public ControlProcessesController(
            ILogger<ControlProcessesController> logger,
            IMapper mapper,
            IRepository<ControlProcessModel> processes,
            IRepository<ScannerModel> scanners,
            IRepository<AccountModel> accounts,
            IRepository<GroupModel> groups,
            IRepository<ClassModel> classes,
            IHubContext<ControllProcessHub> hubContext)
        {
            _logger = logger;
            _mapper = mapper;
            _processes = processes;
            _scanners = scanners;
            _accounts = accounts;
            _groups = groups;
            _classes = classes;
            _hubContext = hubContext;

        }

        [HttpPost]
        [Route("account/groups/{groupId}/classes/{classId}/control-process")]
        public IActionResult Post(long groupId, long classId, [FromBody] ControlProcessDto dto)
        {
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var classModel = _classes.FirstOrDefault(
                classModel =>
                    classModel.Id == classId &&
                    classModel.GroupId == groupId &&
                    classModel.Group.AccountId == accountId,
                p => p.Group
            );

            if (classModel == null)
                return NotFound();

            var process = GetControlProcess((long) accountId);
            if (process != null)
                return Conflict();

            process = _mapper.Map(dto, new ControlProcessModel());
            process.Class = classModel;
            
            _processes.Insert(process);

            return Ok(_mapper.Map(process, new ControlProcessDto()));
        }

        

        [HttpPut]
        [Route("account/groups/{groupId}/classes/{classId}/control-process")]
        public IActionResult Patch(long groupId, long classId, [FromBody] ControlProcessDto dto)
        {
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var classModel = _classes.FirstOrDefault(
                classModel =>
                    classModel.Id == classId &&
                    classModel.GroupId == groupId &&
                    classModel.Group.AccountId == accountId,
                p => p.Group,
                p => p.ControlProcess
            );

            if (classModel == null || classModel.ControlProcess == null)
                return NotFound();

            var process = classModel.ControlProcess;
            _mapper.Map(dto, process);

            _processes.Update(process);

            return Ok(_mapper.Map(process, new ControlProcessDto()));
        }

        [HttpDelete]
        [Route("account/groups/{groupId}/classes/{classId}/control-process")]
        public IActionResult Delete(long groupId, long classId)
        {
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var classModel = _classes.FirstOrDefault(
                classModel =>
                    classModel.Id == classId &&
                    classModel.GroupId == groupId &&
                    classModel.Group.AccountId == accountId,
                p => p.Group,
                p => p.ControlProcess
            );

            if (classModel == null || classModel.ControlProcess == null)
                return NotFound();

            _processes.Delete(classModel.ControlProcess);

            return Ok();
        }

        [HttpPut]
        [Route("/control-processes")]
        public IActionResult GetForScanner([FromQuery] string cardCode, [FromQuery] string scannerKey)
        {
            if (cardCode == null || scannerKey == null)
                return UnprocessableEntity();

            var scanner = _scanners.FirstOrDefault(scanner => scanner.Key == scannerKey);

            if (scanner == null)
                return NotFound();

            if (scanner.IsBlocked)
                return Unauthorized();

            var account = _accounts.FirstOrDefault(account => account.CardCode == cardCode, p => p.Groups);
            if (account == null)
                return NotFound();

            var process = GetControlProcess(account.Id);
            if (process == null)
                return NotFound();
            process.Scanner = scanner;
            _processes.Update(process);

            SendScannerInfo(account.CardCode, scanner.Description);


            return Ok(new ControlProcessInfoDto()
            {
                ClassId = process.ClassId,
                GroupId = process.Class.GroupId,
                ScannerId = scanner.Id,
                ControlMode = process.ControlMode
            }) ;

        }

        [HttpGet]
        [Route("account/control-process")]
        public IActionResult GetForAccount()
        {
            var accountId = HttpContext.Session.GetLong(SessionKeys.AccountId);
            if (accountId == null)
                return Unauthorized();

            var account = _accounts.GetById((long)accountId, p => p.Groups);
            if (account == null)
                return NotFound();

            var process = GetControlProcess(account.Id);
            if (process == null)
                return NotFound();

            return Ok(new ControlProcessInfoDto()
            {
                ClassId = process.ClassId,
                GroupId = process.Class.GroupId,
                ScannerId = process.ScannerId,
                ControlMode = process.ControlMode
            }) ;
        }

        [HttpPut]
        [Route("/groups/{groupId}/classes/{classId}/presences")]
        public IActionResult PutPresence(
            long groupId, 
            long classId, 
            [FromQuery] string studentCardCode, 
            [FromQuery] string scannerKey)
        {
            if (studentCardCode == null || scannerKey == null)
                return UnprocessableEntity();

            var classModel = _classes.GetById(
                classId, 
                p => p.Group.GroupStudents.Where(groupStudent => groupStudent.Student.StudentCardCode == studentCardCode), 
                p => p.Presences, 
                p => p.ControlProcess,
                p => p.Group.Account);

            var groupStudent = classModel.Group.GroupStudents.FirstOrDefault();
            if (classModel == null || classModel.GroupId != groupId || groupStudent == null || classModel.ControlProcess == null)
                return NotFound();

            var scanner = _processes.GetById(classModel.ControlProcess.Id, p => p.Scanner).Scanner;

            if (scanner.IsBlocked)
                return Unauthorized();

            if (scanner == null)
                return NotFound();

            
            if (scanner.Key != scannerKey)
                return Conflict();

            var presence = classModel.Presences
                .FirstOrDefault(presence => presence.GroupStudentId == groupStudent.Id);

            presence.Status = classModel.ControlProcess.ControlMode == "presence" ? "present" : "late";
            _classes.Update(classModel);

            SendStudentPresence(classModel.Group.Account.CardCode, presence.Id, presence.Status);

            return Ok(_mapper.Map(classModel, new ClassDto()));
        }

        private ControlProcessModel GetControlProcess(long accountId)
        { 
            var processes = _processes.GetAll(p => p.Class.Group.Account);

            foreach (var process in processes)
                if (process.Class.Group.AccountId == accountId)
                    return process;

            return null;
            
        }

        private void SendScannerInfo(string cardCode, string scannerDescription)
        {
            _hubContext.Clients.Client(ConnectionStorage.GetConnectionId(cardCode))
                .SendAsync("ReceiveScannerInfo", scannerDescription);
        }

        private void SendStudentPresence(string cardCode, long presenceId, string presenceStatus)
        {
            _hubContext.Clients.Client(ConnectionStorage.GetConnectionId(cardCode))
                .SendAsync("ReceiveStudentPresence", presenceId, presenceStatus);
        }


    }
}
