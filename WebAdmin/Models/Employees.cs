using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class Employees
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmailAddress { get; set; }
        public string JobTitle { get; set; }
        public string BusinessPhone { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string FaxNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string ZippostalCode { get; set; }
        public string CountryRegion { get; set; }
        public string WebPage { get; set; }
        public string Notes { get; set; }
        public string Attachments { get; set; }
        public string Social { get; set; }
        public string TaxId { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Usuario { get; set; }
        public string Pass { get; set; }
        public bool? Admin { get; set; }
        public bool? Accounting { get; set; }
        public bool? Administration { get; set; }
        public bool? UsuariosData { get; set; }
        public bool? Salesdepartament { get; set; }
        public bool? Technicalsupport { get; set; }
        public bool? Inventory { get; set; }
        public bool? MostrarCintaOpciones { get; set; }
        public bool? ActivarShift { get; set; }
        public string OfficeCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastModification { get; set; }
        public int? DepartmentId { get; set; }
        public bool? Status { get; set; }
        public int? PositionId { get; set; }
    }
}
