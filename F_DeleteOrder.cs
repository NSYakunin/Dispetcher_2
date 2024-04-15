using Dispetcher2.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Dispetcher2
{
	public partial class F_DeleteOrder : Form
	{

		IConfig config;
		DataSet ds;
		public F_DeleteOrder()
		{
			InitializeComponent();
			label1.Text = F_Orders.orderDel.ToString();
            this.Text = label1.Text;

			//try
			//{
			//	using (var con = new SqlConnection())
			//	{
			//		con.ConnectionString = config.ConnectionString;
			//		SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
			//		cmd.CommandText = "Select PK_IdOrder,OrderNum,OrderName,DateCreateOrder,FK_IdStatusOrders,NameStatusOrders,ValidationOrder" + "\n" +
			//						  "From Orders" + "\n" +
			//						  "LEFT JOIN SP_StatusOrders ON FK_IdStatusOrders = PK_IdStatusOrders";
			//		cmd.Connection = con;
			//		SqlDataAdapter adapter = new SqlDataAdapter();
			//		adapter.SelectCommand = cmd;
			//		adapter.Fill(ds);
			//		adapter.Dispose();
			//		con.Close();
			//	}
			//}
			//catch (Exception ex)
			//{

			//	MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//}
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}
	}
}
