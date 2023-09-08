using System.Collections.Specialized;
using System.Configuration;

namespace Dispetcher2
{
    partial class F_Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_Login));
            this.lbl_User = new System.Windows.Forms.Label();
            this.tLP_Autorization = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_Enter = new System.Windows.Forms.Label();
            this.tB_ServerMessage = new System.Windows.Forms.TextBox();
            this.tB_Password = new System.Windows.Forms.TextBox();
            this.lbl_ServerMessage = new System.Windows.Forms.Label();
            this.lbl_Password = new System.Windows.Forms.Label();
            this.lbl_Welcome = new System.Windows.Forms.Label();
            this.gB_NewLogin = new System.Windows.Forms.GroupBox();
            this.tB_NewLogin = new System.Windows.Forms.TextBox();
            this.mychB_NewLogin = new Dispetcher2.Controls.MyCheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tLP_Autorization.SuspendLayout();
            this.gB_NewLogin.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_User
            // 
            this.lbl_User.AutoSize = true;
            this.lbl_User.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_User.Location = new System.Drawing.Point(23, 474);
            this.lbl_User.Name = "lbl_User";
            this.lbl_User.Size = new System.Drawing.Size(417, 31);
            this.lbl_User.TabIndex = 0;
            this.lbl_User.Text = "Логин:";
            this.lbl_User.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // tLP_Autorization
            // 
            this.tLP_Autorization.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.tLP_Autorization.ColumnCount = 5;
            this.tLP_Autorization.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tLP_Autorization.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tLP_Autorization.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tLP_Autorization.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tLP_Autorization.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tLP_Autorization.Controls.Add(this.lbl_Enter, 1, 10);
            this.tLP_Autorization.Controls.Add(this.tB_ServerMessage, 1, 3);
            this.tLP_Autorization.Controls.Add(this.tB_Password, 2, 9);
            this.tLP_Autorization.Controls.Add(this.lbl_ServerMessage, 1, 1);
            this.tLP_Autorization.Controls.Add(this.lbl_Password, 1, 8);
            this.tLP_Autorization.Controls.Add(this.lbl_Welcome, 1, 5);
            this.tLP_Autorization.Controls.Add(this.gB_NewLogin, 2, 6);
            this.tLP_Autorization.Controls.Add(this.lbl_User, 1, 13);
            this.tLP_Autorization.Controls.Add(this.mychB_NewLogin, 2, 13);
            this.tLP_Autorization.Controls.Add(this.panel1, 2, 11);
            this.tLP_Autorization.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLP_Autorization.Location = new System.Drawing.Point(0, 0);
            this.tLP_Autorization.Name = "tLP_Autorization";
            this.tLP_Autorization.RowCount = 14;
            this.tLP_Autorization.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tLP_Autorization.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tLP_Autorization.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tLP_Autorization.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tLP_Autorization.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tLP_Autorization.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tLP_Autorization.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tLP_Autorization.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tLP_Autorization.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tLP_Autorization.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tLP_Autorization.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tLP_Autorization.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tLP_Autorization.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tLP_Autorization.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tLP_Autorization.Size = new System.Drawing.Size(1106, 505);
            this.tLP_Autorization.TabIndex = 1;
            // 
            // lbl_Enter
            // 
            this.lbl_Enter.AutoSize = true;
            this.tLP_Autorization.SetColumnSpan(this.lbl_Enter, 3);
            this.lbl_Enter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Enter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_Enter.Location = new System.Drawing.Point(23, 351);
            this.lbl_Enter.Name = "lbl_Enter";
            this.lbl_Enter.Size = new System.Drawing.Size(1060, 15);
            this.lbl_Enter.TabIndex = 1000;
            this.lbl_Enter.Text = "Нажмите \"Enter\"";
            this.lbl_Enter.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tB_ServerMessage
            // 
            this.tB_ServerMessage.BackColor = System.Drawing.SystemColors.Info;
            this.tLP_Autorization.SetColumnSpan(this.tB_ServerMessage, 3);
            this.tB_ServerMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_ServerMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_ServerMessage.Location = new System.Drawing.Point(23, 71);
            this.tB_ServerMessage.Multiline = true;
            this.tB_ServerMessage.Name = "tB_ServerMessage";
            this.tB_ServerMessage.ReadOnly = true;
            this.tB_ServerMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tB_ServerMessage.ShortcutsEnabled = false;
            this.tB_ServerMessage.Size = new System.Drawing.Size(1060, 120);
            this.tB_ServerMessage.TabIndex = 999;
            this.tB_ServerMessage.TabStop = false;
            // 
            // tB_Password
            // 
            this.tB_Password.BackColor = System.Drawing.SystemColors.HighlightText;
            this.tB_Password.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_Password.Location = new System.Drawing.Point(446, 320);
            this.tB_Password.MaxLength = 10;
            this.tB_Password.Name = "tB_Password";
            this.tB_Password.PasswordChar = '*';
            this.tB_Password.Size = new System.Drawing.Size(214, 29);
            this.tB_Password.TabIndex = 0;
            this.tB_Password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tB_Password_KeyDown);
            // 
            // lbl_ServerMessage
            // 
            this.lbl_ServerMessage.AutoSize = true;
            this.tLP_Autorization.SetColumnSpan(this.lbl_ServerMessage, 3);
            this.lbl_ServerMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_ServerMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_ServerMessage.ForeColor = System.Drawing.Color.Brown;
            this.lbl_ServerMessage.Location = new System.Drawing.Point(23, 40);
            this.lbl_ServerMessage.Name = "lbl_ServerMessage";
            this.lbl_ServerMessage.Size = new System.Drawing.Size(1060, 20);
            this.lbl_ServerMessage.TabIndex = 1;
            this.lbl_ServerMessage.Text = "Лента новостей";
            this.lbl_ServerMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Password
            // 
            this.lbl_Password.AutoSize = true;
            this.tLP_Autorization.SetColumnSpan(this.lbl_Password, 3);
            this.lbl_Password.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_Password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_Password.Location = new System.Drawing.Point(23, 297);
            this.lbl_Password.Name = "lbl_Password";
            this.lbl_Password.Size = new System.Drawing.Size(1060, 20);
            this.lbl_Password.TabIndex = 2;
            this.lbl_Password.Text = "Введите свой пароль";
            this.lbl_Password.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_Welcome
            // 
            this.lbl_Welcome.AutoSize = true;
            this.lbl_Welcome.Location = new System.Drawing.Point(23, 202);
            this.lbl_Welcome.Name = "lbl_Welcome";
            this.lbl_Welcome.Size = new System.Drawing.Size(107, 13);
            this.lbl_Welcome.TabIndex = 3;
            this.lbl_Welcome.Text = "Добро пожаловать:";
            this.lbl_Welcome.Visible = false;
            // 
            // gB_NewLogin
            // 
            this.gB_NewLogin.Controls.Add(this.tB_NewLogin);
            this.gB_NewLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gB_NewLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.gB_NewLogin.Location = new System.Drawing.Point(446, 225);
            this.gB_NewLogin.Name = "gB_NewLogin";
            this.gB_NewLogin.Size = new System.Drawing.Size(214, 54);
            this.gB_NewLogin.TabIndex = 1004;
            this.gB_NewLogin.TabStop = false;
            this.gB_NewLogin.Text = "Логин пользователя:";
            // 
            // tB_NewLogin
            // 
            this.tB_NewLogin.BackColor = System.Drawing.SystemColors.HighlightText;
            this.tB_NewLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_NewLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tB_NewLogin.Location = new System.Drawing.Point(3, 22);
            this.tB_NewLogin.MaxLength = 20;
            this.tB_NewLogin.Name = "tB_NewLogin";
            this.tB_NewLogin.Size = new System.Drawing.Size(208, 29);
            this.tB_NewLogin.TabIndex = 1;
            this.tB_NewLogin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tB_NewLogin_KeyDown);
            // 
            // mychB_NewLogin
            // 
            this.mychB_NewLogin.Appearance = System.Windows.Forms.Appearance.Button;
            this.mychB_NewLogin.AutoSize = true;
            this.mychB_NewLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mychB_NewLogin.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.mychB_NewLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mychB_NewLogin.Location = new System.Drawing.Point(446, 477);
            this.mychB_NewLogin.Name = "mychB_NewLogin";
            this.mychB_NewLogin.Size = new System.Drawing.Size(214, 25);
            this.mychB_NewLogin.TabIndex = 1005;
            this.mychB_NewLogin.Text = "Сменить пользователя";
            this.mychB_NewLogin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mychB_NewLogin.UseVisualStyleBackColor = true;
            this.mychB_NewLogin.CheckedChanged += new System.EventHandler(this.mychB_NewLogin_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(446, 369);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(214, 78);
            this.panel1.TabIndex = 1007;
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "main",
            "test"});
            this.comboBox1.Location = new System.Drawing.Point(39, 43);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(142, 32);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(35, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Выберите сервер:";
            // 
            // F_Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(1106, 505);
            this.Controls.Add(this.tLP_Autorization);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "F_Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ПО \"Диспетчер\"";
            this.Load += new System.EventHandler(this.F_Login_Load);
            this.tLP_Autorization.ResumeLayout(false);
            this.tLP_Autorization.PerformLayout();
            this.gB_NewLogin.ResumeLayout(false);
            this.gB_NewLogin.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_User;
        private System.Windows.Forms.TableLayoutPanel tLP_Autorization;
        private System.Windows.Forms.Label lbl_ServerMessage;
        private System.Windows.Forms.Label lbl_Password;
        private System.Windows.Forms.Label lbl_Welcome;
        private System.Windows.Forms.TextBox tB_Password;
        private System.Windows.Forms.TextBox tB_ServerMessage;
        private System.Windows.Forms.Label lbl_Enter;
        private System.Windows.Forms.GroupBox gB_NewLogin;
        private System.Windows.Forms.TextBox tB_NewLogin;
        private Controls.MyCheckBox mychB_NewLogin;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
    }
}