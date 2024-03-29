﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AttendanzApi.Dtos
{
    public class SubjectDto
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string SemesterType { get; set; }

        [Required]
        public int Semester { get; set; }
    }
}
