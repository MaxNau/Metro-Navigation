namespace MetroNavigation.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MetroNavigationContext : DbContext
    {
        public MetroNavigationContext()
            : base("name=MetroNavigationContext")
        {
        }

        public virtual DbSet<StationConnection> StationConnections { get; set; }
        public virtual DbSet<Station> Stations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Station>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Station>()
                .Property(e => e.ConnectedStation)
                .IsUnicode(false);

            modelBuilder.Entity<Station>()
                .HasOptional(e => e.StationConnection)
                .WithRequired(e => e.Station);
        }
    }
}
