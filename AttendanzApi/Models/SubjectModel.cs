using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanzApi.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanzApi.Models
{
    [Table("subjects")]
    public class SubjectModel : IModel
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("semester_type")]
        public string SemesterType { get; set; }

        [Column("semester")]
        public int Semester { get; set; }

        public List<GroupModel> Groups { get; set; }
    }
}
