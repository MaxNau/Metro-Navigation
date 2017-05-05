using MetroNavigation.Controls;
using MetroNavigation.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MetroNavigation.ViewModels
{
    public class StationViewModel
    {
        public double? X { get; set; }
        public double? Y { get; set; }
        public string Name { get; set; }
        public int? Line { get; set; }
    }

    public class MetroNavigationVM
    {
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }
        
        public ObservableCollection<StationViewModel> Stations { get;  set; }

        public MetroNavigationVM()
        {
            Load();
        }

        private void Load()
        {
            Stations = new ObservableCollection<StationViewModel>();

            List<Station> stationsData;
            double canvasLeft;
            double canvasBottom;
            StationControl sc = new StationControl();

            using (var context = new StationEntity())
            {
                stationsData = context.Stations.ToList();
            }

            MinX = stationsData.Min(s => s.OsX).Value;
            MaxX = stationsData.Max(s => s.OsX).Value;
            MinY = stationsData.Min(s => s.OsY).Value;
            MaxY = stationsData.Max(s => s.OsY).Value;

            foreach (var station in stationsData)
            {
                canvasLeft = (SystemParameters.PrimaryScreenWidth - 100) *
                    (station.OsX.Value - MinX) / (MaxX - MinX);

                canvasBottom = (SystemParameters.PrimaryScreenHeight - 100 - 50) *
                    (station.OsY.Value - MinY) / (MaxY - MinY);

                canvasLeft = canvasLeft > (SystemParameters.PrimaryScreenWidth - 100 - sc.MaxWidth / 2)
                                          ? (canvasLeft - sc.MaxWidth / 2)
                                          : canvasLeft;

                canvasBottom = canvasBottom > (SystemParameters.PrimaryScreenHeight - 100 - 50 - sc.MaxHeight / 2)
                                          ? canvasBottom - (sc.MaxHeight / 2)
                                          : canvasBottom;

                Stations.Add(new StationViewModel() { X = canvasLeft, Y = canvasBottom, Name = station.Name, Line = station.Line });
            }
        }
    }
}
