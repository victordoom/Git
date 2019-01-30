using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    public class Vehicles
    {
        [Key]
        public int VehicleID { get; set; }
        public string VehicleName { get; set; }
    }
}
