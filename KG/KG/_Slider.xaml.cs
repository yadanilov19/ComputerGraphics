using System.Windows;
using System.Windows.Controls;

namespace KG
{
    /// <summary>
    /// Interaction logic for _Slider.xaml
    /// </summary>
    public partial class _Slider : UserControl
    {
        public event RoutedEventHandler valueChanged;

        public _Slider()
        {
            InitializeComponent();
        }

        public double Maximum
        {
            get
            {
                return slider.Maximum;
            }
            set
            {
                slider.Maximum = value;
            }
        }

        public double Minimum
        {
            get
            {
                return slider.Minimum;
            }
            set
            {
                slider.Minimum = value;
            }
        }

        public string Label
        {
            get
            {
                return _NameProperty.Text;
            }
            set
            {
                _NameProperty.Text = value;
            }
        }

        public double CurrentValue
        {
            get
            {
                return slider.Value;
            }
            set
            {
                slider.Value = value;
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (valueChanged != null)
            {
                valueChanged(sender, e);
            }
        }
    }
}
