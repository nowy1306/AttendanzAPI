using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanzApi.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanzApi.Models
{
    [Table("classes")]
    public class ClassModel : IModel
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("group_id")]
        public long GroupId { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("note")]
        public string Note { get; set; }

        public GroupModel Group { get; set; }
        public ControlProcessModel ControlProcess { get; set; }
        public List<PresenceModel> Presences { get; set; }
    }
}
