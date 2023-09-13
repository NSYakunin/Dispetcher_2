using System;
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
    public partial class F_Brigade : Form
    {
        IConfig config;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Users users;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Brigade brigade;
        public F_Brigade(IConfig config)
        {
            this.config = config;
            users = new C_Users(config);
            brigade = new C_Brigade(config);
            InitializeComponent();
        }

        DataTable DT_Workers = new DataTable();//Рабочие
        BindingSource BS_Workers = new BindingSource();

        DataTable DT_Brigades = new DataTable();//Бригады
        BindingSource BS_Brigades = new BindingSource();

        private void F_Brigade_Load(object sender, EventArgs e)
        {
            users.Select_FullName_PkLogin(ref DT_Workers);//Использовать только тут иначе набор данных в гриде не изменится
            brigade.SelectAllLoginBrigade(ref DT_Brigades);//Использовать только тут иначе набор данных в гриде не изменится
            //*************************************************
            dGV_Workers.AutoGenerateColumns = false;
            dGV_Workers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_Workers.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            BS_Workers.DataSource = DT_Workers;
            dGV_Workers.DataSource = BS_Workers;
            dGV_Workers.Columns["Col_FullName"].DataPropertyName = DT_Workers.Columns["FullName"].ToString();
            dGV_Workers.Columns["Col_Login"].DataPropertyName = DT_Workers.Columns["PK_Login"].ToString();
            dGV_NewBrigade.AutoGenerateColumns = false;
            dGV_NewBrigade.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_NewBrigade.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            dGV_AllBrigade.AutoGenerateColumns = false;
            dGV_AllBrigade.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_AllBrigade.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            BS_Brigades.DataSource = DT_Brigades;
            dGV_AllBrigade.DataSource = BS_Brigades;
            dGV_AllBrigade.Columns["Col_NumBrigade"].DataPropertyName = DT_Brigades.Columns["PK_IdBrigade"].ToString();
            dGV_AllBrigade.Columns["Col_LoginBrigade"].DataPropertyName = DT_Brigades.Columns["FullName"].ToString();
            dGV_AllBrigade.Columns["Col_IsValidBrigade"].DataPropertyName = DT_Brigades.Columns["IsValid"].ToString();
            tB_Search.Focus();
        }

        private void tB_Search_TextChanged(object sender, EventArgs e)
        {
            BS_Workers.Filter = "FullName like '%" + tB_Search.Text.ToString().Trim() + "%'";
        }

        private void tB_SearchBrigade_TextChanged(object sender, EventArgs e)
        {
            BS_Brigades.Filter = "FullName like '%" + tB_SearchBrigade.Text.ToString().Trim() + "%'";
        }

        private void dGV_Workers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dGV_Workers.CurrentRow != null)
                {
                    bool findWorker = false;
                    foreach (DataGridViewRow dr in dGV_NewBrigade.Rows)
                    {
                        if (dr.Cells["Col_LoginNewBrigade"].Value == dGV_Workers.CurrentRow.Cells["Col_Login"].Value)
                        {
                            findWorker = true;
                            break;
                        }
                    }
                    if (findWorker) MessageBox.Show("Выбранный рабочий уже был добавлен в бригаду.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                    if (dGV_NewBrigade.Rows.Count > 5) MessageBox.Show("Допускается не более 6 рабочих в бригаде.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);//просто чтоб не наглели, а так можно любое кол-во ставить
                    else
                    dGV_NewBrigade.Rows.Add(dGV_Workers.CurrentRow.Cells["Col_FullName"].Value, dGV_Workers.CurrentRow.Cells["Col_Login"].Value);
                    //Применяем фильтр для уменьшения вероятности создания бригады из фамилий в другой последовательности
                    if (dGV_NewBrigade.Rows.Count == 1) tB_SearchBrigade.Text = dGV_Workers.CurrentRow.Cells["Col_Login"].Value.ToString().Trim();
                }
            }
        }

        private void dGV_Brigade_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (dGV_NewBrigade.CurrentRow != null)
                {
                    dGV_NewBrigade.Rows.Remove(dGV_NewBrigade.CurrentRow);
                }
            }
        }

        private void btn_Create_Click(object sender, EventArgs e)
        {
            if (dGV_NewBrigade.Rows.Count > 0)
            {
                string FullName = "";
                foreach (DataGridViewRow dr in dGV_NewBrigade.Rows)
                {
                    FullName += dr.Cells["Col_LoginNewBrigade"].Value.ToString() + "; ";
                }
                Int16 AmountWorkers = Convert.ToInt16(dGV_NewBrigade.Rows.Count);

                int id = brigade.Create(FullName, AmountWorkers);
                if (id > 0)
                    for (int i = 0; i < dGV_NewBrigade.Rows.Count; i++)
                    {
                        string name = dGV_NewBrigade.Rows[i].Cells["Col_LoginNewBrigade"].Value.ToString();
                        if (!brigade.AddWorkerInBrigade(id, name)) break;
                        if (i == dGV_NewBrigade.Rows.Count-1)//Если последняя запись прошла успешно
                        {
                            DT_Brigades.Rows.Add(id, FullName, 1);
                            //dGV_AllBrigade.Rows.Add(Brigade.Get_IDBrigade, Brigade.Get_FullName, 1);
                            //MessageBox.Show("Создание бригады успешно завершено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dGV_NewBrigade.Rows.Clear();
                            dGV_AllBrigade.Rows[dGV_AllBrigade.Rows.Count - 1].Selected=true;
                            dGV_AllBrigade.FirstDisplayedScrollingRowIndex = dGV_AllBrigade.Rows.Count - 1;
                        }
                    }

            }
            
        }

        private void dGV_AllBrigade_KeyDown(object sender, KeyEventArgs e)
        {
            if (dGV_AllBrigade.CurrentRow != null)
            {
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Enter) 
                {
                    CurrencyManager cmgr = (CurrencyManager)this.dGV_AllBrigade.BindingContext[this.dGV_AllBrigade.DataSource, dGV_AllBrigade.DataMember];
                    DataRow row = ((DataRowView)cmgr.Current).Row;
                    int PK_IdBrigade = Convert.ToInt32(row["PK_IdBrigade"]);
                    bool IsValid;
                    if (e.KeyCode == Keys.Delete) IsValid = false; else IsValid = true;
                    brigade.UpdateIsValidBrigade(PK_IdBrigade, IsValid);//Update IsValid
                    row.BeginEdit();
                    row["IsValid"] = IsValid;
                    row.EndEdit();
                }
            }
            
        }

    }
}
