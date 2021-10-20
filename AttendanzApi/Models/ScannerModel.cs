using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanzApi.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanzApi.Models
{
    [Table("scanners")]
    public class ScannerModel : IModel
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("_key")]
        public string Key { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("is_confirmed")]
        public bool IsConfirmed { get; set; }

        [Column("is_blocked")]
        public bool IsBlocked { get; set; }

        public ControlProcessModel ControlProcess { get; set; }
    }
}
