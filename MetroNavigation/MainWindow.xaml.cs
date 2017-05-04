using MetroNavigation.Controls;
using MetroNavigation.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MetroNavigation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroNavigationMap_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                double? canvasLeft;
                double? canvasBottom;
                foreach (var station in ((MetroNavigationVM)DataContext).Stations)
                {
                    StationControl sc = new StationControl();
                    canvasLeft = (SystemParameters.PrimaryScreenWidth - 100) *
                        (station.OsX - ((MetroNavigationVM)DataContext).MinX) / (((MetroNavigationVM)DataContext).MaxX - ((MetroNavigationVM)DataContext).MinX);

                    canvasBottom = (SystemParameters.PrimaryScreenHeight - 100 - 50) *
                        (station.OsY - ((MetroNavigationVM)DataContext).MinY) / (((MetroNavigationVM)DataContext).MaxY - ((MetroNavigationVM)DataContext).MinY);

                    canvasLeft = canvasLeft > (SystemParameters.PrimaryScreenWidth - 100 - sc.MaxWidth / 2)
                                              ? canvasLeft - (sc.MaxWidth / 2)
                                              : canvasLeft;
                    sc.SetValue(Canvas.LeftProperty, canvasLeft);

                    canvasBottom = canvasBottom > (SystemParameters.PrimaryScreenHeight - 100 - 50 - sc.MaxHeight / 2)
                                              ? canvasBottom - (sc.MaxHeight / 2)
                                              : canvasBottom;
                    sc.SetValue(Canvas.BottomProperty, canvasBottom);

                    sc.DataContext = station;

                    if (station.Line == 1)
                        sc.StationColor = Brushes.Red;
                    if (station.Line == 2)
                        sc.StationColor = Brushes.Green;
                    if (station.Line == 3)
                        sc.StationColor = Brushes.Blue;

                    MetroNavigationMap.MetroNavMapCanvas.Children.Add(sc);
                }
            });

        }
    }
}
