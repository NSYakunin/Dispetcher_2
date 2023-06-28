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
        const int _CmdTimeout = 60; //seconds SqlCommand cmd = new SqlCommand() { CommandTimeout = CmdTimeout};
        int _PK_IdBrigade = 0;
        string _FullName;
        Int16 _AmountWorkers = 0;
        public C_Brigade(string FullName, Int16 AmountWorkers)//Конструктор
        {
            _FullName = FullName.Trim();
            _AmountWorkers = AmountWorkers;
            int DoubleID = Check_DoubleFullName();
            if (DoubleID > 0) MessageBox.Show("Данная бригада была создана ранее с № " + DoubleID.ToString(), "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else _PK_IdBrigade = CreateBrigade();
        }

        public int Get_IDBrigade
        {
            get { return _PK_IdBrigade; }
        }

        public string Get_FullName
        {
            get { return _FullName; }
        }

        /// <summary>
        /// insert into BrigadeWorkers (FK_IdBrigade,FK_Login) values (@FK_IdBrigade,@FK_Login)
        /// </summary>
        public bool AddWorkerInBrigade(string Worker)
        {
            if (_PK_IdBrigade > 0)
            {
                try
                {
                    C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.CommandText = "insert into BrigadeWorkers (FK_IdBrigade,FK_Login) " + "\n" +
                                  "values (@FK_IdBrigade,@FK_Login)";
                    cmd.Connection = C_Gper.con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@FK_IdBrigade", SqlDbType.Int));
                    cmd.Parameters["@FK_IdBrigade"].Value = _PK_IdBrigade;
                    cmd.Parameters.Add(new SqlParameter("@FK_Login", SqlDbType.VarChar));
                    cmd.Parameters["@FK_Login"].Value = Worker;
                    //***********************************************************
                    C_Gper.con.Open();
                    cmd.ExecuteNonQuery();
                    C_Gper.con.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    C_Gper.con.Close();
                    MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DeleteBrigade();
                    return false;
                }
            }
            else
                return false;
        }

        /// <summary>
        /// Select PK_IdBrigade,FullName,IsValid From Brigade
        /// </summary>
        public static void SelectAllLoginBrigade(ref DataTable DT)
        {
            try
            {
                DT.Clear();
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand() { CommandTimeout = _CmdTimeout };//using System.Data.SqlClient;
                cmd.CommandText = "Select PK_IdBrigade,FullName,IsValid From Brigade" + "\n" +
                                  "Order by FullName";
                cmd.Connection = C_Gper.con;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(DT);
                adapter.Dispose();
                C_Gper.con.Close();
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Select PK_IdBrigade,FullName From Brigade  <para />
        /// Where IsValid=1 <para />
        /// Order by FullName
        /// </summary>
        public static void Select_ActiveBrigade(ref DataTable DT)
        {
            try
            {
                DT.Clear();
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand() { CommandTimeout = _CmdTimeout };//seconds //using System.Data.SqlClient;
                cmd.CommandText = "Select PK_IdBrigade,FullName From Brigade" + "\n" +
                                  "Where IsValid=1" + "\n" +
                                  "Order by FullName";
                cmd.Connection = C_Gper.con;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(DT);
                adapter.Dispose();
                C_Gper.con.Close();
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Update Brigade set IsValid=@IsValid where PK_IdBrigade=@PK_IdBrigade
        /// </summary>
        public static bool UpdateIsValidBrigade(int PK_IdBrigade, bool IsValid)
        {
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.CommandText = "Update Brigade set IsValid=@IsValid " + "\n" +
                                  "where PK_IdBrigade=@PK_IdBrigade";

                cmd.Connection = C_Gper.con;
                //Parameters**************************************************
                cmd.Parameters.Add(new SqlParameter("@PK_IdBrigade", SqlDbType.Int));
                cmd.Parameters["@PK_IdBrigade"].Value = PK_IdBrigade;
                cmd.Parameters.Add(new SqlParameter("@IsValid", SqlDbType.Bit));
                cmd.Parameters["@IsValid"].Value = IsValid;

                //***********************************************************
                C_Gper.con.Open();
                cmd.ExecuteNonQuery();
                C_Gper.con.Close();
                return true;
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Select PK_IdBrigade From Brigade Where FullName = @FullName
        /// </summary>
        /// <returns> int DoubleID</returns>
        private int Check_DoubleFullName()
        {
            int DoubleID=0;
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                SqlDataReader reader;
                cmd.Parameters.Clear();
                cmd.CommandText = "Select PK_IdBrigade From Brigade" + "\n" +
                                  "Where FullName = @FullName";
                cmd.Parameters.Add(new SqlParameter("@FullName", SqlDbType.VarChar));
                cmd.Parameters["@FullName"].Value = _FullName.Trim();
                cmd.Connection = C_Gper.con;
                C_Gper.con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                        if (reader.IsDBNull(0) == false) DoubleID = reader.GetInt32(0); else DoubleID = 0;    
                }
                reader.Dispose(); reader.Close(); C_Gper.con.Close();
                return DoubleID;
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return DoubleID;
            }
        }

        /// <summary>
        /// insert into Brigade (FullName) values (@FullName) SELECT SCOPE_IDENTITY()
        /// </summary>
        private int CreateBrigade()
        {
            int NewId = 0;
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.Connection = C_Gper.con;
                cmd.Parameters.Clear();
                cmd.CommandText = "insert into Brigade (FullName,AmountWorkers) values (@FullName,@AmountWorkers)" + "\n" +
                                  "SELECT SCOPE_IDENTITY()";
                //Parameters**************************************************
                cmd.Parameters.Add(new SqlParameter("@FullName", SqlDbType.VarChar));
                cmd.Parameters["@FullName"].Value = _FullName;
                cmd.Parameters.Add(new SqlParameter("@AmountWorkers", SqlDbType.SmallInt));
                cmd.Parameters["@AmountWorkers"].Value = _AmountWorkers;
                //***********************************************************
                C_Gper.con.Open();
                NewId = Convert.ToInt32(cmd.ExecuteScalar());
                C_Gper.con.Close();
                return NewId;
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return NewId = 0;
            }
        }

        /// <summary>
        /// delete from Brigade where PK_IdBrigade=@PK_IdBrigade
        /// </summary>
        private void DeleteBrigade()
        {
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.CommandText = "delete from Brigade where PK_IdBrigade=@PK_IdBrigade";
                cmd.Connection = C_Gper.con;
                //Parameters**************************************************
                cmd.Parameters.Add(new SqlParameter("@PK_IdBrigade", SqlDbType.Int));
                cmd.Parameters["@PK_IdBrigade"].Value = _PK_IdBrigade;
                //***********************************************************
                C_Gper.con.Open();
                cmd.ExecuteNonQuery();
                C_Gper.con.Close();
                MessageBox.Show("Создание бригады завершилось неудачей.", "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }









    }
}
