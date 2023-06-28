using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dispetcher2.Class;

namespace Dispetcher2.DialogsForms
{
    public partial class dF_Workers : Form
    {
        public dF_Workers()
        {
            InitializeComponent();
            DT_Workers.Columns.Add("FullName", typeof(string));
            DT_Workers.Columns.Add("PK_Login", typeof(string));
        }

        DataTable DT_Workers = new DataTable();//Рабочие
        BindingSource BS_Workers = new BindingSource();//Рабочие

        string _PK_Login = "";

        public string Get_PK_Login
        {
            get { return _PK_Login; }
        }

        private void dF_Workers_Load(object sender, EventArgs e)
        {
            C_Users.Select_FullName_PkLogin(ref DT_Workers);
            dGV_Workers.AutoGenerateColumns = false;
            dGV_Workers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_Workers.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            BS_Workers.DataSource = DT_Workers;
            dGV_Workers.DataSource = BS_Workers;
            dGV_Workers.Columns["Col_PK_Login"].DataPropertyName = DT_Workers.Columns["PK_Login"].ToString();
            dGV_Workers.Columns["Col_FullName"].DataPropertyName = DT_Workers.Columns["FullName"].ToString();
        }

        private void tB_Workers_TextChanged(object sender, EventArgs e)
        {
            BS_Workers.Filter = " FullName like '%" + tB_Workers.Text.ToString().Trim() + "%'";
        }

        private void btn_Choose_Click(object sender, EventArgs e)
        {
            if (dGV_Workers.CurrentRow != null)
            {
                CurrencyManager cmgr = (CurrencyManager)dGV_Workers.BindingContext[dGV_Workers.DataSource, dGV_Workers.DataMember];
                DataRow row = ((DataRowView)cmgr.Current).Row;
                _PK_Login = row["PK_Login"].ToString();
                this.Close();
            }
            else
                MessageBox.Show("Не выбран рабочий.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void tB_Workers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (dGV_Workers.Rows.Count > 0) dGV_Workers.Select();
        }

        private void dGV_Workers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btn_Choose.PerformClick();
            }
        }

        private void dGV_Workers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btn_Choose.PerformClick();
        }
    }
}
