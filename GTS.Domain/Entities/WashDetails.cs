using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTS.Domain.Entities
{
    public class WashDetails
    {
        public int CustId { get; set; }

        public string WashCom { get; set; } = "";

        public string Formula { get; set; } = "";
    }
}
