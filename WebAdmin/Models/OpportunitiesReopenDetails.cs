using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class OpportunitiesReopenDetails
    {
        public int ReOpenID { get; set; }
        public int ReOpenBy { get; set; }
        public string ReOpenComment { get; set; }
        public DateTime ReOpenDate { get; set; }
        public int OpportunitiesID { get; set; }
        public string ClosedComment { get; set; }
        public DateTime ClosedDate { get; set; }
        public int ClosedBy { get; set; }
        public int ClosingReasonID { get; set; }
    }
}
