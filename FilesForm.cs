using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Dispetcher2.DialogsForms
{
    public partial class FilesForm : Form
    {
        private string targetDirectory;

        public FilesForm(string targetDirectory)
        {
            InitializeComponent();
            this.targetDirectory = targetDirectory;
        }

        private void FilesForm_Load(object sender, EventArgs e)
        {
            LoadFiles();
        }

        private void LoadFiles()
        {
            listViewFiles.Items.Clear();
            string[] files = Directory.GetFiles(targetDirectory);

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                ListViewItem item = new ListViewItem(fileInfo.Name);
                item.SubItems.Add(fileInfo.Length.ToString() + " байт");
                item.SubItems.Add(fileInfo.CreationTime.ToString());
                item.SubItems.Add(fileInfo.LastWriteTime.ToString());
                listViewFiles.Items.Add(item);
            }

            // Автоматически подгоняем ширину столбцов под содержимое
            foreach (ColumnHeader column in listViewFiles.Columns)
            {
                column.Width = -2; // -2 означает авто-ширину по содержимому
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    string destFileName = Path.Combine(targetDirectory, Path.GetFileName(fileName));
                    // Проверяем, существует ли файл с таким именем
                    if (File.Exists(destFileName))
                    {
                        // Если файл существует, предлагаем перезаписать или пропустить
                        DialogResult result = MessageBox.Show(
                            $"Файл {Path.GetFileName(fileName)} уже существует. Перезаписать?",
                            "Файл существует",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.No)
                        {
                            continue;
                        }
                    }
                    try
                    {
                        File.Copy(fileName, destFileName, true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при копировании файла {Path.GetFileName(fileName)}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                LoadFiles();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "Вы действительно хотите удалить выбранные файлы?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    foreach (ListViewItem item in listViewFiles.SelectedItems)
                    {
                        string filePath = Path.Combine(targetDirectory, item.Text);
                        try
                        {
                            File.Delete(filePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при удалении файла {item.Text}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    LoadFiles();
                }
            }
            else
            {
                MessageBox.Show("Выберите файлы для удаления.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void listViewFiles_DoubleClick(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count > 0)
            {
                string filePath = Path.Combine(targetDirectory, listViewFiles.SelectedItems[0].Text);
                try
                {
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии файла {listViewFiles.SelectedItems[0].Text}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}