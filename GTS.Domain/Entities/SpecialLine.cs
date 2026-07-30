using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTS.Domain.Entities
{
    public class SpecialLine
    {
        public int? CustId { get; set; }

        public int Line { get; set; }

        public string ItemCode { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        public string Descr { get; set; } = string.Empty;
    }
}