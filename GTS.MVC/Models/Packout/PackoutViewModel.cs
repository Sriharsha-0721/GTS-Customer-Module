using System.Collections.Generic;

namespace GTS.MVC.Models.Packout
{
    public class PackoutViewModel
    {
        public List<PackoutDetailsViewModel> PackoutDetails { get; set; } = new();

        public List<PackoutItemViewModel> Items { get; set; } = new();

        public string? PackoutCom { get; set; }
    }
}