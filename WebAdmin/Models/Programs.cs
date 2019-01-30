using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class Programs
    {   [Key]
        public int ProgramID { get; set; }
        public string ProgramShortName { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

    }
}
