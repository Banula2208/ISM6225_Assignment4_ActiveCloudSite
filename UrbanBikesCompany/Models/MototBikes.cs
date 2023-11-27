using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace UrbanBikesCompany.Models
{
    public class MototBikes
    {
        public int bikeId { get; set; }

        public string bikeName { get; set; }

        public string type { get; set; }

        public virtual List<BikeServiceModel>? BikeServiceModels { get; set; }
    }

    public class MototBikesModelConfiguration : IEntityTypeConfiguration<MototBikes>
    {
        public void Configure(EntityTypeBuilder<MototBikes> builder)
        {
            builder.HasKey(x => new { x.bikeId });
        }

    }
}
