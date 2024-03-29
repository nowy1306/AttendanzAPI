﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanzApi.Dtos
{
    public class AccountDto
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string CardCode { get; set; }
        public bool IsAdmin { get; set; }
    }
}
