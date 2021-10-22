using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AttendanzApi.Interfaces;

namespace AttendanzApi.Models
{
    [Table("group_students")]
    public class GroupStudentModel : IModel
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("group_id")]
        public long GroupId { get; set; }

        [Column("student_id")]
        public long StudentId { get; set; }

        public GroupModel Group { get; set; }
        public StudentModel Student { get; set; }
        public List<PresenceModel> Presences { get; set; }

    }
}
