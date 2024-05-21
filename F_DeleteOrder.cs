using Dispetcher2.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Dispetcher2
{
	public partial class F_DeleteOrder : Form
	{
		public string OrderUpdate { get; }
		IConfig config;
		DataSet ds;
		System.Data.DataTable dt = new System.Data.DataTable();
		private List<string> selectedItems = new List<string>();
		public F_DeleteOrder(IConfig config)
		{
			InitializeComponent();
			this.config = config;

			try
			{
				using (var con = new SqlConnection(config.ConnectionString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = con;
					cmd.Parameters.Clear();
					cmd.Connection.Open();
					cmd.CommandText = $"SELECT [OrderNum] AS 'Заказ'," +
										$"[OrderName] AS 'Наименование', " +
										$"[FK_IdStatusOrders] AS 'Статус заказа' " +
										$"FROM [Dispetcher2].[dbo].[Orders]";
					SqlDataAdapter adapter = new SqlDataAdapter();
					adapter.SelectCommand = cmd;
					adapter.Fill(dt);
					adapter.Dispose();
					con.Close();
				}


				dt.Columns.Add("Статус", typeof(string));
				

				foreach (DataRow row in dt.Rows)
				{
                    switch (Convert.ToInt32(row[2]))
					{
						case 1:
							row[3] = "Ожидание";
							break;
						case 2:
							row[3] = "Открыт";
							break;
						case 3:
							row[3] = "Закрыт";
							break;
						default:
							row[3] = "";
							break;
					}
                }

				orderListDGV.DataSource = dt;

				orderListDGV.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				orderListDGV.ColumnHeadersHeight = 100;
				orderListDGV.Columns[0].HeaderText = "Заказ";
				orderListDGV.Columns[1].HeaderText = "Наименование";
				orderListDGV.Columns[2].HeaderText = "Статус заказа";
				orderListDGV.Columns[3].HeaderText = "Статус";

				DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
				checkBoxColumn.Name = "Выбрать";
				checkBoxColumn.HeaderText = "V";
				orderListDGV.Columns.Add(checkBoxColumn);
				orderListDGV.Columns[4].Width = 50;

				orderListDGV.Sort(orderListDGV.Columns[3], ListSortDirection.Ascending);

				orderListDGV.Update();
				this.BringToFront();
				this.CenterToScreen();



			}
			catch (Exception ex)
			{

				MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public F_DeleteOrder(string orderUpdate)
		{
			OrderUpdate = orderUpdate;
		}


		private void orderListDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

			if (e.ColumnIndex == 0 && e.RowIndex >= 0)
			{
				DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)orderListDGV.Rows[e.RowIndex].Cells[e.ColumnIndex];

				bool isChecked = (bool)checkBoxCell.EditedFormattedValue;

                if (isChecked)
				{
					selectedItems.Add($"'{orderListDGV.Rows[e.RowIndex].Cells[1].Value.ToString()}'");
				}

				else
				{
					selectedItems.Remove($"'orderListDGV.Rows[e.RowIndex].Cells[1].Value.ToString()'");
				}
			}
		}

		private void closeOrderBTN_Click(object sender, EventArgs e)
		{
			if (selectedItems.Count == 0)
			{
				MessageBox.Show($"Выберите заказ", "Внимание!",
				MessageBoxButtons.OK);
				return;
			}

			DialogResult result = MessageBox.Show(
				$"Закрыть {selectedItems.Count} заказов?", "Внимание!",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning,
				MessageBoxDefaultButton.Button1,
				MessageBoxOptions.DefaultDesktopOnly);


			var dataOrders = string.Join(",", selectedItems);

            if (result == DialogResult.Yes)
			{
				try
				{
					using (var con = new SqlConnection(config.ConnectionString))
					{
						SqlCommand cmd = new SqlCommand();
						cmd.Connection = con;
						cmd.Parameters.Clear();
						cmd.Connection.Open();
						cmd.CommandText = $"UPDATE [dbo].[Orders] SET [FK_IdStatusOrders] = 3" +
							$" WHERE OrderNum IN ({dataOrders})";
                        cmd.ExecuteNonQuery();
					}
				}

				catch (Exception ex)
				{
					MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}
}
