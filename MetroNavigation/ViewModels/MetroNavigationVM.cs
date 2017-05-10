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
using System.Data.Entity;

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
        public bool FindPath { get; private set; }

        public MetroNavigationVM()
        {
            Load();
        }

        // this method will be reworked
        private void Load()
        {
            Stations = new ObservableCollection<StationViewModel>();
            StationConnections = new ObservableCollection<StationConnectionViewModel>();

            SelectStationCommand = new RelayCommand(SelectStation);

            List<Station> stationsData;
            double canvasLeft;
            double canvasBottom;
            StationControl sc = new StationControl();

            using (var context = new MetroNavigationContext())
            {
                stationsData = context.Stations.
                    Include(s => s.StationConnection).ToList();
            }

            MinX = stationsData.Min(s => s.OsX);
            MaxX = stationsData.Max(s => s.OsX);
            MinY = stationsData.Min(s => s.OsY);
            MaxY = stationsData.Max(s => s.OsY);
            List<double> l = new List<double>();
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

                Stations.Add(new StationViewModel() { CanvasLeft = canvasLeft, CanvasBottom = canvasBottom, Name = station.Name, Line = station.Line, ConnectedStationO = new StationConnectionViewModel() { NextStation = station.StationConnection.NextStation, PreviousStation = station.StationConnection.PreviousStation }, TransitToLine = station.TransitToLine, TransitToStation = station.TransitToStation });    
            }

            var temp = Stations.OrderBy(s => s.Line).ToList();

            foreach (StationViewModel station in temp)
            {
                StationViewModel connected = null;
                if (station.ConnectedStationO.NextStation != null)
                {
                    connected = temp.Find(s => s.Name.Trim() == station.ConnectedStationO.NextStation.Trim());
                }

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
                        StationConnections.Add(new StationConnectionViewModel() { X1 = station.CanvasLeft + sc.MaxHeight / 2, X2 = connected.CanvasLeft + sc.MaxHeight / 2, Y1 = SystemParameters.WorkArea.Height - station.CanvasBottom , Y2 = SystemParameters.WorkArea.Height - connected.CanvasBottom , CanvasBottom = connected.CanvasBottom + (sc.MaxHeight / 2) - 2, LineColor = lineColor, NextStation = connected.Name });
                    else if (station.CanvasBottom < connected.CanvasBottom & station.CanvasLeft < connected.CanvasLeft | station.CanvasBottom < connected.CanvasBottom & station.CanvasLeft > connected.CanvasLeft)
                        StationConnections.Add(new StationConnectionViewModel() { X1 = station.CanvasLeft + sc.MaxHeight / 2, X2 = connected.CanvasLeft + sc.MaxHeight / 2, Y1 = SystemParameters.WorkArea.Height - connected.CanvasBottom  + (station.CanvasBottom - connected.CanvasBottom), Y2 = (SystemParameters.WorkArea.Height - connected.CanvasBottom ) + (station.CanvasBottom - connected.CanvasBottom)*2, CanvasBottom = station.CanvasBottom + (sc.MaxHeight / 2) - 2, LineColor = lineColor, NextStation = connected.Name });
                }
            }
        }


        // this method will be reworked
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
                    RemoveStationFromSelection();
                    ResetSelectedStationsAndStationConnections();

                    if (StationPathTo != null)
                    {
                        RemoveStationToSelection();
                    }
                }
                else
                {
                    if (ReferenceEquals(StationPathTo, selectedStation))
                    {
                        RemoveStationToSelection();
                        ResetSelectedStationsAndStationConnections();
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
            if (StationPathFrom != null & StationPathTo != null)
                FindPath = true;
            else
                FindPath = false;
            if (FindPath == true)
            {
                FindPathZ(StationPathFrom, StationPathTo);
            }
        }

        public void FindPathZ(StationViewModel startStation, StationViewModel endStation)
        {
            HideStationsAndConnectionsThatIsNotOnTheSelectedPath();
            StationPathFrom.IsSelectedStationInThePath = true;
            StationPathTo.IsSelectedStationInThePath = true;

            if (startStation.ConnectedStationO.PreviousStation == null & startStation.Line == StationPathTo.Line)
            {
                MoveToNext(startStation, endStation);
            }
            else if (startStation.ConnectedStationO.NextStation == null & startStation.Line == StationPathTo.Line)
            {
                MoveToPrevious(startStation, endStation);
            }
            else if (startStation.Line == StationPathTo.Line && check(startStation, endStation))
            {
                MoveToNext(startStation, endStation);
            }
            else if (startStation.Line == StationPathTo.Line && !check(startStation, endStation))
            {
                MoveToPrevious(startStation, endStation);
            }
            else
            {
                var transit = Stations.FirstOrDefault(s => s.TransitToLine == StationPathTo.Line & StationPathFrom.Line == s.Line);
                if(check(startStation, transit))
                {
                    MoveToNext(startStation, transit);
                    startStation = Stations.ToList().Find(s => s.Name.Trim() == transit.TransitToStation.Trim());
                    if (check(startStation, endStation))
                        MoveToNext(startStation, endStation);
                    else
                        MoveToPrevious(startStation, endStation);
                }
                else
                {
                    MoveToPrevious(startStation, transit);
                    startStation = Stations.ToList().Find(s => s.Name.Trim() == transit.TransitToStation.Trim());
                    if (!check(startStation, endStation))
                        MoveToPrevious(startStation, endStation);
                    else
                        MoveToNext(startStation, endStation);
                }
            }

        }

        public void MoveToNext(StationViewModel startStation, StationViewModel endStation)
        {
            while (startStation.ConnectedStationO.NextStation != endStation.Name)
            {
                startStation = Stations.ToList().Find(s => s.Name.Trim() == startStation.ConnectedStationO.NextStation.Trim());
                StationConnections.SingleOrDefault(s =>
                {
                    if (s.NextStation != null & s.NextStation.Trim() == startStation.ConnectedStationO.NextStation.Trim())
                    {
                        s.IsSelectedConnection = true;
                        return true;
                    }
                    return false;
                });
                startStation.IsSelectedStationInThePath = true;
            }
        }

        public bool check(StationViewModel station, StationViewModel endStation)
        {
            if (station.ConnectedStationO.NextStation != null && station.ConnectedStationO.NextStation != endStation.Name)
            {
                return check(Stations.ToList().Find(s => s.Name.Trim() == station.ConnectedStationO.NextStation.Trim()), endStation);
            }
            else if (station.ConnectedStationO.NextStation == endStation.Name)
            {
                return true;
            }
            else
            {
                return false;
            }
                    
        }

        public void MoveToPrevious(StationViewModel startStation, StationViewModel endStation)
        {
            while (startStation.ConnectedStationO.PreviousStation != endStation.Name)
            {
                startStation = Stations.ToList().Find(s => s.Name.Trim() == startStation.ConnectedStationO.PreviousStation.Trim());
                StationConnections.SingleOrDefault(s =>
                {
                    if (s.NextStation != null & s.NextStation.Trim() == startStation.ConnectedStationO.NextStation.Trim())
                    {
                        s.IsSelectedConnection = true;
                        return true;
                    }
                    return false;
                });
                startStation.IsSelectedStationInThePath = true;
            }
        }

        private void RemoveStationToSelection()
        {
            StationPathTo.IsSelected = false;
            StationPathTo = null;
        }

        private void ResetSelectedStationsAndStationConnections()
        {
            Stations.Where(s => s.IsSelectedStationInThePath == false | true)
                            .ToList().ForEach(s => { s.IsSelectedStationInThePath = null; });
                        StationConnections.Where(sc => sc.IsSelectedConnection == false | true)
                            .ToList().ForEach(sc => { sc.IsSelectedConnection = null; });
        }

        private void HideStationsAndConnectionsThatIsNotOnTheSelectedPath()
        {
            StationConnections.Where(sc => sc.IsSelectedConnection == null)
                            .ToList().ForEach(sc => { sc.IsSelectedConnection = false; });
            Stations.Where(s => s.IsSelectedStationInThePath == null)
                        .ToList().ForEach(s => { s.IsSelectedStationInThePath = false; });
        }

        private void RemoveStationFromSelection()
        {
            StationPathFrom.IsSelected = false;
            StationPathFrom = null;
        }
    }
}
