using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AttendanzApi.Dtos;
using AttendanzApi.Extensions;
using AttendanzApi.Models;
using AttendanzApi.Interfaces;
using Microsoft.Extensions.Logging;

namespace AttendanzApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepository<AccountModel> _accounts;

        public AccountsController(ILogger<AccountsController> logger, IMapper mapper, IRepository<AccountModel> accounts)
        {
            _mapper = mapper;
            _accounts = accounts;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AccountDto dto)
        {
            var account = _mapper.Map(dto, new AccountModel());
            _accounts.Insert(account);

            return Ok(_mapper.Map(account, new AccountDto()));
        }

        [HttpPost]
        [Route("session")]
        public IActionResult Login([FromBody] LoginRequestDto dto)
        {
            var account = _accounts.FirstOrDefault(account => account.Login == dto.Login);
            if (account == null)
                return NotFound();

            if (dto.Password != account.Password)
                return Unauthorized();

            HttpContext.Session.SetLong(SessionKeys.AccountId, account.Id);
            HttpContext.Session.SetString(SessionKeys.AccountCardCode, account.CardCode);
            HttpContext.Session.SetBool(SessionKeys.IsAdmin, account.IsAdmin);

            var resp = new LoginResponseDto()
            {
                IsAdmin = account.IsAdmin
            };
            return Ok(resp);
        }

        [HttpGet]
        [Route("session")]
        public IActionResult Get()
        {
            _logger.LogInformation(HttpContext.Session.GetLong(SessionKeys.AccountId).ToString());
            if (HttpContext.Session.GetLong(SessionKeys.AccountId) == null)
                return Unauthorized();

            return Ok();
        }

        [HttpDelete]
        [Route("session")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok();
        }
    }
}
