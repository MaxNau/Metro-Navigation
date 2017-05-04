namespace MetroNavigation.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class StationEntity : DbContext
    {
        public StationEntity()
            : base("StationEntity")
        {
        }

        public virtual DbSet<Station> Stations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Station>()
                .Property(e => e.Name)
                .IsFixedLength();
        }
    }
}
