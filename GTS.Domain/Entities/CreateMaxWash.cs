using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTS.Domain.Entities
{
    public class CreateMaxWash
    {
        public int CustId { get; set; }

        public string ItemCode { get; set; } = string.Empty;

        // Add these two
        public string OldItemCode { get; set; } = string.Empty;

        public string NewItemCode { get; set; } = string.Empty;

        public int MaxWash { get; set; }

        public int MaxWeeks { get; set; }

        public int MaxCycles { get; set; }
    }
}