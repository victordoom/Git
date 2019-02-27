using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class Opportunities
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Business Name")]
        [Required(ErrorMessage = "Business Name is needed.")]
        public string CompanyName { get; set; }
        //public int Employee  { get; set; }
        public string Category { get; set; }

        [Required(ErrorMessage = "Rating is needed.")]
        public string Rating { get; set; }

        public string HowFound { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Open Date")]
        public DateTime? OpenDate { get; set; }
        public double Probability { get; set; }

        [Display(Name = "Est Revenue")]
        public Decimal? EstRevenue { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Est Return")]
        public DateTime? EstClosedDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public string USState { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is needed.")]
        public string Address { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "State is needed.")]
        public string State { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is needed.")]
        public string City { get; set; }

        [Display(Name = "ZipCode")]
        [Required(ErrorMessage = "ZipCode is needed.")]
        public string ZipCode { get; set; }
        [Display(Name = "Owner Name")]
        public string OwnerName { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Visited Date")]
        public DateTime? VisitedDate { get; set; }

        [Display(Name = "Registration By")]
        public int UserID { get; set; }

        [Display(Name = "Category")]
        public int CategoryID { get; set; }

        [Display(Name = "POS Program")]
        public int ProgramID { get; set; }
        public string POSProgramAssigned { get; set; }

        [Required(ErrorMessage = "How Found is needed.")]
        [Display(Name = "How Found")]
        public int HowFoundID { get; set; }

        [Display(Name = "Lead #")]
        public string NumberLeadToFollowUp { get; set; }

        [Display(Name = "Email Address")]
        //[Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }
        public string Web { get; set; }

        public string LastComment { get; set; }


        //closed opportunities
        public DateTime? ClosedDate { get; set; }
        public int? ClosedBy { get; set; }
        public string ClosedComment { get; set; }
        public bool Closed { get; set; }
       


        public virtual Collection<OpportunitiesDetails> OpportunitiesDetails { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public string UrlRating
        //{
        //    get { return UrlRating + "; " + UrlRating; }
        //    private set
        //    {
        //        //Just need this here to trick EF
        //    }
        //}
    }
}

