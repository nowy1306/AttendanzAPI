using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanzApi.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanzApi.Models
{
    [Table("presences")]
    public class PresenceModel : IModel
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("group_student_id")]
        public long GroupStudentId { get; set; }

        [Column("class_id")]
        public long ClassId { get; set; }

        [Column("status")]
        public string Status { get; set; }

        public GroupStudentModel GroupStudent { get; set; }
        public ClassModel Class { get; set; }
    }
}
