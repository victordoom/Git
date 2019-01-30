using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class OpportunitiesDetails
    {
        [Key]

        public int DetailID { get; set; }
        public int OpportunitiesID { get; set; }
        public DateTime DetailDatetime { get; set; }

        [Required(ErrorMessage = "Comment is needed.")]
        public string Comment { get; set; }

        [Display(Name = "By")]
        public int UserID { get; set; }
        public string Status { get; set; }
        [Display(Name = "Visited Date")]
        [DataType(DataType.Date)]
        public DateTime VisitDate { get; set; }
        [Display(Name = "Next Visit")]
        [DataType(DataType.Date)]
        public DateTime? NextVisitDate { get; set; }
        [Display(Name = "Email Me")]
        public string EmailNotification { get; set; }
        public string EmailSent { get; set; }
        [Display(Name = "Created")]
        public DateTime CreatedDate { get; set; }

        public virtual Opportunities Opportunities { get; set; }


    }
}
