using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class SalesComments
    {
        public int CommentId { get; set; }
        public int? SalesId { get; set; }
        public int? CommentBy { get; set; }
        public DateTime? CommentDatetime { get; set; }
        public string Comment { get; set; }
        public string Title { get; set; }
        public string Nombre { get; set; }
    }
}
