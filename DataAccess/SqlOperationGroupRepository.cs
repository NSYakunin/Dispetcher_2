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
    public class SqlOperationGroupRepository : OperationGroupRepository
    {
        IConfig config;
        IConverter converter;
        List<OperationGroup> groups;

        public SqlOperationGroupRepository(IConfig config, IConverter converter)
        {
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр config");
            if (converter == null) throw new ArgumentException("Пожалуйста укажите параметр converter");
            this.config = config;
            this.converter = converter;
        }

        void LoadGroups()
        {
            groups = new List<OperationGroup>();
            using (var cn = new SqlConnection(connectionString: config.ConnectionString))
            {
                using (var cmd = new SqlCommand() { Connection = cn })
                {
                    cmd.CommandText = "SELECT * FROM [dbo].[Sp_OperationsGroup]";
                    cmd.CommandType = CommandType.Text;

                    cn.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            // Вроде не должно быть NULL, поэтому для простоты не проверяем
                            int id = converter.Convert<int>(r["PK_IdOperGroup"]);
                            string name = converter.Convert<string>(r["NamеOperGroup"]);
                            OperationGroup g = new OperationGroup() { Id = id, Name = name };
                            groups.Add(g);
                        }
                    }
                }
            }
        }

        public override void Load()
        {
            LoadGroups();
        }
        public override IEnumerable<OperationGroup> GetGroups()
        {
            if (groups == null) Load();
            return groups;
        }
        public override IEnumerable GetList()
        {
            return groups;
        }
    }
}
