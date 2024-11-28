using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dispetcher2
{
    public partial class NoteForm : Form
    {
        public string NoteText
        {
            get { return textBoxNote.Text; }
            set { textBoxNote.Text = value; }
        }

        public void ClearTextBoxVis(bool flag)
        {
            changeBTN.Visible = flag;
        }


        public NoteForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textBoxNote_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void clearTextBox_Click(object sender, EventArgs e)
        {
            textBoxNote.Clear();
        }

        private void btnNO_Click(object sender, EventArgs e)
        {
            changeBTN.Visible = false;
            this.Close();
        }

        private void changeBTN_Click(object sender, EventArgs e)
        {
            string newNote = textBoxNote.Text.Trim();

            if (string.IsNullOrEmpty(newNote))
            {
                // Если текст пустой, удаляем заметку
                NoteText = string.Empty;
            }
            else
            {
                // Обновляем заметку
                NoteText = newNote;
            }

            // Закрываем форму с DialogResult.OK, чтобы вызвать обновление в вызывающем коде
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
