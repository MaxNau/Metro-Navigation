using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MetroNavigation.ViewModels
{
    public class StationViewModel : INotifyPropertyChanged
    {
        private double canvasLeft;
        private double canvasBottom;
        private bool isSielected;

        public string Name { get; set; }
        public int Line { get; set; }
        public string ConnectedStation { get; set; }

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
