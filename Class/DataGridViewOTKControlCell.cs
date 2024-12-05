using System.Drawing;
using System.Windows.Forms;
using System;
using System.Data;
using Dispetcher2.DialogsForms;
using System.IO;

namespace Dispetcher2.Class
{
    public enum CheckBoxState
    {
        Unchecked,
        Checked,
        CrossedBrown,
        CrossedRed
    }

    public class DataGridViewOTKControlCell : DataGridViewCell
    {
        // Размеры и отступы для галочек
        private static int CheckBoxWidth = 25;
        private static int CheckBoxHeight = 25;
        private static int CheckBoxSpacing = 10;

        // Поля для контекстного меню
        private ContextMenuStrip contextMenu;
        private int clickedCheckBoxIndex = -1; // Индекс чекбокса, на который кликнули

        public DataGridViewOTKControlCell()
        {
            this.ValueType = typeof(OTKControlData);

            // Инициализируем контекстное меню
            contextMenu = new ContextMenuStrip();
            contextMenu.ShowImageMargin = false; // Убираем отступ слева

            // Добавляем пункты меню
            contextMenu.Items.Add("Доработка").Click += (s, e) => { SetCheckBoxState(clickedCheckBoxIndex, CheckBoxState.CrossedBrown); };
            contextMenu.Items.Add("Брак").Click += (s, e) => { SetCheckBoxState(clickedCheckBoxIndex, CheckBoxState.CrossedRed); };
            contextMenu.Items.Add("С разрешения конструктора").Click += (s, e) => { /* Реализовать при необходимости */ };
            contextMenu.Items.Add("Прикрепить файл").Click += (s, e) => { AttachFile(); };
            contextMenu.Items.Add("Просмотреть файлы").Click += (s, e) => { ViewFiles(); };
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("Редактировать заметку").Click += (s, e) => { EditNote(); };
            contextMenu.Items.Add("Сбросить").Click += (s, e) => { ResetCellState(); };
            contextMenu.Items.Add("Сохранить").Click += (s, e) => { /* Заглушка для сохранения */ };
        }

