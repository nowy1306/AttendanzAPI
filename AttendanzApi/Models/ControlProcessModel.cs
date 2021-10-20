using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanzApi.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanzApi.Models
{
    [Table("control_processes")]
    public class ControlProcessModel : IModel
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("class_id")]
        public long ClassId { get; set; }

        [Column("scanner_id")]
        public long? ScannerId { get; set; }

        [Column("control_mode")]
        public string ControlMode { get; set; }

        public ClassModel Class { get; set; }
        public ScannerModel Scanner { get; set; }
    }
}
