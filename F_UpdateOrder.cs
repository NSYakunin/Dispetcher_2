using Dispetcher2.Class;
using Microsoft.Office.Interop.Word;
using SourceGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace Dispetcher2
{
	public partial class F_UpdateOrder : Form
	{
		public string OrderUpdate = F_Orders.orderUpdate.ToString();
		BindingSource Bind_DT_OrdersDetails = new BindingSource();
        System.Data.DataTable dt = new System.Data.DataTable();
		List<string> lst = new List<string>();
		public F_UpdateOrder(IConfig config)
		{
			InitializeComponent();

			try
			{
				using (var con = new SqlConnection(config.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
					cmd.Connection = con;
					cmd.Parameters.Clear();
					cmd.Connection.Open();
					cmd.CommandText = $"SELECT [PK_IdOrder] FROM [Dispetcher2].[dbo].[Orders] where OrderNum = '{OrderUpdate}'";
					object qwe = cmd.ExecuteScalar();
					cmd.CommandText = $"SELECT distinct [niipm].[product] AS ЩЦМ, Position AS Позиция, NameDetail AS Имя_детали, " +
										$"(SELECT TOP 1[version] FROM[НИИПМ].[dbo].[rvwVersions] WHERE id = IdLoodsman) AS Версия_диспетчер, " +
										$"(SELECT TOP 1[version] FROM[НИИПМ].[dbo].[rvwVersions] where product = ShcmDetail AND state in ('Утвержден', 'Архив', 'Проектирование') " +
										$"AND type in ('Сборочная единица', 'Деталь') ORDER BY version DESC) AS Версия_лоцман " +
										$"FROM OrdersDetails " +
										$"LEFT JOIN Sp_Details ON OrdersDetails.FK_IdDetail = Sp_Details.PK_IdDetail " +
										$"LEFT JOIN Sp_TypeDetails ON Sp_Details.FK_IdTypeDetail = Sp_TypeDetails.PK_IdTypeDetail " +
										$"LEFT JOIN[НИИПМ].[dbo].[rvwVersions] AS niipm " +
										$"ON niipm.product = ShcmDetail " +
										$"Where FK_IdOrder = '6412' AND[niipm].state in ('Утвержден', 'Архив', 'Проектирование') AND[niipm].type in ('Сборочная единица', 'Деталь') " +
										$"ORDER BY Position";

					cmd.Parameters.Add(new SqlParameter("@FK_IdOrder", SqlDbType.Int));
					cmd.Parameters["@FK_IdOrder"].Value = qwe.ToString();
					SqlDataAdapter adapter = new SqlDataAdapter();
					adapter.SelectCommand = cmd;
                    adapter.Fill(dt);
					adapter.Dispose();
					con.Close();
				}

				dt.DefaultView.RowFilter = "Версия_диспетчер < Версия_лоцман";
				orderDetailsDGV.DataSource = dt;
				orderDetailsDGV.Update();

				// Установка режима автоматического изменения размера для третьего столбца
				orderDetailsDGV.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;



			}
			catch (Exception ex)
			{

				MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public F_UpdateOrder()
		{
			InitializeComponent();
		}

		private void F_UpdateOrder_Load(object sender, EventArgs e)
		{
			nameOrderLBL.Text = $"Данные о заказе № {OrderUpdate}";
		}
	}
}
