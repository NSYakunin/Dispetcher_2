using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.SqlClient;
using Dispetcher2.Class;

namespace Dispetcher2.DialogsForms
{
    public partial class FilesForm : Form
    {
        private string targetDirectory;
        private int operationID;
        private string currentUser;
        public Configuration conf = new Configuration();


        private bool autoOpenAddDialog;

        private bool autoCloseAfterAdd;

        public FilesForm(string targetDirectory, int operationID, bool autoOpenAddDialog = false, bool autoCloseAfterAdd = false)
        {
            InitializeComponent();
            this.targetDirectory = targetDirectory;
            this.operationID = operationID;
            this.currentUser = Environment.UserName;
            this.autoOpenAddDialog = autoOpenAddDialog;
            this.autoCloseAfterAdd = autoCloseAfterAdd;
        }

        private void FilesForm_Load(object sender, EventArgs e)
        {
            LoadFiles();

            if (autoOpenAddDialog)
            {
                btnAdd_Click(this, EventArgs.Empty);
            }
        }

        private void LoadFiles()
        {
            listViewFiles.Items.Clear();

            // Получаем файлы из базы данных для текущего OperationID
            DataTable filesTable = GetFilesFromDatabase();

            foreach (DataRow row in filesTable.Rows)
            {
                string fileName = row["FileName"].ToString();
                string filePath = Path.Combine(targetDirectory, fileName);
                string uploadDate = Convert.ToDateTime(row["UploadDate"]).ToString();
                string uploadedBy = row["UploadedBy"].ToString();

                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    ListViewItem item = new ListViewItem(fileName);
                    item.SubItems.Add(fileInfo.Length.ToString() + " байт");
                    item.SubItems.Add(uploadDate);
                    item.SubItems.Add(uploadedBy);
                    listViewFiles.Items.Add(item);
                }
                else
                {
                    // Если файла нет на диске, удаляем запись из базы данных
                    DeleteFileFromDatabase(fileName);
                }
            }

            // Автоматически подгоняем ширину столбцов под содержимое
            foreach (ColumnHeader column in listViewFiles.Columns)
            {
                column.Width = -2; // -2 означает авто-ширину по содержимому
            }
        }

        private DataTable GetFilesFromDatabase()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(conf.ConnectionString))
            {
                string query = "SELECT FileID, FileName, FilePath, UploadDate, UploadedBy FROM OperationFiles WHERE OperationID = @OperationID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OperationID", operationID);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
            }

            return dataTable;
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

                        // Сохраняем информацию о файле в базе данных
                        SaveFileToDatabase(Path.GetFileName(fileName), destFileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при копировании файла {Path.GetFileName(fileName)}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                LoadFiles();
            }
            if (autoCloseAfterAdd)
            {
                this.Close();
            }
        }

        private void SaveFileToDatabase(string fileName, string filePath)
        {
            using (SqlConnection connection = new SqlConnection(conf.ConnectionString))
            {
                string query = "INSERT INTO OperationFiles (OperationID, FileName, FilePath, UploadDate, UploadedBy) VALUES (@OperationID, @FileName, @FilePath, @UploadDate, @UploadedBy)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OperationID", operationID);
                command.Parameters.AddWithValue("@FileName", fileName);
                command.Parameters.AddWithValue("@FilePath", filePath);
                command.Parameters.AddWithValue("@UploadDate", DateTime.Now);
                command.Parameters.AddWithValue("@UploadedBy", currentUser);

                connection.Open();
                command.ExecuteNonQuery();
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
                        string fileName = item.Text;
                        string filePath = Path.Combine(targetDirectory, fileName);
                        try
                        {
                            File.Delete(filePath);

                            // Удаляем информацию о файле из базы данных
                            DeleteFileFromDatabase(fileName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при удалении файла {fileName}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void DeleteFileFromDatabase(string fileName)
        {
            using (SqlConnection connection = new SqlConnection(conf.ConnectionString))
            {
                string query = "DELETE FROM OperationFiles WHERE OperationID = @OperationID AND FileName = @FileName";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OperationID", operationID);
                command.Parameters.AddWithValue("@FileName", fileName);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private void listViewFiles_DoubleClick(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count > 0)
            {
                string fileName = listViewFiles.SelectedItems[0].Text;
                string filePath = Path.Combine(targetDirectory, fileName);
                try
                {
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии файла {fileName}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 1)
            {
                ListViewItem selectedItem = listViewFiles.SelectedItems[0];
                string oldFileName = selectedItem.Text;
                string oldFilePath = Path.Combine(targetDirectory, oldFileName);

                using (RenameFileForm renameForm = new RenameFileForm(oldFileName))
                {
                    if (renameForm.ShowDialog() == DialogResult.OK)
                    {
                        string newFileName = renameForm.NewFileName;
                        string newFilePath = Path.Combine(targetDirectory, newFileName);

                        if (File.Exists(newFilePath))
                        {
                            MessageBox.Show("Файл с таким именем уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        try
                        {
                            File.Move(oldFilePath, newFilePath);

                            // Обновляем информацию о файле в базе данных
                            RenameFileInDatabase(oldFileName, newFileName, newFilePath);

                            LoadFiles();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при переименовании файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите один файл для переименования.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RenameFileInDatabase(string oldFileName, string newFileName, string newFilePath)
        {
            using (SqlConnection connection = new SqlConnection(conf.ConnectionString))
            {
                string query = "UPDATE OperationFiles SET FileName = @NewFileName, FilePath = @NewFilePath WHERE OperationID = @OperationID AND FileName = @OldFileName";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NewFileName", newFileName);
                command.Parameters.AddWithValue("@NewFilePath", newFilePath);
                command.Parameters.AddWithValue("@OperationID", operationID);
                command.Parameters.AddWithValue("@OldFileName", oldFileName);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}