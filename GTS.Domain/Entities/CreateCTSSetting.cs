using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTS.Domain.Entities
{
    public class CreateCTSSetting
    {
        public short MarketCenter { get; set; }

        public int CustNbr { get; set; }

        public short? PrintIssueStatusFlag { get; set; }

        public short? PrintBornonDateFlag { get; set; }

        public short? NOGFlag { get; set; }

        public string LabelHeader { get; set; } = "";
    }
}