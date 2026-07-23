using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTS.Domain.Entities
{
    public class CustLineCmts
    {
        public int CustId { get; set; }

        public int WearItemId { get; set; }

        public string Descr { get; set; } = string.Empty;
    }
}