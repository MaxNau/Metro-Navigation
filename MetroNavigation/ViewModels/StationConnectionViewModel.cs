using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace MetroNavigation.ViewModels
{
    public class StationConnectionViewModel : INotifyPropertyChanged
    {
        private double y1;
        private double y2;
        private double x1;
        private double x2;
        private int connectionHeight;
        private double? canvasBottom;
        private double? canvasLeft;
        private int opacity;
        private bool? isSelectedConnection;

        public string NextStation { get; set; }
        public string PreviousStation { get; set; }

        public double X1
        {
            get { return x1; }
            set
            {
                x1 = value;
                OnPropertyChanged("X1");
            }
        }

        public double X2
        {
            get { return x2; }
            set
            {
                x2 = value;
                OnPropertyChanged("X2");
            }
        }

        public int ConnectionHeight
        {
            get { return connectionHeight; }
            set
            {
                connectionHeight = value;
                OnPropertyChanged("ConnectionHeight");
            }
        }

        public bool? IsSelectedConnection
        {
            get { return isSelectedConnection; }
            set
            {
                isSelectedConnection = value;
                OnPropertyChanged("IsSelectedConnection");
            }
        }

        public int Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                OnPropertyChanged("Opacity");
            }
        }

        public StationConnectionViewModel()
        {
            ConnectionHeight = 4;
            ZIndex = -1;
            Opacity = 1;
        }

        public double? CanvasLeft
        {
            get { return canvasLeft; }
            set
            {
                canvasLeft = value;
                OnPropertyChanged("CanvasLeft");
            }
        }

        public double? CanvasBottom
        {
            get { return canvasBottom; }
            set
            {
                canvasBottom = value;
                OnPropertyChanged("CanvasBottom");
            }
        }

        public double Y1
        {
            get { return y1; }
            set
            {
                y1 = value;
                OnPropertyChanged("Y1");
            }
        }

        public double Y2
        {
            get { return y2; }
            set
            {
                y2 = value;
                OnPropertyChanged("Y2");
            }
        }

        public int ZIndex { get; }
        public Brush LineColor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
