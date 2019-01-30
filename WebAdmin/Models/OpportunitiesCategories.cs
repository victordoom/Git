using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class OpportunitiesCategories
    {   [Key]
        public int CategoryID { get; set; }
        public string CategoryDescription { get; set; }
    }
}
