using System.ComponentModel.DataAnnotations;

namespace GTS.Application.DTOs
{
    public class UpdateCustomerDTO
    {
        [Required]
        public int CustId { get; set; }

        [Required]
        public short MarketCenter { get; set; }

        [Required]
        public int CustNbr { get; set; }

        public int? Account { get; set; }
        public int? Dept { get; set; }

        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        public int Route { get; set; }

        [Required]
        [StringLength(9)]
        public string GID { get; set; } = null!;

        public DateTime? StopDt { get; set; }

        public int MonSeq { get; set; }
        public int TueSeq { get; set; }
        public int WedSeq { get; set; }
        public int ThuSeq { get; set; }
        public int FriSeq { get; set; }
        public int SatSeq { get; set; }
        public int SunSeq { get; set; }

        [StringLength(50)]
        public string? Addr1 { get; set; }

        [StringLength(50)]
        public string? Addr2 { get; set; }

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(50)]
        public string? State { get; set; }

        [StringLength(50)]
        public string? Zip { get; set; }

        [StringLength(50)]
        public string? Phone { get; set; }

        [StringLength(50)]
        public string? Fax { get; set; }

        [StringLength(50)]
        public string? Freq { get; set; }

        public bool OSSFlag { get; set; }
        public bool? SoilFlag { get; set; }
        public bool? LRFlag { get; set; }
        public bool? STFlag { get; set; }

        [StringLength(1)]
        public string? InvSeq { get; set; }

        public int? MastAcctNbr { get; set; }

        [StringLength(20)]
        public string? NatAcctNbr { get; set; }

        public bool? PrepFlag { get; set; }
        public bool? NameFlag { get; set; }
        public bool? ProdFlag { get; set; }
        public bool? EmbrFlag { get; set; }

        public DateTime? CreateDt { get; set; }

        [StringLength(50)]
        public string? PONumber { get; set; }

        public byte? SterileCode { get; set; }
        public bool? CtmndFlg { get; set; }
        public bool? DelTicket { get; set; }

        [StringLength(50)]
        public string? PropertyMark { get; set; }

        [StringLength(30)]
        public string? ShipVia { get; set; }

        [StringLength(20)]
        public string? Package { get; set; }

        [StringLength(25)]
        public string? Contact { get; set; }

        [StringLength(30)]
        public string? BillName { get; set; }

        [StringLength(30)]
        public string? BillAddr { get; set; }

        [StringLength(30)]
        public string? BillExAddr { get; set; }

        [StringLength(20)]
        public string? BillCity { get; set; }

        [StringLength(2)]
        public string? BillState { get; set; }

        [StringLength(50)]
        public string? BillZipCod { get; set; }

        [StringLength(10)]
        public string? BillPhone { get; set; }

        public short? CntnrsIn { get; set; }
        public short? CntnrsOut { get; set; }

        [Required]
        public int UpdtUser { get; set; }

        [Required]
        public DateTime UpdtTime { get; set; }
    }
}