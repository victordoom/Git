using System;
using System.Collections.Generic;

namespace WebAdmin.Models
{
    public partial class CasesCategories
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string IncomeExpense { get; set; }
        public bool? Taxable { get; set; }
    }
}
