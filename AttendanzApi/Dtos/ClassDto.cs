using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AttendanzApi.Dtos
{
    public class ClassDto
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
    }
}
