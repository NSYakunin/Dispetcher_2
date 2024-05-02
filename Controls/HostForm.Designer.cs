namespace Dispetcher2.Controls
{
    partial class HostForm
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
            this.MainElementHost = new System.Windows.Forms.Integration.ElementHost();
            this.SuspendLayout();
            // 
            // MainElementHost
            // 
            this.MainElementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainElementHost.Location = new System.Drawing.Point(0, 0);
            this.MainElementHost.Name = "MainElementHost";
            this.MainElementHost.Size = new System.Drawing.Size(800, 450);
            this.MainElementHost.TabIndex = 0;
            this.MainElementHost.Text = "MainElementHost";
            this.MainElementHost.Child = null;
            // 
            // HostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MainElementHost);
            this.Name = "HostForm";
            this.Text = "HostForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HostForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost MainElementHost;
    }
}