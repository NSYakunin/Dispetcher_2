using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace Dispetcher2
{
    /// <summary>
    /// Логика взаимодействия для WaitControl.xaml
    /// </summary>
    public partial class WaitControl : UserControl, INotifyPropertyChanged
    {
        string messageValue;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Message
        {
            get { return messageValue; }
            set
            {
                messageValue = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }

        public WaitControl()
        {
            InitializeComponent();
            this.DataContext = this;
            Stop();
        }
        public void Start()
        {
            mainGrid.Visibility = Visibility.Visible;
            var a = new DoubleAnimation();
            a.From = 0;
            a.To = 359;
            a.Duration = TimeSpan.FromSeconds(3);
            a.RepeatBehavior = RepeatBehavior.Forever;
            waitTransform.BeginAnimation(RotateTransform.AngleProperty, a);
        }

        public void Stop()
        {
            waitTransform.BeginAnimation(RotateTransform.AngleProperty, null);
            mainGrid.Visibility = Visibility.Hidden;

        }
    }
}
