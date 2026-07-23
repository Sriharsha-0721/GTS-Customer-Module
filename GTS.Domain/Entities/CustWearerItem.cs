using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTS.Domain.Entities
{
    public class CustWearerItem
    {
        public int CustId { get; set; }

        public int WearItemId { get; set; }

        public int LineNbr { get; set; }

        public string Item { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        public int WearId { get; set; }

        public int WearNbr { get; set; }

        public string Locker { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string EmblCode { get; set; } = string.Empty;

        public string Descr { get; set; } = string.Empty;
    }
}
