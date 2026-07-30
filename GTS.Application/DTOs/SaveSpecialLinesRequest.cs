using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTS.Application.DTOs
{
    public class SaveSpecialLinesRequest
    {
        public int CustId { get; set; }

        public List<int> Lines { get; set; } = new();
    }
}
