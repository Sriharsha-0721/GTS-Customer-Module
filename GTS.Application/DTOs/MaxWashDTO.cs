using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTS.Application.DTOs
{
    public class MaxWashDTO
    {
        public int CustId { get; set; }

        public string ItemCode { get; set; } = string.Empty;

        public string OldItemCode { get; set; } = string.Empty;

        public string NewItemCode { get; set; } = string.Empty;

        public int MaxWash { get; set; }

        public int MaxWeeks { get; set; }

        public int MaxCycles { get; set; }
    }
}