        private void ViewFiles()
        {
           
            // Получаем данные аналогично методу AttachFile
            DataGridViewRow dgRow = this.OwningRow;
            DataRow dataRow = ((DataRowView)dgRow.DataBoundItem).Row;

            string Oper = dataRow["Oper"].ToString();
            long? IdLoodsman = dataRow["IdLoodsman"] != DBNull.Value ? (long?)Convert.ToInt64(dataRow["IdLoodsman"]) : null;

            DataGridView dataGridView = this.DataGridView;
            F_Fact form = dataGridView.FindForm() as F_Fact;
            if (form != null)
            {
                long PK_IdOrderDetail = form.GetCurrentPK_IdOrderDetail();
                form.HandleViewFiles(PK_IdOrderDetail, Oper, IdLoodsman);
            }
        }
        private void AttachFile()
        {
            // Получаем данные из текущей строки
            DataGridViewRow dgRow = this.OwningRow;
            DataRow dataRow = ((DataRowView)dgRow.DataBoundItem).Row;

            string Oper = dataRow["Oper"].ToString();

            DataGridView dataGridView = this.DataGridView;
            F_Fact form = dataGridView.FindForm() as F_Fact;
            if (form != null)
            {
                long PK_IdOrderDetail = form.GetCurrentPK_IdOrderDetail();

                // Получаем OperationID
                int OperationID = form.GetOperationID(PK_IdOrderDetail, Oper);

                if (OperationID == 0)
                {
                    MessageBox.Show("OperationID не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string targetDirectory = $@"\\Ascon\Dispetcher\DispetcherDock\OperationID_{OperationID}";
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                // Открываем форму FilesForm с параметром autoOpenAddDialog = true
                FilesForm filesForm = new FilesForm(targetDirectory, OperationID, autoOpenAddDialog: true, autoCloseAfterAdd: true);
                filesForm.ShowDialog();
            }
        }

        protected override void Paint(Graphics graphics,
                                      Rectangle clipBounds,
                                      Rectangle cellBounds,
                                      int rowIndex, 
                                      DataGridViewElementStates cellState,
                                      object value,
                                      object formattedValue,
                                      string errorText, 
                                      DataGridViewCellStyle cellStyle,
                                      DataGridViewAdvancedBorderStyle advancedBorderStyle,
                                      DataGridViewPaintParts paintParts)
        {
            if (this.ReadOnly)
            {
                cellStyle.BackColor = Color.LightGray;
                cellStyle.ForeColor = Color.DarkGray;
            }
            OTKControlData otkData = this.Value as OTKControlData;
            if (otkData == null)
            {
                otkData = new OTKControlData();
            }

            CheckBoxState[] state = otkData.States;
            if (state == null || state.Length != 3)
            {
                state = new CheckBoxState[] { CheckBoxState.Unchecked, CheckBoxState.Unchecked, CheckBoxState.Unchecked };
            }

            // Проверяем состояния
            bool isThirdChecked = (state[2] == CheckBoxState.Checked);
            bool hasCrosses = (state[0] == CheckBoxState.CrossedRed || state[1] == CheckBoxState.CrossedRed || state[2] == CheckBoxState.CrossedRed);

            // Определяем, должны ли чекбоксы быть неактивными
            bool isCellInactive = isThirdChecked || hasCrosses;

            // Определяем цвет фона ячейки
            Color cellBackColor = cellStyle.BackColor;
            if (hasCrosses)
            {
                cellBackColor = Color.DarkRed;
            }
            else if (isThirdChecked)
            {
                cellBackColor = Color.LightGreen;
            }

            // Рисуем фон ячейки
            using (SolidBrush cellBackground = new SolidBrush(cellBackColor))
            {
                graphics.FillRectangle(cellBackground, cellBounds);
            }

            // Рисуем границы ячейки
            PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);

            // Вычисляем позиции для каждой галочки
            int totalWidth = 3 * CheckBoxWidth + 2 * CheckBoxSpacing;
            int startX = cellBounds.X + (cellBounds.Width - totalWidth) / 2;
            int startY = cellBounds.Y + (cellBounds.Height - CheckBoxHeight) / 2;

            for (int i = 0; i < 3; i++)
            {
                // Позиция и размер чекбокса
                Rectangle cbRect = new Rectangle(startX + i * (CheckBoxWidth + CheckBoxSpacing),
                                                 startY, CheckBoxWidth, CheckBoxHeight);

                // Определяем состояние чекбокса
                CheckBoxState cbState = state.Length > i ? state[i] : CheckBoxState.Unchecked;

                // Определяем, должен ли чекбокс быть неактивным
                bool isEnabled = !isCellInactive;

                ButtonState buttonState = ButtonState.Normal;

                if (!isEnabled)
                {
                    buttonState |= ButtonState.Inactive;
                }

                // Рисуем рамку чекбокса
                ControlPaint.DrawCheckBox(graphics, cbRect, buttonState);

                if (cbState == CheckBoxState.Checked)
                {
                    // Рисуем галочку
                    ControlPaint.DrawCheckBox(graphics, cbRect, buttonState | ButtonState.Checked);
                }
                else if (cbState == CheckBoxState.CrossedBrown || cbState == CheckBoxState.CrossedRed)
                {
                    // Определяем цвет крестика
                    Color crossColor = cbState == CheckBoxState.CrossedBrown ? Color.DarkKhaki : Color.Red;

                    // Рисуем крестик
                    using (Pen pen = new Pen(crossColor, 4))
                    {
                        graphics.DrawLine(pen, cbRect.X + 4, cbRect.Y + 4, cbRect.Right - 4, cbRect.Bottom - 4);
                        graphics.DrawLine(pen, cbRect.Right - 4, cbRect.Y + 4, cbRect.X + 4, cbRect.Bottom - 4);
                    }
                }
                else
                {
                    // Рисуем номер только если чекбокс не отмечен и активен
                    if (isEnabled)
                    {
                        string number = (i + 1).ToString();
                        SizeF numberSize = graphics.MeasureString(number, cellStyle.Font);

                        // Вычисляем позицию для номера
                        PointF numberLocation = new PointF(
                            cbRect.X + (cbRect.Width - numberSize.Width) / 2,
                            cbRect.Y + (cbRect.Height - numberSize.Height) / 2
                        );

                        // Рисуем номер
                        graphics.DrawString(number, cellStyle.Font, Brushes.Black, numberLocation);
                    }
                }
            }
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (this.ReadOnly)
            {
                return;
            }
            base.OnMouseClick(e);


            if (e.Button == MouseButtons.Left)
            {
                // Получаем индекс чекбокса по позиции клика
                int checkboxIndex = GetCheckboxIndexAtPoint(e.Location);
                // Получаем текущее значение ячейки
                OTKControlData otkData = this.Value as OTKControlData;
                if (otkData == null)
                {
                    otkData = new OTKControlData();
                }

                CheckBoxState[] state = otkData.States;
                if (state == null || state.Length != 3)
                {
                    state = new CheckBoxState[] { CheckBoxState.Unchecked, CheckBoxState.Unchecked, CheckBoxState.Unchecked };
                }

                // Проверяем состояния
                bool isThirdChecked = (state[2] == CheckBoxState.Checked);
                bool hasCrosses = (state[0] == CheckBoxState.CrossedRed || state[1] == CheckBoxState.CrossedRed || state[2] == CheckBoxState.CrossedRed);
                bool isCellInactive = isThirdChecked || hasCrosses;



                if (isCellInactive)
                {
                    // Блокируем изменение состояний
                    return;
                }

                // Вычисляем позиции для каждой галочки
                int totalWidth = 3 * CheckBoxWidth + 2 * CheckBoxSpacing;
                int startX = (this.Size.Width - totalWidth) / 2;
                int startY = (this.Size.Height - CheckBoxHeight) / 2;

                bool stateChanged = false;

                for (int i = 0; i < 3; i++)
                {
                    Rectangle cbRect = new Rectangle(startX + i * (CheckBoxWidth + CheckBoxSpacing),
                                                     startY, CheckBoxWidth, CheckBoxHeight);

                    if (cbRect.Contains(e.Location))
                    {
                        if (i == 2)
                        {
                            // Переключаем третий чекбокс
                            if (state[2] == CheckBoxState.Unchecked)
                            {
                                state[2] = CheckBoxState.Checked;
                            }
                            else
                            {
                                state[2] = CheckBoxState.Unchecked;
                            }
                        }
                        else
                        {
                            // Переключаем первый или второй чекбокс
                            if (state[i] == CheckBoxState.Unchecked)
                            {
                                state[i] = CheckBoxState.Checked;
                            }
                            else
                            {
                                state[i] = CheckBoxState.Unchecked;
                            }
                        }

                        stateChanged = true;

                        // Обновляем состояния в объекте OTKControlData
                        otkData.States = state;

                        // Обновляем значение ячейки
                        this.Value = otkData;

                        // Уведомляем DataGridView об изменении
                        this.DataGridView.InvalidateCell(this);
                        this.DataGridView.NotifyCurrentCellDirty(true);
                        this.DataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        break;
                    }
                }

                if (stateChanged)
                {
                    // Перерисовываем ячейку, чтобы обновить отображение
                    this.DataGridView.InvalidateCell(this);
                }
            }
        }

        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (this.ReadOnly)
            {
                return;
            }
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Right)
            {
                // Получаем координаты клика относительно ячейки
                Point clickLocation = e.Location;

                // Вычисляем позиции для каждой галочки
                int totalWidth = 3 * CheckBoxWidth + 2 * CheckBoxSpacing;
                int startX = (this.Size.Width - totalWidth) / 2;
                int startY = (this.Size.Height - CheckBoxHeight) / 2;

                for (int i = 0; i < 3; i++)
                {
                    Rectangle cbRect = new Rectangle(startX + i * (CheckBoxWidth + CheckBoxSpacing),
                                                     startY, CheckBoxWidth, CheckBoxHeight);

                    if (cbRect.Contains(clickLocation))
                    {
                        clickedCheckBoxIndex = i;
                        // Отображаем контекстное меню
                        contextMenu.Show(Cursor.Position);
                        break;
                    }
                }
            }
        }

