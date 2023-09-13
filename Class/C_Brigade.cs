using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;


namespace Dispetcher2.Class
{
    class C_Brigade
    {
        // ку
        const int _CmdTimeout = 60; //seconds SqlCommand cmd = new SqlCommand() { CommandTimeout = CmdTimeout};

        IConfig config;
        public C_Brigade(IConfig config)//Конструктор
        {
            this.config = config;
        }

        public int Create(string name, Int16 AmountWorkers)
        {
            int DoubleID = Check_DoubleFullName(name);
            if (DoubleID > 0)
            {
                MessageBox.Show("Данная бригада была создана ранее с № " + DoubleID.ToString(), "Внимание!!!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }
            else return CreateBrigade(name, AmountWorkers);
        }



        /// <summary>
        /// insert into BrigadeWorkers (FK_IdBrigade,FK_Login) values (@FK_IdBrigade,@FK_Login)
        /// </summary>
        public bool AddWorkerInBrigade(int id, string name)
        {
            if (id > 0)
            {
                try
                {
                    using (var con = new SqlConnection())
                    {
                        con.ConnectionString = config.ConnectionString;
                        SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                        cmd.CommandText = "insert into BrigadeWorkers (FK_IdBrigade,FK_Login) " + "\n" +
                                      "values (@FK_IdBrigade,@FK_Login)";
                        cmd.Connection = con;
                        //Parameters**************************************************
                        cmd.Parameters.Add(new SqlParameter("@FK_IdBrigade", SqlDbType.Int));
                        cmd.Parameters["@FK_IdBrigade"].Value = id;
                        cmd.Parameters.Add(new SqlParameter("@FK_Login", SqlDbType.VarChar));
                        cmd.Parameters["@FK_Login"].Value = name;
                        //***********************************************************
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DeleteBrigade(id);
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// Select PK_IdBrigade,FullName,IsValid From Brigade
        /// </summary>
        public void SelectAllLoginBrigade(ref DataTable DT)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    DT.Clear();
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = _CmdTimeout };//using System.Data.SqlClient;
                    cmd.CommandText = "Select PK_IdBrigade,FullName,IsValid From Brigade" + "\n" +
                                      "Order by FullName";
                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(DT);
                    adapter.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Select PK_IdBrigade,FullName From Brigade  <para />
        /// Where IsValid=1 <para />
        /// Order by FullName
        /// </summary>
        public void Select_ActiveBrigade(ref DataTable DT)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    DT.Clear();
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = _CmdTimeout };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "Select PK_IdBrigade,FullName From Brigade" + "\n" +
                                      "Where IsValid=1" + "\n" +
                                      "Order by FullName";
                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(DT);
                    adapter.Dispose();
                    
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Update Brigade set IsValid=@IsValid where PK_IdBrigade=@PK_IdBrigade
        /// </summary>
        public bool UpdateIsValidBrigade(int PK_IdBrigade, bool IsValid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.CommandText = "Update Brigade set IsValid=@IsValid " + "\n" +
                                      "where PK_IdBrigade=@PK_IdBrigade";

                    cmd.Connection = con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@PK_IdBrigade", SqlDbType.Int));
                    cmd.Parameters["@PK_IdBrigade"].Value = PK_IdBrigade;
                    cmd.Parameters.Add(new SqlParameter("@IsValid", SqlDbType.Bit));
                    cmd.Parameters["@IsValid"].Value = IsValid;

                    //***********************************************************
                    con.Open();
                    cmd.ExecuteNonQuery();
                    
                    return true;
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Select PK_IdBrigade From Brigade Where FullName = @FullName
        /// </summary>
        /// <returns> int DoubleID</returns>
        private int Check_DoubleFullName(string name)
        {
            int DoubleID=0;
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    SqlDataReader reader;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "Select PK_IdBrigade From Brigade" + "\n" +
                                      "Where FullName = @FullName";
                    cmd.Parameters.Add(new SqlParameter("@FullName", SqlDbType.VarChar));
                    cmd.Parameters["@FullName"].Value = name.Trim();
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            if (reader.IsDBNull(0) == false) DoubleID = reader.GetInt32(0); else DoubleID = 0;
                    }
                    reader.Dispose(); reader.Close(); con.Close();
                    return DoubleID;
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return DoubleID;
            }
        }

        /// <summary>
        /// insert into Brigade (FullName) values (@FullName) SELECT SCOPE_IDENTITY()
        /// </summary>
        private int CreateBrigade(string name, Int16 AmountWorkers)
        {
            int NewId = 0;
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "insert into Brigade (FullName,AmountWorkers) values (@FullName,@AmountWorkers)" + "\n" +
                                      "SELECT SCOPE_IDENTITY()";
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@FullName", SqlDbType.VarChar));
                    cmd.Parameters["@FullName"].Value = name.Trim();
                    cmd.Parameters.Add(new SqlParameter("@AmountWorkers", SqlDbType.SmallInt));
                    cmd.Parameters["@AmountWorkers"].Value = AmountWorkers;
                    //***********************************************************
                    con.Open();
                    NewId = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    return NewId;
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return NewId = 0;
            }
        }

        /// <summary>
        /// delete from Brigade where PK_IdBrigade=@PK_IdBrigade
        /// </summary>
        private void DeleteBrigade(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.CommandText = "delete from Brigade where PK_IdBrigade=@PK_IdBrigade";
                    cmd.Connection = con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@PK_IdBrigade", SqlDbType.Int));
                    cmd.Parameters["@PK_IdBrigade"].Value = id;
                    //***********************************************************
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Создание бригады завершилось неудачей.", "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }









    }
}
