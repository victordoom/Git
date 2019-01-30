using System;
using System.Collections.Generic;

namespace WebAdmin.Models
{
    public partial class SalesCompany
    {
        public SalesCompany()
        {
            Cases = new HashSet<Cases>();
            SalesLocations = new HashSet<SalesLocations>();
        }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int? OwnerId { get; set; }

        public ICollection<Cases> Cases { get; set; }
        public ICollection<SalesLocations> SalesLocations { get; set; }
    }
}
