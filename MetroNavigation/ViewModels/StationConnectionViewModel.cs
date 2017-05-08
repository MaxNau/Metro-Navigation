﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace MetroNavigation.ViewModels
{
    public class StationConnectionViewModel : INotifyPropertyChanged
    {
        private double y1;
        private double y2;
        private double? canvasBottom;
        private double? canvasLeft;

        public double X1 { get; set; }
        public double X2 { get; set; }
        public int ConnectionHeight { get; set; }

        public StationConnectionViewModel()
        {
            ConnectionHeight = 4;
            ZIndex = -1;
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
