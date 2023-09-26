using System;
using System.Windows.Forms;
using Dispetcher2.Class;
using Microsoft.Office.Interop.Word;

namespace Dispetcher2
{
    public partial class F_Index : Form
    {
        FormFactory factory;
        public F_Index(FormFactory factory)
        {
            this.factory = factory;
            InitializeComponent();
            
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

//        #region MDI methods
//        private void CloseAllMDIchild()
//        {
//            //Закрытие всех дочерних форм
//            foreach (var form in this.MdiChildren) form.Close();
//        }

//        //DoSomething<TSomeType>() where TSomeType : IMyInterface, new() {...}
//        //private void CheckAndCreateMDI<TForm>(string NameForm) where TForm : Form, new()
//        private void CheckAndCreateMDI(Form f)
//        {
//            /*if (this.MdiChildren.Count() == 0) CreateMDI<TForm>();
//            else
//            {
//                bool CheckForm = false;
//                foreach (var form in this.MdiChildren)
//                {
//                    if (form.Name == NameForm)
//                    {
//                        if (NameForm != "F_Fact")
//                        {
//                            CheckForm = false;
//                            form.Close();
//                        }
//                        else
//                        {
//                            CheckForm = true;
//                            form.Select();
//                        }
//                        break;
//                    }
//                }
//                if (!CheckForm) CreateMDI<TForm>();
//            }*/
//            CloseAllMDIchild();
//            CreateMDI(f);
//        }
//        // private void CreateMDI<TForm>() where TForm : Form, new()
//        private void CreateMDI(Form f)
//        {
//            var MDIChild = f;
//            MDIChild.MdiParent = this;
//            MDIChild.FormBorderStyle = FormBorderStyle.None;
//            MDIChild.Dock = DockStyle.Fill;
//            MDIChild.Show();
//        }
//#endregion
        void Show(Form MDIChild)
        {
            //Закрытие всех дочерних форм
            foreach (var form in this.MdiChildren)
            {
                form.Close();
                form.Dispose();
            }

            MDIChild.MdiParent = this;
            MDIChild.FormBorderStyle = FormBorderStyle.None;
            MDIChild.Dock = DockStyle.Fill;
            MDIChild.Show();
        }

        #region Клики по пунктам меню

        private void рабочиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " - Рабочие";
            var f = factory.GetForm("Рабочие");
            Show(f);
        }

        private void бригадыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " - Бригады";
            var f = factory.GetForm("Бригады");
            Show(f);
        }

        private void производственныйКалендарьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " - Производственный календарь";
            var f = factory.GetForm("Производственный календарь");
            Show(f);
        }

        private void табельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " - Табель учёта рабочего времени";
            var f = factory.GetForm("Табель учёта рабочего времени");
            Show(f);
        }


        private void toolStripMenuItemUsers_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " - Пользователи";
            var f = factory.GetForm("Пользователи");
            Show(f);
        }

        private void toolStripMenuItemOther_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " - Настройки администратора";
            var f = factory.GetForm("Настройки администратора");
            Show(f);
        }

        private void tSB_Orders_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " - Заказы";
            var f = factory.GetForm("Заказы");
            Show(f);
        }

        private void tSB_Facts_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " - Фактически выполненные операции";
            var f = factory.GetForm("Фактически выполненные операции");
            Show(f);
        }

        private void tSB_Kit_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " - Комплектация";
            var f = factory.GetForm("Комплектация");
            Show(f);
        }

        private void отчётнарядПоВыполненнымОперациямToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " - Отчёт-наряд по выполненным операциям";
            var f = factory.GetForm("Отчёт-наряд по выполненным операциям");
            Show(f);
        }

        private void операцииВыполненныеРабочимПоЗаказамформа17ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " - Операции выполненные рабочим по заказам (форма №17)";
            var f = factory.GetForm("Операции выполненные рабочим по заказам");
            Show(f);
        }

        private void движениеДеталейToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " - Движение деталей";
            var f = factory.GetForm("Движение деталей");
            Show(f);
        }

        private void отчетПоВыполненнымОперациямразрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " - Отчет по выполненным операциям";
            var f = factory.GetForm("Отчет по выполненным операциям");
            Show(f);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " - ПРОИЗВОДСТВО-ПЛАН";
            var f = factory.GetForm("ПРОИЗВОДСТВО-ПЛАН");
            Show(f);
        }

        private void планграфикформа6ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " - План-график (форма №6)";
            var f = factory.GetForm("План-график");
            Show(f);
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

            var f = factory.GetForm("Лого");
            Show(f);
            toolStripStatusLabel1.Text = factory.GetInformation();
        }

        //Лишнее
        private void F_Index_SizeChanged(object sender, EventArgs e)
        {
            //toolStripStatusLabel1.Text = this.Size.ToString();
        }

        private void laborToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Text = "ПО \"Диспетчер\"" + " — Трудоемкость";
            var f = factory.GetForm("Трудоемкость");
            Show(f);
        }
    }
}
