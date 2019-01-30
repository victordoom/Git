using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class CompaniesLocations
    {
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public int LocationId { get; set; }
        public string sAddress { get; set; }
        public string phone { get; set; }
        public string dba_name { get; set; }
        public int hasopencases { get; set; }
        public string contractID { get; set; }
        public int HasTechnicalSpp { get; set; }
    }
}