        private void SetCheckBoxState(int index, CheckBoxState newState)
        {
            // Получаем текущее значение ячейки
            OTKControlData otkData = this.Value as OTKControlData;
            if (otkData == null)
            {
                otkData = new OTKControlData();
            }

            CheckBoxState[] state = otkData.States;
            if (state == null || state.Length != 3)
            {
                state = new CheckBoxState[] { CheckBoxState.Unchecked, CheckBoxState.Unchecked, CheckBoxState.Unchecked };
            }

            state[index] = newState;
            otkData.States = state;

            // Обновляем ячейку
            this.Value = otkData;
            this.DataGridView.InvalidateCell(this);
            this.DataGridView.NotifyCurrentCellDirty(true);
        }

        private void ResetCellState()
        {
            // Сбрасываем состояние ячейки в исходное
            OTKControlData otkData = this.Value as OTKControlData;
            if (otkData == null)
            {
                otkData = new OTKControlData();
            }

            otkData.States = new CheckBoxState[] { CheckBoxState.Unchecked, CheckBoxState.Unchecked, CheckBoxState.Unchecked };

            this.Value = otkData;
            this.DataGridView.InvalidateCell(this);
            this.DataGridView.NotifyCurrentCellDirty(true);

            // Уведомляем форму о сбросе ячейки
            DataGridView dataGridView = this.DataGridView;
            F_Fact form = dataGridView.FindForm() as F_Fact;
            if (form != null)
            {
                // Передаем строку, в которой произошел сброс
                form.CellReset(this.OwningRow);
            }
        }

