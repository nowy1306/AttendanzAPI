using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanzApi.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanzApi.Models
{
    [Table("accounts")]
    public class AccountModel : IModel
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("card_code")]
        public string CardCode { get; set; }

        [Column("is_admin")]
        public bool IsAdmin { get; set; }

        public List<GroupModel> Groups { get; set; }
    }
}
