using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Dispetcher2.Class
{
    class C_Users
    {

        /// <summary>
        /// Select (LastName+' '+Name+' '+ SecondName) as FullName,PK_Login From Users <para />
        /// Where OnlyUser = 0 and IsValid=1
        /// </summary>
        public static void Select_FullName_PkLogin(ref DataTable DT)
        {
            try
            {
                DT.Clear();
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                cmd.CommandText = "Select (LastName+' '+Name+' '+ SecondName) as FullName,PK_Login From Users" + "\n" +
                                  "Where OnlyUser = 0 and IsValid=1" + "\n" +
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
        /// Select PK_Login From Users  <para />
        /// Where OnlyUser = 0 and IsValid=1
        /// </summary>
        public static void Select_PkLoginOnlyWorker(ref DataTable DT)
        {
            try
            {
                DT.Clear();
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                cmd.CommandText = "Select PK_Login,Department From Users" + "\n" +
                                  "Inner join Sp_Department ON Sp_Department.PK_IdDepartment=Users.FK_IdDepartment" + "\n" +
                                  "Where OnlyUser = 0 and IsValid=1" + "\n" +
                                  "Order by PK_Login";
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

        public static bool Check_PK_Login(string PK_Login)
        {
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                SqlDataReader reader;
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT PK_Login FROM Users" + "\n" +
                                 "Where PK_Login=@PK_Login";
                cmd.Parameters.Add(new SqlParameter("@PK_Login", SqlDbType.VarChar));
                cmd.Parameters["@PK_Login"].Value = PK_Login.Trim();
                cmd.Connection = C_Gper.con;
                C_Gper.con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {/*while (reader.Read()){if (reader.IsDBNull(0) == false) ffff = reader.GetInt32(0);}*/
                    reader.Dispose(); reader.Close(); C_Gper.con.Close();
                    return true;
                }
                else
                {
                    reader.Dispose(); reader.Close(); C_Gper.con.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }










    }
}
