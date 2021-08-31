using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForksWebAPI.DATA.Models
{
    public class UserDB
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Hash { get; set; }


        public string Roles { get; set; }


        public bool IsActive { get; set; }


    }
}
