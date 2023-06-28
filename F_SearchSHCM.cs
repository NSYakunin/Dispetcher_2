using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dispetcher2
{
    public partial class F_SearchSHCM : Form
    {
        DataTable DT_Search = new DataTable();

        public F_SearchSHCM()
        {
            InitializeComponent();
            DT_Search.Columns.Add("OrderNum", typeof(string));
            DT_Search.Columns.Add("Position", typeof(int));
            DT_Search.Columns.Add("ShcmDetail", typeof(string));
            DT_Search.Columns.Add("NameDetail", typeof(string));
            DT_Search.Columns.Add("AmountDetails", typeof(int));
            DT_Search.Columns.Add("IdLoodsman", typeof(long));
            //***********************************************************************************************
        }

        private void F_SearchSHCM_Load(object sender, EventArgs e)
        {
            dGV_Search.AutoGenerateColumns = false;
            dGV_Search.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_Search.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            dGV_Search.DataSource = DT_Search;
            dGV_Search.Columns["Col_OrderNum"].DataPropertyName = DT_Search.Columns["OrderNum"].ToString();
            dGV_Search.Columns["Col_Position"].DataPropertyName = DT_Search.Columns["Position"].ToString();
            dGV_Search.Columns["Col_ShcmDetail"].DataPropertyName = DT_Search.Columns["ShcmDetail"].ToString();
            dGV_Search.Columns["Col_NameDetail"].DataPropertyName = DT_Search.Columns["NameDetail"].ToString();
            dGV_Search.Columns["Col_AmountDetails"].DataPropertyName = DT_Search.Columns["AmountDetails"].ToString();
            dGV_Search.Columns["Col_IdLoodsman"].DataPropertyName = DT_Search.Columns["IdLoodsman"].ToString();
        }

        private void tB_ShcmDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter & tB_ShcmDetail.Text.Trim().Length > 0)
            {
                Class.C_DataBase DB = new Class.C_DataBase(Class.C_Gper.ConnStrDispetcher2);

                string sql = "Select OrderNum,Od.Position,spD.ShcmDetail,spD.NameDetail,Od.AmountDetails,spD.IdLoodsman From Sp_Details spD" + "\n" +
                             "inner join OrdersDetails Od On Od.FK_IdDetail = spD.PK_IdDetail" + "\n" +
                             "inner join Orders O On O.PK_IdOrder = Od.FK_IdOrder" + "\n" +
                             "Where spD.ShcmDetail like '%" + tB_ShcmDetail.Text.Trim() + "%'" + "\n" +
                             "Order by OrderNum";
                DB.Select_DT(ref DT_Search, sql);
            }
        }

        private void tB_ShcmDetail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',') e.KeyChar = '.';
        }

        private void tB_ShcmDetail_TextChanged(object sender, EventArgs e)
        {
            DT_Search.Clear();
        }



    }
}
