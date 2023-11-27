using Microsoft.EntityFrameworkCore;
using UrbanBikesCompany.Models;

namespace UrbanBikesCompany
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {


        }
        public DbSet<ServiceModel> services { get; set; }

        public DbSet<MototBikes> bikes { get; set; }

        public DbSet<BikeServiceModel> bikeServiceModel { get; set; }

        public DbSet<ServiceOrder> orders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ServiceModelConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceOrderModelConfiguration());
            modelBuilder.ApplyConfiguration(new BikeServiceModelConfiguration());
            modelBuilder.ApplyConfiguration(new MototBikesModelConfiguration());


        }

    }
}
