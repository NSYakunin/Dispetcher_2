using System.Drawing;
using System.Windows.Forms;
using System;

namespace Dispetcher2.Class
{
    public enum CheckBoxState
    {
        Unchecked,
        Checked,
        Crossed
    }
    public class DataGridViewOTKControlCell : DataGridViewCell
    {


        // Размеры и отступы для галочек
        private static int CheckBoxWidth = 25;
        private static int CheckBoxHeight = 25;
        private static int CheckBoxSpacing = 10;

        public DataGridViewOTKControlCell()
        {
            this.ValueType = typeof(CheckBoxState[]);
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
                        DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
                        DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            // Получаем текущее значение ячейки
            CheckBoxState[] state = value as CheckBoxState[] ?? new CheckBoxState[] { CheckBoxState.Unchecked, CheckBoxState.Unchecked, CheckBoxState.Unchecked };

            bool isThirdCheckedOrCrossed = (state[2] == CheckBoxState.Checked || state[2] == CheckBoxState.Crossed);

            // Определяем цвет фона ячейки
            Color cellBackColor = isThirdCheckedOrCrossed ? Color.LightGreen : cellStyle.BackColor;

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
                bool isEnabled = !(isThirdCheckedOrCrossed && i < 2); // Блокируем первые два, если третий отмечен или с крестиком

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
                else if (cbState == CheckBoxState.Crossed)
                {
                    // Рисуем крестик
                    using (Pen pen = new Pen(Color.Black, 2))
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

        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseUp(e);

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
                        // Создаем контекстное меню
                        ContextMenuStrip contextMenu = new ContextMenuStrip();

                        // Добавляем пункты меню
                        ToolStripMenuItem item1 = new ToolStripMenuItem("Доработка");
                        ToolStripMenuItem item2 = new ToolStripMenuItem("Брак");
                        ToolStripMenuItem item3 = new ToolStripMenuItem("С разрешения конструктора");
                        ToolStripMenuItem item4 = new ToolStripMenuItem("Прикрепить файл");

                        int index = i; // Для использования в лямбда-выражении

                        // Добавляем обработчики событий
                        item1.Click += (sender, args) => { SetCheckBoxState(index, CheckBoxState.Crossed); };
                        item2.Click += (sender, args) => { SetCheckBoxState(index, CheckBoxState.Crossed); };
                        item3.Click += (sender, args) => { /* Реализовать при необходимости */ };
                        item4.Click += (sender, args) => { /* Реализовать позже */ };

                        // Добавляем пункты в меню
                        contextMenu.Items.AddRange(new ToolStripItem[] { item1, item2, item3, item4 });

                        // Отображаем меню
                        contextMenu.Show(Cursor.Position);
                        break;
                    }
                }
            }
        }

        private void SetCheckBoxState(int index, CheckBoxState newState)
        {
            // Получаем текущее значение ячейки
            CheckBoxState[] state = this.Value as CheckBoxState[];
            if (state == null || state.Length != 3)
            {
                state = new CheckBoxState[] { CheckBoxState.Unchecked, CheckBoxState.Unchecked, CheckBoxState.Unchecked };
            }

            if (index == 2)
            {
                // Если устанавливаем третий чекбокс, сбрасываем первые два
                state[2] = newState;
                state[0] = CheckBoxState.Unchecked;
                state[1] = CheckBoxState.Unchecked;
            }
            else
            {
                state[index] = newState;
            }

            this.Value = state;
            this.DataGridView.InvalidateCell(this);
            this.DataGridView.NotifyCurrentCellDirty(true);
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Left)
            {
                // Получаем координаты клика относительно ячейки
                Point clickLocation = e.Location;

                // Получаем текущее значение ячейки
                CheckBoxState[] state = this.Value as CheckBoxState[];
                if (state == null || state.Length != 3)
                {
                    state = new CheckBoxState[] { CheckBoxState.Unchecked, CheckBoxState.Unchecked, CheckBoxState.Unchecked };
                }

                // Вычисляем позиции для каждой галочки
                int totalWidth = 3 * CheckBoxWidth + 2 * CheckBoxSpacing;
                int startX = (this.Size.Width - totalWidth) / 2;
                int startY = (this.Size.Height - CheckBoxHeight) / 2;

                bool stateChanged = false;

                bool isThirdCheckedOrCrossed = (state[2] == CheckBoxState.Checked || state[2] == CheckBoxState.Crossed);

                for (int i = 0; i < 3; i++)
                {
                    Rectangle cbRect = new Rectangle(startX + i * (CheckBoxWidth + CheckBoxSpacing),
                                                     startY, CheckBoxWidth, CheckBoxHeight);

                    if (cbRect.Contains(clickLocation))
                    {
                        if (isThirdCheckedOrCrossed && i < 2)
                        {
                            // Если третий чекбокс отмечен или с крестиком, блокируем первые два
                            break;
                        }

                        if (i == 2)
                        {
                            // Переключаем третий чекбокс
                            if (state[2] == CheckBoxState.Unchecked)
                            {
                                state[2] = CheckBoxState.Checked;
                                // Сбрасываем первые два чекбокса, если третий отмечен
                                state[0] = CheckBoxState.Unchecked;
                                state[1] = CheckBoxState.Unchecked;
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
                        this.Value = state;
                        this.DataGridView.InvalidateCell(this);
                        this.DataGridView.NotifyCurrentCellDirty(true);
                        break;
                    }
                }

                if (stateChanged)
                {
                    // Перерисовываем ячейку, чтобы обновить фон
                    this.DataGridView.InvalidateCell(this);
                }
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return new CheckBoxState[] { CheckBoxState.Unchecked, CheckBoxState.Unchecked, CheckBoxState.Unchecked };
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
            get
            {
                return typeof(bool[]);
            }
            set
            {
                base.ValueType = value;
            }
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