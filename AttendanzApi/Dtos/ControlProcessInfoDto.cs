using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanzApi.Dtos
{
    public class ControlProcessInfoDto
    {
        public long GroupId { get; set; }
        public long ClassId { get; set; }
        public long? ScannerId { get; set; }
        public string ControlMode { get; set; }
    }
}
