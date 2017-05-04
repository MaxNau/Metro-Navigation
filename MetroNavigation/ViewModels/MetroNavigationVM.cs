using MetroNavigation.Models;
using System.Collections.Generic;
using System.Linq;

namespace MetroNavigation.ViewModels
{
    public class MetroNavigationVM
    {
        public int? MinX { get; }
        public int? MaxX { get; }
        public int? MinY { get; }
        public int? MaxY { get; }
        public List<Station> Stations { get; set; }

        public MetroNavigationVM()
        {
            using (var context = new StationEntity())
            {
                Stations = context.Stations.ToList();
            }

            MinX = Stations.Min(s => s.OsX);
            MaxX = Stations.Max(s => s.OsX);
            MinY = Stations.Min(s => s.OsY);
            MaxY = Stations.Max(s => s.OsY);
        }
    }
}
