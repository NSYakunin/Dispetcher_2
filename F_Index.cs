using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dispetcher2.Class;
using Microsoft.Office.Interop.Word;

namespace Dispetcher2
{
    public partial class F_Index : Form
    {
        public F_Index()
        {
            InitializeComponent();
            this.toolStripStatusLabel1.Text = C_Gper.GetServerName();
        }

        #region Настройка поведения выпадающих списков пунктов меню
        private void tSSB_Reports_ButtonClick(object sender, EventArgs e)
        {
            //this.tSSB_Reports.DropDown.AutoClose = false;
            if (tSSB_Reports.DropDownButtonPressed) tSSB_Reports.DropDown.Close(); else tSSB_Reports.ShowDropDown();
            //this.tSSB_Reports.DropDown.AutoClose = true;
        }

        private void tSSB_Planning_ButtonClick(object sender, EventArgs e)
        {
            //this.tSSB_Planning.DropDown.AutoClose = false;
            if (tSSB_Planning.DropDownButtonPressed) tSSB_Planning.DropDown.Close(); else tSSB_Planning.ShowDropDown();
            //this.tSSB_Planning.DropDown.AutoClose = true;
        }

        private void tSSB_Users_ButtonClick(object sender, EventArgs e)
        {
            if (tSSB_Users.DropDownButtonPressed) tSSB_Users.DropDown.Close(); else tSSB_Users.ShowDropDown();
        }

        private void tSSB_Settings_ButtonClick(object sender, EventArgs e)
        {
            if (tSSB_Settings.DropDownButtonPressed) tSSB_Settings.DropDown.Close(); else tSSB_Settings.ShowDropDown();
        }

        private void tSSB_Help_ButtonClick(object sender, EventArgs e)
        {
            if (tSSB_Help.DropDownButtonPressed) tSSB_Help.DropDown.Close(); else tSSB_Help.ShowDropDown();
        }

        #endregion

        #region MDI methods
        private void CloseAllMDIchild()
        {
            //Получаем ссылку на активную дочернюю форму и закрываем её
            //Form frmchild = this.ActiveMdiChild;
            //frmchild.Close();
            foreach (var form in this.MdiChildren) form.Close();//Закрытие всех дочерних форм
        }

        //DoSomething<TSomeType>() where TSomeType : IMyInterface, new() {...}
        private void CheckAndCreateMDI<TForm>(string NameForm) where TForm : Form, new()
        {
            /*if (this.MdiChildren.Count() == 0) CreateMDI<TForm>();
            else
            {
                bool CheckForm = false;
                foreach (var form in this.MdiChildren)
                {
                    if (form.Name == NameForm)
                    {
                        if (NameForm != "F_Fact")
                        {
                            CheckForm = false;
                            form.Close();
                        }
                        else
                        {
                            CheckForm = true;
                            form.Select();
                        }
                        break;
                    }
                }
                if (!CheckForm) CreateMDI<TForm>();
            }*/
            CloseAllMDIchild();
            CreateMDI<TForm>();
        }

        private void CreateMDI<TForm>() where TForm : Form, new()
        {
            TForm MDIChild = new TForm();
            MDIChild.MdiParent = this;
            MDIChild.FormBorderStyle = FormBorderStyle.None;
            MDIChild.Dock = DockStyle.Fill;
            //this.MinimumSize = new Size(MDIChild.MinimumSize.Width+20, MDIChild.MinimumSize.Height + 100);
            MDIChild.Show();
        }
#endregion

        #region Клики по пунктам меню

        private void рабочиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckAndCreateMDI<F_Workers>("F_Workers");
            this.Text = "ПО \"Диспетчер\"" + " - Рабочие";
            //toolStripStatusLabel1.Text = this.MdiChildren.Count().ToString();
        }

