using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AttendanzApi.Interfaces;

namespace AttendanzApi.Models
{
    [Table("_groups")]
    public class GroupModel : IModel
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("account_id")]
        public long AccountId { get; set; }

        [Column("subject_id")]
        public long SubjectId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("total_class_number")]
        public int TotalClassNumber { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("week_day")]
        public string WeekDay { get; set; }

        [Column("start_time")]
        public TimeSpan StartTime { get; set; }

        [Column("end_time")]
        public TimeSpan EndTime { get; set; }

        public AccountModel Account { get; set; }
        public SubjectModel Subject { get; set; }
        public List<ClassModel> Classes { get; set; }
        public List<GroupStudentModel> GroupStudents { get; set; }
    }
}
