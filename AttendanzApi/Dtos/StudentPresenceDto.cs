using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanzApi.Dtos
{
    public class StudentPresenceDto : PresenceDto
    {
        public ClassDto Class { get; set; }
    }
}
