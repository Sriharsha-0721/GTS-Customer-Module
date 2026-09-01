using System.Data;
using GTSCustomerAPI.Domain.Entities;
using GTSCustomerAPI.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GTSCustomerAPI.Infrastructure.Repositories;

public class CustomerProfileRepository
    : ICustomerProfileRepository
{
    private readonly string _cs;

    public CustomerProfileRepository(
        IConfiguration cfg)
    {
        _cs = cfg.GetConnectionString(
            "DefaultConnection")!;
    }

    public async Task<CustomerProfile?> GetByCustIdAsync(
        int custId)
    {
        using var con = new SqlConnection(_cs);
        using var cmd = new SqlCommand(
            "ClnRm_ReadCustProfile", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CustID", custId);

        await con.OpenAsync();
        using var r = await cmd.ExecuteReaderAsync();
        if (!await r.ReadAsync()) return null;

        return new CustomerProfile
        {
            CustId = G_Int(r, "CustId"),
            MarketCenter = G_Int(r, "MarketCenter"),
            CustNbr = G_Int(r, "CustNbr"),
            SterileCode = G_Int(r, "SterileCode"),
            CtmndFlg = G_Int(r, "CtmndFlg"),
            DelTicket = G_Int(r, "DelTicket"),
            OSSFlag = G_Bool(r, "OSSFlag"),
            PropertyMark = G_Str(r, "PropertyMark"),
            ShipVia = G_Str(r, "ShipVia"),
            Package = G_Str(r, "Package"),
            ProdCom = G_Str(r, "ProdCom"),
            OfficeCom = G_Str(r, "OfficeCom"),
            GenOfficeCom = G_Str(r, "GenOfficeCom"),
            MerControlCom = G_Str(r, "MerControlCom"),
            ReceivingCom = G_Str(r, "ReceivingCom"),
            SoilCom = G_Str(r, "SoilCom"),
            WashCom = G_Str(r, "WashCom"),
            DryerCom = G_Str(r, "DryerCom"),
            MainCleanRoomCom = G_Str(r, "MainCleanRoomCom"),
            PackoutCom = G_Str(r, "PackoutCom"),
            ShippingCom = G_Str(r, "ShippingCom"),
            DriverCom = G_Str(r, "DriverCom"),
            MendCom = G_Str(r, "MendCom"),
            QACom = G_Str(r, "QACom"),
            CustSrvCom = G_Str(r, "CustSrvCom"),
            BillingCom = G_Str(r, "BillingCom"),
            QAInspCom = G_Str(r, "QAInspCom")
        };
    }

    public async Task<bool> SaveAsync(
        CustomerProfile p, int userId)
    {
        using var con = new SqlConnection(_cs);
        using var cmd = new SqlCommand(
            "ClnRm_UpdateCustProfile", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue(
            "@CustID", p.CustId);
        cmd.Parameters.AddWithValue(
            "@SterileCode", p.SterileCode);
        cmd.Parameters.AddWithValue(
            "@CtmndFlg", p.CtmndFlg != 0);
        cmd.Parameters.AddWithValue(
            "@DelTicket", p.DelTicket != 0);
        cmd.Parameters.AddWithValue(
            "@PropertyMark", p.PropertyMark);
        cmd.Parameters.AddWithValue(
            "@ShipVia", p.ShipVia);
        cmd.Parameters.AddWithValue(
            "@Package", p.Package);
        cmd.Parameters.AddWithValue(
            "@ProdCom", p.ProdCom);
        cmd.Parameters.AddWithValue(
            "@OfficeCom", p.OfficeCom);
        cmd.Parameters.AddWithValue(
            "@GenOfficeCom", p.GenOfficeCom);
        cmd.Parameters.AddWithValue(
            "@MerControlCom", p.MerControlCom);
        cmd.Parameters.AddWithValue(
            "@ReceivingCom", p.ReceivingCom);
        cmd.Parameters.AddWithValue(
            "@SoilCom", p.SoilCom);
        cmd.Parameters.AddWithValue(
            "@WashCom", p.WashCom);
        cmd.Parameters.AddWithValue(
            "@DryerCom", p.DryerCom);
        cmd.Parameters.AddWithValue(
            "@MainCleanRoomCom", p.MainCleanRoomCom);
        cmd.Parameters.AddWithValue(
            "@PackoutCom", p.PackoutCom);
        cmd.Parameters.AddWithValue(
            "@ShippingCom", p.ShippingCom);
        cmd.Parameters.AddWithValue(
            "@DriverCom", p.DriverCom);
        cmd.Parameters.AddWithValue(
            "@MendCom", p.MendCom);
        cmd.Parameters.AddWithValue(
            "@QACom", p.QACom);
        cmd.Parameters.AddWithValue(
            "@CustSrvCom", p.CustSrvCom);
        cmd.Parameters.AddWithValue(
            "@BillingCom", p.BillingCom);
        cmd.Parameters.AddWithValue(
            "@UserID", userId);
        cmd.Parameters.AddWithValue(
            "@OSSFlag", p.OSSFlag);
        cmd.Parameters.AddWithValue(
            "@QAInspCom", p.QAInspCom);

        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
        return true;
    }

    private static string G_Str(
        SqlDataReader r, string col)
    {
        try
        {
            int i = r.GetOrdinal(col);
            return r.IsDBNull(i) ? "" : r.GetString(i);
        }
        catch { return ""; }
    }

    private static int G_Int(
        SqlDataReader r, string col)
    {
        try
        {
            int i = r.GetOrdinal(col);
            return r.IsDBNull(i)
                ? 0
                : Convert.ToInt32(r.GetValue(i));
        }
        catch { return 0; }
    }

    private static bool G_Bool(
        SqlDataReader r, string col)
    {
        try
        {
            int i = r.GetOrdinal(col);
            if (r.IsDBNull(i)) return false;
            var t = r.GetFieldType(i);
            if (t == typeof(bool))
                return r.GetBoolean(i);
            return Convert.ToInt32(
                r.GetValue(i)) != 0;
        }
        catch { return false; }
    }
}