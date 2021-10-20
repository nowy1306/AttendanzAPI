using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanzApi.Dtos
{
    public class ScannerDto
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsBlocked { get; set; }
    }
}
