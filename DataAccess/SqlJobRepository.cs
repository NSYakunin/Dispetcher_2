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
    public class SqlJobRepository : JobRepository
    {
        IConfig config;
        IConverter converter;
        List<Job> jobs;
        public SqlJobRepository(IConfig config, IConverter converter)
        {
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр config");
            if (converter == null) throw new ArgumentException("Пожалуйста укажите параметр converter");
            this.config = config;
            this.converter = converter;
        }
        void ExecuteJobRead()
        {
            jobs = new List<Job>();
            using (var cn = new SqlConnection() { ConnectionString = config.ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn })
                {
                    cmd.CommandText = "JobRead";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var item = new Job();
                            jobs.Add(item);

                            if (converter.CheckConvert<int>(r["Pk_IdJob"]))
                                item.Id = converter.Convert<int>(r["Pk_IdJob"]);

                            if (converter.CheckConvert<string>(r["NameJob"]))
                                item.Name = converter.Convert<string>(r["NameJob"]);
                            
                            if (converter.CheckConvert<bool>(r["JobIsvalid"]))
                                item.Valid = converter.Convert<bool>(r["JobIsvalid"]);

                            if (converter.CheckConvert<int>(r["OperationGroupId"]))
                                item.OperationGroupId = converter.Convert<int>(r["OperationGroupId"]);
                        }
                    }
                }
            }
            jobs.Sort(this.JobComparer);
        }
        int JobComparer(Job j1, Job j2)
        {
            if (j1 == null || j2 == null) return 0;
            if (j1.Name == null) return 0;
            else return j1.Name.CompareTo(j2.Name);
        }
        public override void JobUpdate(Job item)
        {
            using (var cn = new SqlConnection() { ConnectionString = config.ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn })
                {
                    cmd.CommandText = "JobUpdate";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", item.Id);
                    cmd.Parameters.AddWithValue("@Name", item.Name);
                    cmd.Parameters.AddWithValue("@Valid", item.Valid);
                    cmd.Parameters.AddWithValue("@OperationGroupId", item.OperationGroupId);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public override IEnumerable<Job> JobRead()
        {
            if (jobs == null) Load();
            return jobs;
        }
        public override IEnumerable GetList()
        {
            if (jobs == null) Load();
            return jobs;
        }
        public override void Load()
        {
            ExecuteJobRead();
        }
    }
}