        private void EditNote()
        {
            // Получаем объект OTKControlData из значения ячейки
            OTKControlData otkData = this.Value as OTKControlData;
            if (otkData == null)
            {
                otkData = new OTKControlData();
            }

            // Открываем форму для редактирования заметки
            NoteForm noteForm = new NoteForm();
            noteForm.NoteText = otkData.Note; // Передаем текущую заметку в форму

            if (noteForm.ShowDialog() == DialogResult.OK)
            {
                // Обновляем заметку после изменений
                otkData.Note = noteForm.NoteText;

                if (string.IsNullOrEmpty(otkData.Note))
                {
                    // Если заметка была удалена, устанавливаем дату и пользователя в значения по умолчанию
                    otkData.ChangeDate = DateTime.MinValue;
                    otkData.User = string.Empty;
                }
                else
                {
                    // Если заметка обновлена, устанавливаем текущую дату и пользователя
                    otkData.ChangeDate = DateTime.Now;
                    otkData.User = Environment.UserName;
                }

                // Обновляем значение ячейки
                this.Value = otkData;

                // Уведомляем DataGridView об изменении
                this.DataGridView.NotifyCurrentCellDirty(true);

                // Сохраняем изменения сразу
                DataGridView dataGridView = this.DataGridView;
                F_Fact form = dataGridView.FindForm() as F_Fact;
                if (form != null)
                {
                    // Используем OwningRow для получения строки, в которой произошло редактирование
                    DataGridViewRow row = this.OwningRow;

                    // Вызываем метод сохранения, передавая корректную строку
                    form.SaveOTKControlData(row);
                }

                // Обновляем отображение ячейки
                this.DataGridView.InvalidateCell(this);
            }
        }

        private int GetCheckboxIndexAtPoint(Point point)
        {
            OTKControlData otkData = this.Value as OTKControlData;
            if (otkData == null || otkData.States == null)
                return -1;

            int numberOfCheckboxes = otkData.States.Length;
            int checkboxWidth = 14;
            int checkboxHeight = 14;
            int totalCheckboxesWidth = numberOfCheckboxes * checkboxWidth + (numberOfCheckboxes - 1) * 4;
            int startX = this.ContentBounds.X + (this.ContentBounds.Width - totalCheckboxesWidth) / 2;
            int startY = this.ContentBounds.Y + (this.ContentBounds.Height - checkboxHeight) / 2;

            // Корректируем координаты относительно начала ячейки
            Point cellRelativePoint = new Point(point.X - this.ContentBounds.X, point.Y - this.ContentBounds.Y);

            for (int i = 0; i < numberOfCheckboxes; i++)
            {
                Rectangle checkboxRect = new Rectangle(startX + i * (checkboxWidth + 4) - this.ContentBounds.X, startY - this.ContentBounds.Y, checkboxWidth, checkboxHeight);
                if (checkboxRect.Contains(cellRelativePoint))
                {
                    return i;
                }
            }

            return -1;
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return new OTKControlData();
            }
        }

        public override object Clone()
        {
            DataGridViewOTKControlCell cell = (DataGridViewOTKControlCell)base.Clone();
            return cell;
        }

        public override Type EditType
        {
            get
            {
                return null;
            }
        }

        public override Type ValueType
        {
            get { return typeof(OTKControlData); }
            set { base.ValueType = value; }
        }

        public override Type FormattedValueType
        {
            get
            {
                return typeof(string);
            }
        }
    }
}