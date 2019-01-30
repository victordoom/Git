using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace WebAdmin.Models
{
    public partial class CasesDetails
    {
        [Key]
        public int DetailId { get; set; }
        public int UserID { get; set; }


        public DateTime? DetailDatetime { get; set; }

        [Required(ErrorMessage = "Comment is needed.")]
        public string Comment { get; set; }
        
        public string Status { get; set; }
        public string Suggestion { get; set; }
        public string NormalRow { get; set; }

        public int CasesID { get; set; }
        public virtual Cases Cases { get; set; }

        public virtual SegUsuarios SegUsuarios { get; set; }

        //public virtual Collection<SegUsuarios> SegUsuarios { get; set; }


    }
}
