using System;
using System.Windows.Forms;

namespace Dispetcher2.DialogsForms
{
    public partial class RenameFileForm : Form
    {
        public string NewFileName { get; private set; }

        public RenameFileForm(string currentFileName)
        {
            InitializeComponent();
            txtCurrentFileName.Text = currentFileName;
            txtNewFileName.Text = currentFileName;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewFileName.Text))
            {
                MessageBox.Show("Введите новое имя файла.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NewFileName = txtNewFileName.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}