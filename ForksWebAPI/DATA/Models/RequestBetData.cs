using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ForksWebAPI.DATA.Models
{
    public class RequestBetData
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Value { get; set; }


        [Required]
        public string CridId { get; set; }

        [Required]
        public string Key { get; set; }


        [Required]
        public DateTime DateCreation { get; set; }


    }
}
