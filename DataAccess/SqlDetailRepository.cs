using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Dispetcher2.Class;
using System.Collections;

namespace Dispetcher2.DataAccess
{
    public class SqlDetail : Detail
    {

    }
    public class SqlDetailRepository : DetailRepository
    {
        IConfig config;
        List<SqlDetail> details;
        public SqlDetailRepository(IConfig config)
        {
            this.config = config;
        }
        public override IEnumerable GetList()
        {
            if (details == null) Load();
            return details;
        }
        public override IEnumerable<Detail> GetDetails()
        {
            if (details == null) Load();
            return details;
        }
        public override void Load()
        {
            LoadAllDetailView();
        }
        void LoadAllDetailView()
        {
            details = new List<SqlDetail>();
            using (var cn = new SqlConnection() { ConnectionString = config.ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn })
                {
                    cmd.CommandText = "SELECT * FROM [dbo].[AllDetailView]";
                    cmd.CommandType = CommandType.Text;
                    
                    cn.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            SqlDetail item = new SqlDetail();
                            details.Add(item);
                            item.OrderId = Converter.GetInt(r["FK_IdOrder"]);
                            item.NameType = Converter.GetString(r["NameType"]);
                            item.Position = Converter.GetInt(r["Position"]);
                            item.Shcm = Converter.GetString(r["ShcmDetail"]);
                            item.Name = Converter.GetString(r["NameDetail"]);
                            item.Amount = Converter.GetInt(r["AmountDetails"]);
                            item.AllPositionParent = Converter.GetString(r["AllPositionParent"]);
                            item.OrderDetailId = Converter.GetLong(r["PK_IdOrderDetail"]);
                            item.IdDetail = Converter.GetLong(r["FK_IdDetail"]);
                            item.IdLoodsman = Converter.GetLong(r["IdLoodsman"]);
                            item.PositionParent = Converter.GetInt(r["PositionParent"]);
                        }
                    }
                }
            }
        }
    }
}
