using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTS.Domain.Entities
{
    public class Wearer
    {
        public int WearerId { get; set; }
        public int CustId { get; set; }
        public string WearNbr { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Locker { get; set; } = string.Empty;
        public string LockRm { get; set; } = string.Empty;
        public bool Sex { get; set; }   // false = Female, true = Male
    }
}