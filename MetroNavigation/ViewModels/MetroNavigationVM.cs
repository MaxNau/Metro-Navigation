using MetroNavigation.Commands;
using MetroNavigation.Controls;
using MetroNavigation.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System;

namespace MetroNavigation.ViewModels
{
    public class MetroNavigationVM
    {
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }

        public ObservableCollection<StationViewModel> Stations { get; set; }
        public ObservableCollection<StationConnectionViewModel> StationConnections { get; set; }

        public StationViewModel StationPathFrom { get; set; }
        public StationViewModel StationPathTo { get; set; }

        public ICommand SelectStationCommand { get; set; }

        public MetroNavigationVM()
        {
            Load();
        }

        private void Load()
        {
            Stations = new ObservableCollection<StationViewModel>();
            StationConnections = new ObservableCollection<StationConnectionViewModel>();

            SelectStationCommand = new RelayCommand(SelectStation);

            List<Station> stationsData;
            double canvasLeft;
            double canvasBottom;
            StationControl sc = new StationControl();

            using (var context = new StationEntity())
            {
                stationsData = context.Stations.ToList();
            }

            MinX = stationsData.Min(s => s.OsX);
            MaxX = stationsData.Max(s => s.OsX);
            MinY = stationsData.Min(s => s.OsY);
            MaxY = stationsData.Max(s => s.OsY);

            foreach (var station in stationsData)
            {
                canvasLeft = (SystemParameters.WorkArea.Width * 0.9) *
                    (station.OsX - MinX) / (MaxX - MinX);

                canvasBottom = (SystemParameters.WorkArea.Height * 0.9) *
                    (station.OsY - MinY) / (MaxY - MinY);

                canvasLeft = canvasLeft > (SystemParameters.WorkArea.Width * 0.9 )
                                          ? canvasLeft 
                                          : canvasLeft;

                canvasBottom = canvasBottom > (SystemParameters.WorkArea.Height * 0.9) 
                                          ? canvasBottom 
                                          : canvasBottom;

                Stations.Add(new StationViewModel() { CanvasLeft = canvasLeft, CanvasBottom = canvasBottom, Name = station.Name, Line = station.Line, ConnectedStation = station.ConnectedStation });
            }

            var temp = Stations.OrderBy(s => s.Line).ToList();

            foreach (StationViewModel station in temp)
            {
                StationViewModel connected = temp.Find(s => s.Name.Trim() == station.ConnectedStation.Trim());
                if (connected != null)
                {
                    Brush lineColor = Brushes.Black;
                    if (station.Line == 1)
                    {
                        lineColor = Brushes.Red;
                    }
                    else if (station.Line == 2)
                    {
                        lineColor = Brushes.Green;
                    }
                    else if (station.Line == 3)
                    {
                        lineColor = Brushes.Blue;
                    }

                    if (station.CanvasBottom > connected.CanvasBottom & station.CanvasLeft > connected.CanvasLeft | station.CanvasBottom > connected.CanvasBottom & station.CanvasLeft < connected.CanvasLeft)
                        StationConnections.Add(new StationConnectionViewModel() { X1 = station.CanvasLeft + sc.MaxHeight / 2, X2 = connected.CanvasLeft + sc.MaxHeight / 2, Y1 = SystemParameters.WorkArea.Height - station.CanvasBottom , Y2 = SystemParameters.WorkArea.Height - connected.CanvasBottom , CanvasBottom = connected.CanvasBottom + (sc.MaxHeight / 2) - 2, LineColor = lineColor });
                    else if (station.CanvasBottom < connected.CanvasBottom & station.CanvasLeft < connected.CanvasLeft | station.CanvasBottom < connected.CanvasBottom & station.CanvasLeft > connected.CanvasLeft)
                        StationConnections.Add(new StationConnectionViewModel() { X1 = station.CanvasLeft + sc.MaxHeight / 2, X2 = connected.CanvasLeft + sc.MaxHeight / 2, Y1 = SystemParameters.WorkArea.Height - connected.CanvasBottom  + (station.CanvasBottom - connected.CanvasBottom), Y2 = (SystemParameters.WorkArea.Height - connected.CanvasBottom ) + (station.CanvasBottom - connected.CanvasBottom)*2, CanvasBottom = station.CanvasBottom + (sc.MaxHeight / 2) - 2, LineColor = lineColor });
                }
            }
        }

        private void SelectStation(object selectedStation)
        {
            if (StationPathFrom == null)
            {
                StationPathFrom = (StationViewModel)selectedStation;
                StationPathFrom.IsSelected = true;
            }
            else
            {
                if (ReferenceEquals(StationPathFrom, selectedStation))
                {
                    StationPathFrom.IsSelected = false;
                    StationPathFrom = null;
                    if (StationPathTo != null)
                    {
                        StationPathTo.IsSelected = false;
                        StationPathTo = null;
                    }
                }
                else
                {
                    if (ReferenceEquals(StationPathTo, selectedStation))
                    {
                        StationPathTo.IsSelected = false;
                        StationPathTo = null;
                    }
                    else
                    {
                        if (StationPathTo != null)
                        {
                            if (!ReferenceEquals(StationPathTo, selectedStation))
                            {

                            }
                        }
                        else
                        {
                            StationPathTo = (StationViewModel)selectedStation;
                            StationPathTo.IsSelected = true;
                        }
                    }
                }
            }
        }
    }
}
