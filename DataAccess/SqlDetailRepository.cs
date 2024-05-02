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
        IConverter converter;
        public SqlDetailRepository(IConfig config, IConverter converter)
        {
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр config");
            if (converter == null) throw new ArgumentException("Пожалуйста укажите параметр converter");
            this.config = config;
            this.converter = converter;
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

                            if (converter.CheckConvert<int>(r["FK_IdOrder"]))
                                item.OrderId = converter.Convert<int>(r["FK_IdOrder"]);

                            if (converter.CheckConvert<string>(r["NameType"]))
                                item.NameType = converter.Convert<string>(r["NameType"]);

                            if (converter.CheckConvert<int>(r["Position"]))
                                item.Position = converter.Convert<int>(r["Position"]);

                            if (converter.CheckConvert<string>(r["ShcmDetail"]))
                                item.Shcm = converter.Convert<string>(r["ShcmDetail"]);

                            if (converter.CheckConvert<string>(r["NameDetail"]))
                                item.Name = converter.Convert<string>(r["NameDetail"]);

                            if (converter.CheckConvert<int>(r["AmountDetails"]))
                                item.Amount = converter.Convert<int>(r["AmountDetails"]);

                            if (converter.CheckConvert<string>(r["AllPositionParent"]))
                                item.AllPositionParent = converter.Convert<string>(r["AllPositionParent"]);

                            if (converter.CheckConvert<long>(r["PK_IdOrderDetail"]))
                                item.OrderDetailId = converter.Convert<long>(r["PK_IdOrderDetail"]);

                            if (converter.CheckConvert<long>(r["FK_IdDetail"]))
                                item.IdDetail = converter.Convert<long>(r["FK_IdDetail"]);

                            if (converter.CheckConvert<long>(r["IdLoodsman"]))
                                item.IdLoodsman = converter.Convert<long>(r["IdLoodsman"]);

                            if (converter.CheckConvert<int>(r["PositionParent"]))
                                item.PositionParent = converter.Convert<int>(r["PositionParent"]);
                        }
                    }
                }
            }
        }
    }
}
