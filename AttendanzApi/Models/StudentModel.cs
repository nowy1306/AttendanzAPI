using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanzApi.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanzApi.Models
{
    [Table("students")]
    public class StudentModel : IModel
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("firstname")]
        public string Firstname { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("index_number")]
        public string IndexNumber { get; set; }

        public List<GroupStudentModel> GroupStudents { get; set; }
    }
}
