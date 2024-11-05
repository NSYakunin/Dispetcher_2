using System.Drawing;
using System.Windows.Forms;
using System;

public class DataGridViewOTKControlCell : DataGridViewCell
{
    // Размеры и отступы для галочек
    private static int CheckBoxWidth = 25;
    private static int CheckBoxHeight = 25;
    private static int CheckBoxSpacing = 10;

    public DataGridViewOTKControlCell()
    {
        this.ValueType = typeof(bool[]);
    }

    protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
        DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
        DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
    {
        // Получаем текущее значение ячейки
        bool[] state = value as bool[] ?? new bool[] { false, false, false };

        // Определяем цвет фона ячейки
        Color cellBackColor = state[2] ? Color.LightGreen : cellStyle.BackColor;

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
            bool isChecked = state.Length > i && state[i];
            bool isEnabled = !(state[2] && i < 2); // Блокируем первые два, если третий отмечен

            ButtonState buttonState = ButtonState.Normal;

            if (!isEnabled)
            {
                buttonState |= ButtonState.Inactive;
            }
            if (isChecked)
            {
                buttonState |= ButtonState.Checked;
            }

            // Рисуем чекбокс
            ControlPaint.DrawCheckBox(graphics, cbRect, buttonState);

            // Рисуем номер только если чекбокс не отмечен и активен
            if (!isChecked && isEnabled)
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

    protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
    {
        base.OnMouseClick(e);

        // Получаем координаты клика относительно ячейки
        Point clickLocation = e.Location;

        // Получаем текущее значение ячейки
        bool[] state = this.Value as bool[];
        if (state == null || state.Length != 3)
        {
            state = new bool[] { false, false, false };
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

            if (cbRect.Contains(clickLocation))
            {
                if (state[2] && i < 2)
                {
                    // Если третий чекбокс отмечен, блокируем первые два
                    break;
                }

                if (i == 2)
                {
                    // Переключаем третий чекбокс
                    state[2] = !state[2];

                    if (state[2])
                    {
                        // Сбрасываем первые два чекбокса, если третий отмечен
                        state[0] = false;
                        state[1] = false;
                    }
                }
                else
                {
                    // Переключаем первый или второй чекбокс
                    state[i] = !state[i];
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

    public override object DefaultNewRowValue
    {
        get
        {
            return new bool[] { false, false, false };
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