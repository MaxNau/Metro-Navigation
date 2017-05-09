namespace MetroNavigation.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StationConnection
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string NextStation { get; set; }

        [StringLength(50)]
        public string PreviousStation { get; set; }

        public virtual Station Station { get; set; }
    }
}
