using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class Attachments
    {
        public int Idattachfile { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string Path { get; set; }
        public DateTime AttachDate { get; set; }
        public string Typeid { get; set; }
        public string StringId { get; set; }
        public int IntId { get; set; }
        public char TechnicalSupport { get; set; }

       
    }
}
