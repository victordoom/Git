using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class OpportunitiesHowFound
    {
        [Key]
        public int HowFoundID { get; set; }
        public string HowFoundDescription { get; set; }
    }
}
