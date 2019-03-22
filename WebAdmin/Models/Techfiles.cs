using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class Techfiles
    {
        public Int64 IdRow { get; private set; }
        public string ContractID { get; private set; }
        public string CompanyName { get; private set; }
        public string DBA_name { get; private set; }
        public string CompanyAddress { get; private set; }
        public string StrFile { get; private set; }
        public string StrFileExt { get; private set; }
        
       
    }
}