        private void бригадыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckAndCreateMDI<F_Brigade>("F_Brigade");
            this.Text = "ПО \"Диспетчер\"" + " - Бригады";
            //toolStripStatusLabel1.Text = this.MdiChildren.Count().ToString();
        }

        private void производственныйКалендарьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckAndCreateMDI<F_ProductionCalendar>("F_ProductionCalendar");
            this.Text = "ПО \"Диспетчер\"" + " - Производственный календарь";
            //toolStripStatusLabel1.Text = this.MdiChildren.Count().ToString();
        }

        private void табельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckAndCreateMDI<F_TimeSheets>("F_TimeSheets");
            this.Text = "ПО \"Диспетчер\"" + " - Табель учёта рабочего времени";
            //toolStripStatusLabel1.Text = this.MdiChildren.Count().ToString();
        }


        private void toolStripMenuItemUsers_Click(object sender, EventArgs e)
        {
            CheckAndCreateMDI<F_Users>("F_Users");
            this.Text = "ПО \"Диспетчер\"" + " - Пользователи";
            //toolStripStatusLabel1.Text = this.MdiChildren.Count().ToString();
        }

        private void toolStripMenuItemOther_Click(object sender, EventArgs e)
        {
            CheckAndCreateMDI<F_Settings>("F_Settings");
            this.Text = "ПО \"Диспетчер\"" + " - Настройки администратора";
            //toolStripStatusLabel1.Text = this.MdiChildren.Count().ToString();
        }

        private void tSB_Orders_Click(object sender, EventArgs e)
        {
            CheckAndCreateMDI<F_Orders>("F_Orders");
            this.Text = "ПО \"Диспетчер\"" + " - Заказы";
            //toolStripStatusLabel1.Text = this.MdiChildren.Count().ToString();
        }

        private void tSB_Facts_Click(object sender, EventArgs e)
        {
            CheckAndCreateMDI<F_Fact>("F_Fact");
            this.Text = "ПО \"Диспетчер\"" + " - Фактически выполненные операции";
            //toolStripStatusLabel1.Text = this.MdiChildren.Count().ToString();
        }

        private void tSB_Kit_Click(object sender, EventArgs e)
        {
            //Комплектация
            CheckAndCreateMDI<F_Kit>("F_Kit");
            this.Text = "ПО \"Диспетчер\"" + " - Комплектация";
            //toolStripStatusLabel1.Text = this.MdiChildren.Count().ToString();
        }

        private void отчётнарядПоВыполненнымОперациямToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            C_Gper.NameReport = 3;
            CheckAndCreateMDI<F_Reports>("F_Reports");
            this.Text = "ПО \"Диспетчер\"" + " - Отчёт-наряд по выполненным операциям";
            //toolStripStatusLabel1.Text = this.MdiChildren.Count().ToString();
        }

        private void операцииВыполненныеРабочимПоЗаказамформа17ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            C_Gper.NameReport = 117;
            CheckAndCreateMDI<F_Reports>("F_Reports");
            this.Text = "ПО \"Диспетчер\"" + " - Операции выполненные рабочим по заказам (форма №17)";
            //toolStripStatusLabel1.Text = this.MdiChildren.Count().ToString();
        }

        private void движениеДеталейToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            C_Gper.NameReport = 6;
            CheckAndCreateMDI<F_Reports>("F_Reports");
            this.Text = "ПО \"Диспетчер\"" + " - Движение деталей";
            //toolStripStatusLabel1.Text = this.MdiChildren.Count().ToString();
        }

        private void отчетПоВыполненнымОперациямразрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            C_Gper.NameReport = 7;
            CheckAndCreateMDI<F_Reports>("F_Reports");
            this.Text = "ПО \"Диспетчер\"" + " - Отчет по выполненным операциям";
            //toolStripStatusLabel1.Text = this.MdiChildren.Count().ToString();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CheckAndCreateMDI<F_Planning>("F_Planning");
            this.Text = "ПО \"Диспетчер\"" + " - ПРОИЗВОДСТВО-ПЛАН";
            //toolStripStatusLabel1.Text = this.MdiChildren.Count().ToString();
        }

        private void планграфикформа6ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            C_Gper.NameReport = 106;
            CheckAndCreateMDI<F_ReportsPlan>("F_ReportsPlan");
            this.Text = "ПО \"Диспетчер\"" + " - План-график (форма №6)";
            //toolStripStatusLabel1.Text = this.MdiChildren.Count().ToString();
        }

        private void toolStripMI_HelpUser_Click(object sender, EventArgs e)
        {
            Type officeType = Type.GetTypeFromProgID("Word.Application");
            if (officeType == null)
                MessageBox.Show("Не установлена программа \"Microsoft Word\".", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //Вызываем файл
                try
                {
                    string WayFile = @"\\Loodsman\Dispetcher\UpdatesDisp2\Helps\Диспетчеризация_Рук_во_опер.docx";
                    System.Diagnostics.Process.Start(WayFile);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void toolStripMI_HelpAdmin_Click(object sender, EventArgs e)
        {
            Type officeType = Type.GetTypeFromProgID("Word.Application");
            if (officeType == null)
                MessageBox.Show("Не установлена программа \"Microsoft Word\".", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //Вызываем файл
                try
                {
                    string WayFile = @"\\Loodsman\Dispetcher\UpdatesDisp2\Helps\Диспетчеризация_Рук_во_администратора.docx";
                    System.Diagnostics.Process.Start(WayFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        #endregion

        private void F_Index_Load(object sender, EventArgs e)
        {
            //tSB_Orders.PerformClick();//Форма заказов
            //UsersAccess
            if (C_Gper.Orders_Set) tSB_Orders.Visible = true; else tSB_Orders.Visible = false;
            if (C_Gper.Fact_Set) tSB_Facts.Visible = true; else tSB_Facts.Visible = false;
            if (C_Gper.Kit_Set) tSB_Kit.Visible = true; else tSB_Kit.Visible = false;
            //if (C_Gper.Technology_Set) tSB_Technology.Visible = true; else tSB_Technology.Visible = false;
            if (C_Gper.Planning_Set) tSSB_Planning.Visible = true; else tSSB_Planning.Visible = false;
            if (C_Gper.Reports_Set) tSSB_Reports.Visible = true; else tSSB_Reports.Visible = false;
            if (C_Gper.Users_Set) tSSB_Users.Visible = true; else tSSB_Users.Visible = false;
            if (C_Gper.Settings_Set) tSSB_Settings.Visible = true; else tSSB_Settings.Visible = false;
            tSB_Technology.Visible = false;
            //tSB_Settings.Visible = false;
            CheckAndCreateMDI<F_IndexLogo>("F_IndexLogo");
        }

        //Лишнее
        private void F_Index_SizeChanged(object sender, EventArgs e)
        {
            //toolStripStatusLabel1.Text = this.Size.ToString();
        }

        private void laborToolStripMenuItem_Click(object sender, EventArgs e)
        {
            C_Gper.NameReport = 66;
            CheckAndCreateMDI<F_Reports>("F_Reports");
            this.Text = "ПО \"Диспетчер\"" + " — Трудоемкость";
        }
    }
}
