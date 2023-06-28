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
    public partial class dF_Brigade : Form
    {
        public dF_Brigade()
        {
            InitializeComponent();
            DT_Brigades.Columns.Add("PK_IdBrigade", typeof(int));
            DT_Brigades.Columns.Add("FullName", typeof(string));
        }

        DataTable DT_Brigades = new DataTable();//Бригады
        BindingSource BS_Brigades = new BindingSource();//Бригады

        int _PK_IdBrigade = 0;
        string _FullNameBrigade = "";

        public int Get_IDBrigade
        {
            get { return _PK_IdBrigade; }
        }

        public string Get_FullNameBrigade
        {
            get { return _FullNameBrigade; }
        }

        private void dF_Brigade_Load(object sender, EventArgs e)
        {
            C_Brigade.Select_ActiveBrigade(ref DT_Brigades);
            dGV_Brigade.AutoGenerateColumns = false;
            dGV_Brigade.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_Brigade.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            BS_Brigades.DataSource = DT_Brigades;
            dGV_Brigade.DataSource = BS_Brigades;
            dGV_Brigade.Columns["Col_NumBrigade"].DataPropertyName = DT_Brigades.Columns["PK_IdBrigade"].ToString();
            dGV_Brigade.Columns["Col_LoginBrigade"].DataPropertyName = DT_Brigades.Columns["FullName"].ToString();
        }

        private void tB_SearchBrigade_TextChanged(object sender, EventArgs e)
        {
            BS_Brigades.Filter = " FullName like '%" + tB_SearchBrigade.Text.ToString().Trim() + "%'";
        }

        private void btn_Choose_Click(object sender, EventArgs e)
        {
            if (dGV_Brigade.CurrentRow != null)
            {
                CurrencyManager cmgr = (CurrencyManager)dGV_Brigade.BindingContext[dGV_Brigade.DataSource, dGV_Brigade.DataMember];
            DataRow row = ((DataRowView)cmgr.Current).Row;
            _PK_IdBrigade = Convert.ToInt32(row["PK_IdBrigade"]);
            _FullNameBrigade = row["FullName"].ToString();
            this.Close();
            }
            else
                MessageBox.Show("Не выбрана бригада.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void tB_SearchBrigade_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (dGV_Brigade.Rows.Count > 0) dGV_Brigade.Select();
        }

        private void dGV_Brigade_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btn_Choose.PerformClick();
            }
        }

        private void dGV_Brigade_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btn_Choose.PerformClick();
        }
    }
}
