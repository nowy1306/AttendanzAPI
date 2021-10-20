using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanzApi.Dtos
{
    public class ClassPresenceDto : PresenceDto
    {
        public StudentDto Student { get; set; }
    }
}
