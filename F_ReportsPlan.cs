﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dispetcher2.Class;

namespace Dispetcher2
{
    public partial class F_ReportsPlan : Form
    {
        // Конфигурация
        IConfig config;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Orders orders;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Reports RepForm6;

        public F_ReportsPlan(IConfig config)
        {
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр config");
            this.config = config;
            orders = new C_Orders(config);

            RepForm6 = new C_Reports(config);

            InitializeComponent();
            DT_Orders.Columns.Add("PK_IdOrder", typeof(int));
            DT_Orders.Columns.Add("OrderNum", typeof(string));
            DT_Orders.Columns.Add("OrderName", typeof(string));
        }

        DataTable DT_Orders = new DataTable();
        BindingSource BS_Orders = new BindingSource();

        private void F_ReportsPlan_Load(object sender, EventArgs e)
        {
            if (config.SelectedReportMode == ReportMode.ПланГрафик)//План-график (форма №6)
            {
                myTabC_ReportsPlan.SelectedTab = tPageForm106;
                dGV_Orders.AutoGenerateColumns = false;
                dGV_Orders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dGV_Orders.RowsDefaultCellStyle.BackColor = SystemColors.Info;
                BS_Orders.DataSource = DT_Orders;
                dGV_Orders.DataSource = BS_Orders;
                dGV_Orders.Columns["Col_OrderNum"].DataPropertyName = DT_Orders.Columns["OrderNum"].ToString();
                //Bindings
                tB_OrderName.DataBindings.Add("Text", BS_Orders, "OrderName", false, DataSourceUpdateMode.OnPropertyChanged);
                tB_OrderNumInfo.DataBindings.Add("Text", BS_Orders, "OrderNum", false, DataSourceUpdateMode.OnPropertyChanged);
                orders.SelectOrdersData(2, ref DT_Orders);//2-opened
            }
        }

        private void tB_OrderNum_TextChanged(object sender, EventArgs e)
        {
            BS_Orders.Filter = " OrderNum like '%" + tB_OrderNum.Text.ToString().Trim() + "%'";
        }

        private void btn_PlanForm6Create_Click(object sender, EventArgs e)
        {
            CurrencyManager cmgr = (CurrencyManager)this.dGV_Orders.BindingContext[this.dGV_Orders.DataSource, dGV_Orders.DataMember];
            DataRow row = ((DataRowView)cmgr.Current).Row;
            int PK_IdOrder = Convert.ToInt32(row["PK_IdOrder"]);
            
            RepForm6.PlanSheduleForm6(PK_IdOrder, tB_OrderNumInfo.Text.Trim(), tB_OrderName.Text.Trim());
        }



    }
}
