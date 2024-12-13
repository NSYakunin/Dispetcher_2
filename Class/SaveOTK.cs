using Dispetcher2.DialogsForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dispetcher2.Class
{
    public class SaveOTK
    {
        private IConfig config;
        DataGridView dGV_Details;
        DataGridView dGV_Tehnology;

        public SaveOTK(DataGridView dgv1, DataGridView dgv2, IConfig config) 
        {
            dGV_Details = dgv1;
            dGV_Tehnology = dgv2;
            this.config = config;
        }

        public void SaveMethod()
        {
            StringBuilder sb = new StringBuilder();

            // Получаем выбранную подробную информацию из dGV_Details
            string shcmDetail = "";
            string pk_IdOrderDetail = "";
            if (dGV_Details.CurrentRow != null)
            {
                DataRowView detailRowView = dGV_Details.CurrentRow.DataBoundItem as DataRowView;
                if (detailRowView != null)
                {
                    DataRow detailRow = detailRowView.Row;
                    shcmDetail = detailRow["ShcmDetail"].ToString();
                    pk_IdOrderDetail = detailRow["PK_IdOrderDetail"].ToString();
                }
            }
            bool hasChanges = false;
            bool hasStateChanges = false;
            string currentUser = Environment.UserName;

            List<OperationData> operationsToSave = new List<OperationData>();

            foreach (DataGridViewRow row in dGV_Tehnology.Rows)
            {
                if (row.IsNewRow) continue;

                DataRow dataRow = ((DataRowView)row.DataBoundItem).Row;

                OTKControlData currentData = dataRow["OTKControlData"] as OTKControlData;
                OTKControlData originalData = dataRow["OriginalOTKControlData"] as OTKControlData;

                bool isChanged = !AreOTKControlDataEqual(currentData, originalData);
                bool isStateChanged = !AreOTKControlDataStatesEqual(currentData, originalData);

                if (isChanged)
                {
                    hasChanges = true;
                }
                if (isStateChanged)
                {
                    hasStateChanges = true;
                }

                // Собираем данные операции
                OperationData opData = new OperationData
                {
                    PK_IdOrderDetail = Convert.ToInt64(pk_IdOrderDetail),
                    Oper = dataRow["Oper"].ToString(),
                    Tpd = dataRow["Tpd"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Tpd"]),
                    Tsh = dataRow["Tsh"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Tsh"]),
                    IdLoodsman = dataRow["IdLoodsman"] == DBNull.Value ? (long?)null : Convert.ToInt64(dataRow["IdLoodsman"]),
                    OTKControlData = currentData,
                    IsChanged = isChanged,
                    IsStateChanged = isStateChanged,
                    ChangeDate = DateTime.Now
                };

                operationsToSave.Add(opData);
            }

            if (hasChanges)
            {
                string note = null;
                if (hasStateChanges)
                {
                    // Если были изменения в состояниях чекбоксов, открываем окно заметки
                    NoteForm noteForm = new NoteForm();

                    if (noteForm.ShowDialog() == DialogResult.OK)
                    {
                        note = noteForm.NoteText;
                    }
                    else
                    {
                        // Если пользователь отменил ввод заметки, отменяем сохранение
                        return;
                    }
                }

                DateTime changeDate = DateTime.Now;
                SaveOperationsToDatabase(operationsToSave, note, currentUser, changeDate);
            }


            else
            {
                MessageBox.Show("Нет изменений для сохранения.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private DataRow FindDataRowByOper(string oper)
        {
            DataTable dataTable = dGV_Tehnology.DataSource as DataTable;
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row["Oper"].ToString() == oper)
                    {
                        return row;
                    }
                }
            }
            return null;
        }

        private bool AreOTKControlDataStatesEqual(OTKControlData data1, OTKControlData data2)
        {
            if (data1 == null || data2 == null)
                return false;

            if (!data1.States.SequenceEqual(data2.States))
                return false;

            return true;
        }

        public static bool AreOTKControlDataEqual(OTKControlData data1, OTKControlData data2)
        {
            if (data1 == null || data2 == null)
                return false;

            if (!data1.States.SequenceEqual(data2.States))
                return false;

            if (data1.Note != data2.Note)
                return false;

            return true;
        }

        private void SaveOperationsToDatabase(List<OperationData> operations, string note, string userName, DateTime changeDate)
        {
            try
            {
                using (var con = new SqlConnection(config.ConnectionString))
                {
                    con.Open();
                    using (var transaction = con.BeginTransaction())
                    {
                        foreach (var op in operations)
                        {
                            // Сначала вставляем или обновляем запись в таблице OperationsOTK
                            int operationID = InsertOrUpdateOperation(con, transaction, op);

                            // Проверяем, были ли изменения в OTKControlData
                            if (op.IsChanged && op.OTKControlData != null && op.OTKControlData.States != null)
                            {
                                for (int i = 0; i < op.OTKControlData.States.Length; i++)
                                {
                                    // Вставляем или обновляем записи в таблице OTKControl
                                    string noteToSave = note ?? op.OTKControlData.Note;

                                    InsertOrUpdateOTKControl(con, transaction, operationID, i, op.OTKControlData.States[i], changeDate, noteToSave, userName);
                                }
                            }
                        }

                        transaction.Commit();
                    }
                }

                MessageBox.Show("Данные успешно сохранены в базу данных.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int InsertOrUpdateOperation(SqlConnection con, SqlTransaction transaction, OperationData op)
        {
            string checkOperationQuery = "SELECT OperationID FROM OperationsOTK WHERE PK_IdOrderDetail = @PK_IdOrderDetail AND Oper = @Oper";
            SqlCommand checkCmd = new SqlCommand(checkOperationQuery, con, transaction);
            checkCmd.Parameters.AddWithValue("@PK_IdOrderDetail", op.PK_IdOrderDetail);
            checkCmd.Parameters.AddWithValue("@Oper", op.Oper);
            object result = checkCmd.ExecuteScalar();

            int operationID;
            if (result != null)
            {
                operationID = Convert.ToInt32(result);
                string updateOperationQuery = "UPDATE OperationsOTK SET Tpd = @Tpd, Tsh = @Tsh, IdLoodsman = @IdLoodsman WHERE OperationID = @OperationID";
                SqlCommand updateCmd = new SqlCommand(updateOperationQuery, con, transaction);
                updateCmd.Parameters.AddWithValue("@Tpd", (object)op.Tpd ?? DBNull.Value);
                updateCmd.Parameters.AddWithValue("@Tsh", (object)op.Tsh ?? DBNull.Value);
                updateCmd.Parameters.AddWithValue("@IdLoodsman", (object)op.IdLoodsman ?? DBNull.Value);
                updateCmd.Parameters.AddWithValue("@OperationID", operationID);
                updateCmd.ExecuteNonQuery();
            }
            else
            {
                string insertOperationQuery = "INSERT INTO OperationsOTK (PK_IdOrderDetail, Oper, Tpd, Tsh, IdLoodsman) OUTPUT INSERTED.OperationID VALUES (@PK_IdOrderDetail, @Oper, @Tpd, @Tsh, @IdLoodsman)";
                SqlCommand insertCmd = new SqlCommand(insertOperationQuery, con, transaction);
                insertCmd.Parameters.AddWithValue("@PK_IdOrderDetail", op.PK_IdOrderDetail);
                insertCmd.Parameters.AddWithValue("@Oper", op.Oper);
                insertCmd.Parameters.AddWithValue("@Tpd", (object)op.Tpd ?? DBNull.Value);
                insertCmd.Parameters.AddWithValue("@Tsh", (object)op.Tsh ?? DBNull.Value);
                insertCmd.Parameters.AddWithValue("@IdLoodsman", (object)op.IdLoodsman ?? DBNull.Value);
                operationID = (int)insertCmd.ExecuteScalar();
            }

            return operationID;
        }

        private void InsertOrUpdateOTKControl(SqlConnection con, SqlTransaction transaction, int operationID, int checkBoxIndex, CheckBoxState checkBoxState, DateTime changeDate, string note, string userName)
        {
            // Проверяем, существует ли запись в таблице OTKControl для данного operationID и checkBoxIndex
            string checkOTKControlQuery = "SELECT OTKControlID FROM OTKControl WHERE OperationID = @OperationID AND CheckBoxIndex = @CheckBoxIndex";
            SqlCommand checkCmd = new SqlCommand(checkOTKControlQuery, con, transaction);
            checkCmd.Parameters.AddWithValue("@OperationID", operationID);
            checkCmd.Parameters.AddWithValue("@CheckBoxIndex", checkBoxIndex);
            object result = checkCmd.ExecuteScalar();

            if (result != null)
            {
                // Запись существует, выполняем обновление
                int otkControlID = Convert.ToInt32(result);
                string updateOTKControlQuery = "UPDATE OTKControl SET CheckBoxState = @CheckBoxState, ChangeDate = @ChangeDate, Note = @Note, [User] = @User WHERE OTKControlID = @OTKControlID";
                SqlCommand updateCmd = new SqlCommand(updateOTKControlQuery, con, transaction);
                updateCmd.Parameters.AddWithValue("@CheckBoxState", (int)checkBoxState);
                updateCmd.Parameters.AddWithValue("@ChangeDate", changeDate);
                updateCmd.Parameters.AddWithValue("@Note", (object)note ?? DBNull.Value);
                updateCmd.Parameters.AddWithValue("@User", (object)userName ?? DBNull.Value);
                updateCmd.Parameters.AddWithValue("@OTKControlID", otkControlID);
                updateCmd.ExecuteNonQuery();
            }
            else
            {
                // Записи нет, выполняем вставку
                string insertOTKControlQuery = "INSERT INTO OTKControl (OperationID, CheckBoxIndex, CheckBoxState, ChangeDate, Note, [User]) VALUES (@OperationID, @CheckBoxIndex, @CheckBoxState, @ChangeDate, @Note, @User)";
                SqlCommand insertCmd = new SqlCommand(insertOTKControlQuery, con, transaction);
                insertCmd.Parameters.AddWithValue("@OperationID", operationID);
                insertCmd.Parameters.AddWithValue("@CheckBoxIndex", checkBoxIndex);
                insertCmd.Parameters.AddWithValue("@CheckBoxState", (int)checkBoxState);
                insertCmd.Parameters.AddWithValue("@ChangeDate", changeDate);
                insertCmd.Parameters.AddWithValue("@Note", (object)note ?? DBNull.Value);
                insertCmd.Parameters.AddWithValue("@User", (object)userName ?? DBNull.Value);
                insertCmd.ExecuteNonQuery();
            }
        }

        public void SaveSingleRow(DataRow dataRow)
        {
            // Получаем PK_IdOrderDetail из текущей строки деталей
            string pk_IdOrderDetail = "";
            if (dGV_Details.CurrentRow != null)
            {
                DataRowView detailRowView = dGV_Details.CurrentRow.DataBoundItem as DataRowView;
                if (detailRowView != null)
                {
                    DataRow detailRow = detailRowView.Row;
                    pk_IdOrderDetail = detailRow["PK_IdOrderDetail"].ToString();
                }
            }

            if (string.IsNullOrEmpty(pk_IdOrderDetail))
            {
                MessageBox.Show("PK_IdOrderDetail отсутствует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OTKControlData currentData = dataRow["OTKControlData"] as OTKControlData;

            string currentUser = Environment.UserName;
            DateTime changeDate = DateTime.Now;

            // Собираем данные операции
            OperationData opData = new OperationData
            {
                PK_IdOrderDetail = Convert.ToInt64(pk_IdOrderDetail),
                Oper = dataRow["Oper"].ToString(),
                Tpd = dataRow["Tpd"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Tpd"]),
                Tsh = dataRow["Tsh"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Tsh"]),
                IdLoodsman = dataRow["IdLoodsman"] == DBNull.Value ? (long?)null : Convert.ToInt64(dataRow["IdLoodsman"]),
                OTKControlData = currentData,
                IsChanged = true,
                IsStateChanged = false, // Предполагаем, что при редактировании заметки состояния не менялись
                ChangeDate = changeDate
            };

            List<OperationData> operationsToSave = new List<OperationData> { opData };

            SaveOperationsToDatabase(operationsToSave, currentData.Note, currentUser, changeDate);

            // Обновляем OriginalOTKControlData
            dataRow["OriginalOTKControlData"] = currentData.Clone();

            // Обновляем DataGridView
            dGV_Tehnology.Refresh();
        }
    }
}
