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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace Dispetcher2
{
	public partial class F_UpdateOrder : Form
	{
		public string OrderUpdate = F_Orders.orderUpdate.ToString();
		BindingSource Bind_DT_OrdersDetails = new BindingSource();
        System.Data.DataTable dt = new System.Data.DataTable();
		List<string> lstOrder2 = new List<string>();
		Dictionary<string, int> dictOrder = new Dictionary<string, int>();
		IConfig config;

		public F_UpdateOrder(IConfig config)
		{
			this.config = config;
			InitializeComponent();
			this.Text = $"{OrderUpdate}";

			try
			{
				using (var con = new SqlConnection(config.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = con;
					cmd.Parameters.Clear();
					cmd.Connection.Open();
					cmd.CommandText = $"SELECT [PK_IdOrder] FROM [Dispetcher2].[dbo].[Orders] where OrderNum = '{OrderUpdate}'";
					object qwe = cmd.ExecuteScalar();
					cmd.CommandText = $"SELECT distinct [niipm].[product] AS ЩЦМ, Position AS Позиция, NameDetail AS Имя_детали, " +
										$"(SELECT TOP 1 [version] FROM[НИИПМ].[dbo].[rvwVersions] WHERE id = IdLoodsman) AS Версия_Диспетчер, " +
										$"(SELECT TOP 1 id FROM [НИИПМ].[dbo].[rvwVersions] WHERE id = IdLoodsman) AS Id_Диспетчер, " +
										$"(SELECT TOP 1 [version] FROM[НИИПМ].[dbo].[rvwVersions] where product = ShcmDetail AND state in ('Утвержден', 'Архив', 'Проектирование') " +
										$"AND type in ('Сборочная единица', 'Деталь') ORDER BY version DESC) AS Версия_лоцман ," +
										$"(SELECT TOP 1 id FROM[НИИПМ].[dbo].[rvwVersions] where product = ShcmDetail AND state in ('Утвержден', 'Архив', 'Проектирование') " +
										$"AND type in ('Сборочная единица', 'Деталь') ORDER BY version DESC) AS Id_Лоцман " +
										$"FROM OrdersDetails " +
										$"LEFT JOIN Sp_Details ON OrdersDetails.FK_IdDetail = Sp_Details.PK_IdDetail " +
										$"LEFT JOIN Sp_TypeDetails ON Sp_Details.FK_IdTypeDetail = Sp_TypeDetails.PK_IdTypeDetail " +
										$"LEFT JOIN[НИИПМ].[dbo].[rvwVersions] AS niipm " +
										$"ON niipm.product = ShcmDetail " +
										$"Where FK_IdOrder = @FK_IdOrder AND[niipm].state in ('Утвержден', 'Архив', 'Проектирование') AND[niipm].type in ('Сборочная единица', 'Деталь') " +
										$"ORDER BY Position";

					cmd.Parameters.Add(new SqlParameter("@FK_IdOrder", SqlDbType.Int));
					cmd.Parameters["@FK_IdOrder"].Value = qwe.ToString();
					SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dt);
					adapter.Dispose();
					con.Close();
				}
                dt.DefaultView.RowFilter = "Версия_Диспетчер < Версия_лоцман";
				orderDetailsDGV.DataSource = dt;

				// Установка режима автоматического изменения размера для третьего столбца
				orderDetailsDGV.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				// Задаем фиксированную высоту строки с заголовком столбцов
				orderDetailsDGV.ColumnHeadersHeight = 100; // Задаем высоту в пикселях
				orderDetailsDGV.Columns[0].HeaderText = "ЩЦМ";
				orderDetailsDGV.Columns[1].HeaderText = "Позиция";
				orderDetailsDGV.Columns[1].Width = 60;
				orderDetailsDGV.Columns[2].HeaderText = "Имя";
				orderDetailsDGV.Columns[3].HeaderText = "Версия в диспетчере";
				orderDetailsDGV.Columns[3].Width = 70;
				orderDetailsDGV.Columns[4].HeaderText = "id в диспетчере";
				orderDetailsDGV.Columns[4].Width = 70;
				orderDetailsDGV.Columns[5].HeaderText = "Версия в Лоцман";
				orderDetailsDGV.Columns[5].Width = 70;
				orderDetailsDGV.Columns[6].HeaderText = "id в Лоцман";
				orderDetailsDGV.Columns[6].Width = 70;


				orderDetailsDGV.Update();

				this.BringToFront();
				this.CenterToScreen();

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

		private void updateBTN_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in orderDetailsDGV.Rows)
			{
				if (row.Cells[0].Value != null && row.Cells[6].Value != null)
				{
					dictOrder.Add((string)row.Cells[0].Value, (int)row.Cells[6].Value);
				}
			}


			foreach (var shcm in dictOrder.Keys)
			{
				try
				{
					using (var con = new SqlConnection())
					{
						con.ConnectionString = config.ConnectionString;
						SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };
						cmd.CommandText = $"UPDATE [dbo].[Sp_Details] SET IdLoodsman = {dictOrder[shcm]} WHERE ShcmDetail = '{shcm}'";
						cmd.Connection = con;
						cmd.Connection.Open();
						cmd.ExecuteNonQuery();
						con.Close();
					}

				}
				catch (Exception ex)
				{

					MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			MessageBox.Show($"Заказ  {OrderUpdate.ToString()} обновлен!", "Отлично!", MessageBoxButtons.OK);
			dictOrder.Clear();
			this.Close();
		}
	}
}
