﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanzApi.Dtos
{
    public class GroupStudentDto : StudentDto
    {
        public string StudentCardCode { get; set; }
    }
}
