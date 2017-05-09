using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MetroNavigation.ViewModels
{
    public class StationViewModel : INotifyPropertyChanged
    {
        private double canvasLeft;
        private double canvasBottom;
        private bool isSielected;
        private double opacity;
        private bool? isSelectedStationInThePath;
        private StationConnectionViewModel connectedStation;

        public string Name { get; set; }
        public int Line { get; set; }

        public StationConnectionViewModel ConnectedStationO
        {
            get { return connectedStation; }
            set
            {
                connectedStation = value;
                OnPropertyChanged("ConnectedStationO");
            }
        }

        public StationViewModel()
        {
            Opacity = 1;
        }

        public bool? IsSelectedStationInThePath
        {
            get { return isSelectedStationInThePath; }
            set
            {
                isSelectedStationInThePath = value;
                OnPropertyChanged("IsSelectedStationInThePath");
            }
        }

        public double Opacity
        {
            get { return opacity; }
            set
            {
                opacity = value;
                OnPropertyChanged("Opacity");
            }
        }

        public double CanvasLeft
        {
            get { return canvasLeft; }
            set
            {
                canvasLeft = value;
                OnPropertyChanged("CanvasLeft");
            }
        }

        public double CanvasBottom
        {
            get { return canvasBottom; }
            set
            {
                canvasBottom = value;
                OnPropertyChanged("CanvasBottom");
            }
        }


        public bool IsSelected
        {
            get { return isSielected; }
            set
            {
                isSielected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
