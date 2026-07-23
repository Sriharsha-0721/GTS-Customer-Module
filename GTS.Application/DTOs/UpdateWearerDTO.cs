using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTS.Application.DTOs
{
    public class UpdateWearerDTO
    {
        public int WearerId { get; set; }
        public string Locker { get; set; } = "";
        public string LockRm { get; set; } = "";
        public bool Sex { get; set; }
    }
}
