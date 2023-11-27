using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace UrbanBikesCompany.Models
{
    public class ServiceModel
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }

        public virtual List<BikeServiceModel> BikeServiceModels { get; set; }
    }

    public class ServiceModelConfiguration : IEntityTypeConfiguration<ServiceModel>
    {
        public void Configure(EntityTypeBuilder<ServiceModel> builder)
        {
            builder.HasKey(x => new { x.ServiceId });

        }
    }
}
