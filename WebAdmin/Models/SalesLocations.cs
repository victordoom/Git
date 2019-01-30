using System;
using System.Collections.Generic;

namespace WebAdmin.Models
{
    public partial class SalesLocations
    {
        public SalesLocations()
        {
            Cases = new HashSet<Cases>();
        }

        public int LocationId { get; set; }
        public int CompanyId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string TaxId { get; set; }
        public int? ContactOwnerId { get; set; }
        public DateTime? ContactBirthday { get; set; }
        public string ContactName { get; set; }
        public string WebPage { get; set; }
        public string Email { get; set; }
        public string EmailMerchant { get; set; }
        public string DbaAddress { get; set; }
        public string DbaName { get; set; }
        public string LegalName { get; set; }
        public string DbaCity { get; set; }
        public string DbaState { get; set; }
        public string DbaZipCode { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }

        public SalesCompany Company { get; set; }
        public ICollection<Cases> Cases { get; set; }
    }
}
