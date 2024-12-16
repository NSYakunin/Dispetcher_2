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
    public partial class TooltipForm : Form
    {
        public TooltipForm()
        {
            InitializeComponent();
            InitializeContent();
        }

        private void InitializeContent()
        {
            // Настройка формы
            this.BackColor = Color.LightYellow;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // Создание FlowLayoutPanel
            FlowLayoutPanel flp = new FlowLayoutPanel();
            flp.FlowDirection = FlowDirection.LeftToRight;
            flp.WrapContents = true;
            flp.AutoSize = true;
            flp.Padding = new Padding(10);
            this.Controls.Add(flp);

            // Добавление инструкций
            AddInstruction(flp, "красный крест", Color.Red, "брак");
            AddInstruction(flp, "синий крест", Color.Blue, "не полное количество");
            AddInstruction(flp, "коричневый крест", Color.Brown, "доработка");
            AddInstruction(flp, "галочка", Color.Green, "принято");

            // Добавление TextBox
            TextBox tb = new TextBox();
            tb.Multiline = true;
            tb.Width = 300;
            tb.Height = 100;
            tb.Margin = new Padding(10);
            tb.Text = "Здесь вы можете отредактировать текст...";
            flp.SetFlowBreak(tb, true);
            flp.Controls.Add(tb);
        }

        private void AddInstruction(FlowLayoutPanel flp, string description, Color color, string tooltip)
        {
            Panel panel = new Panel();
            panel.AutoSize = true;
            panel.Margin = new Padding(5);

            PictureBox pb = new PictureBox();
            pb.Size = new Size(16, 16);
            pb.BackColor = color;
            pb.Paint += (s, e) =>
            {
                // Рисуем крест или галочку
                if (description.Contains("крест"))
                {
                    Pen pen = new Pen(Color.White, 2);
                    e.Graphics.DrawLine(pen, 0, 0, pb.Width, pb.Height);
                    e.Graphics.DrawLine(pen, pb.Width, 0, 0, pb.Height);
                }
                else if (description.Contains("галочка"))
                {
                    Pen pen = new Pen(Color.White, 2);
                    e.Graphics.DrawLines(pen, new Point[] {
                    new Point(2, pb.Height / 2),
                    new Point(pb.Width / 2, pb.Height - 2),
                    new Point(pb.Width - 2, 2)
                });
                }
            };
            pb.BackColor = color;
            pb.Size = new Size(16, 16);
            pb.Paint += (s, e) =>
            {
                if (description.Contains("крест"))
                {
                    Pen pen = new Pen(Color.White, 2);
                    e.Graphics.DrawLine(pen, 0, 0, pb.Width, pb.Height);
                    e.Graphics.DrawLine(pen, pb.Width, 0, 0, pb.Height);
                }
                else if (description.Contains("галочка"))
                {
                    Pen pen = new Pen(Color.White, 2);
                    e.Graphics.DrawLines(pen, new Point[] {
                    new Point(2, pb.Height / 2),
                    new Point(pb.Width / 2, pb.Height - 2),
                    new Point(pb.Width - 2, 2)
                });
                }
            };
            panel.Controls.Add(pb);

            Label lbl = new Label();
            lbl.Text = $" - {tooltip}";
            lbl.AutoSize = true;
            lbl.Location = new Point(pb.Right + 5, 0);
            panel.Controls.Add(lbl);

            flp.Controls.Add(panel);
        }
    }
}
