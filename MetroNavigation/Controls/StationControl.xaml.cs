using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MetroNavigation.Controls
{
    /// <summary>
    /// Interaction logic for StationControl.xaml
    /// </summary>
    public partial class StationControl : UserControl
    {
        public StationControl()
        {
            InitializeComponent();
        }

        public Brush StationColor
        {
            get { return (Brush)GetValue(StationColorProperty); }

            set { SetValue(StationColorProperty, value); }
        }

        public static readonly DependencyProperty StationColorProperty = DependencyProperty.Register("StationColor", typeof(Brush), typeof(StationControl), new FrameworkPropertyMetadata(Brushes.Black));
    }
}
