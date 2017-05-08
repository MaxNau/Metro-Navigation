namespace MetroNavigation.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Station
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int OsX { get; set; }

        public int OsY { get; set; }

        public int Line { get; set; }

        [StringLength(50)]
        public string ConnectedStation { get; set; }
    }
}
