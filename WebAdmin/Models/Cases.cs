using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebAdmin.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    public partial class Cases
    {

        //public Cases()
        //{
        //    CasesDetails = new List<CasesDetails>();
        //}
        [Key]
        public int CasesID { get; set; }
        [Required(ErrorMessage = "Title is needed.")]
        public string Title { get; set; }
        [Display(Name = "Assigned To")]
        public int? AssignedTo { get; set; }
        public int? Caller { get; set; }
        [Display(Name = "Opened By")]
        public int? OpenedBy { get; set; }

        [Display(Name = "Opened Date")]
        [DataType(DataType.Date)]
        public DateTime? OpenedDate { get; set; }
        public int? ContactName { get; set; }
        public string Status { get; set; }
        [Required(ErrorMessage = "Category is needed.")]
        public string Category { get; set; }
        public string Priority { get; set; }
        [Required(ErrorMessage = "Summary is needed.")]
        public string Description { get; set; }
    
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        internal static object FindDataUser(string nombre)
        {
            throw new NotImplementedException();
        }

        public string Comments { get; set; }
        [Display(Name = "Resolved Date")]
        [DataType(DataType.Date)]
        public DateTime? ResolvedDate { get; set; }
        public string RelatedCases { get; set; }
        public int? Kb { get; set; }
        public string Attachments { get; set; }
        [Display(Name = "Contract ID")]
        public string ContractId { get; set; }
        public int? OpenedByOld { get; set; }
        public int? AssignedToOld { get; set; }
        [Display(Name = "Company")]
        public int? CompanyId { get; set; }
        [Display(Name = "Location")]
        public int? LocationId { get; set; }
        public int? IssueTypeId { get; set; }
        [Required(ErrorMessage = "Caller Name is needed.")]
        [Display(Name = "Caller Name")]
        public string CallerName { get; set; }
        [Required(ErrorMessage = "Phone Number is needed.")]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public string CallerPhone { get; set; }
        [Required(ErrorMessage = "Tittle is needed.")]
        [Display(Name = "Tittle")]
        public string CallerTitle { get; set; }
        [Display(Name = "Email Address")]
        //[Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string CallerEmail { get; set; }
        public string AssignedToSendEmail { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string TypeId { get; set; }

        public string Web { get; set; }
        public string LastComment { get; set; }

        public string FileName { get; set; }
        public string FileType { get; set; }

        public SegUsuarios AssignedToNavigation { get; set; }
        public SalesCompany Company { get; set; }
        public SalesLocations Location { get; set; }
        
        public virtual Collection<CasesDetails> CasesDetails { get; set; }
        ////   }

    }
}
