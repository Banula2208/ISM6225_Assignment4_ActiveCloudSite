using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace UrbanBikesCompany.Models
{
    public class BikeServiceModel
    {
        public int bs_id { get; set; }

        public int service_id { get; set; }


        public int bike_id { get; set; }

        public float price { get; set;}

        public ServiceModel service { get; set; }


        public MototBikes bike { get; set; }

        public virtual ServiceOrder? ServiceOrder { get; set; }

        public int? OrderId { get; set; }

    }

    public class BikeServiceModelConfiguration : IEntityTypeConfiguration<BikeServiceModel>
    {
        public void Configure(EntityTypeBuilder<BikeServiceModel> builder)
        {
            builder.HasKey(x => new { x.bs_id });

            builder.HasOne(x => x.service).WithMany(x => x.BikeServiceModels).HasForeignKey(x => x.service_id).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(x => x.bike).WithMany(x => x.BikeServiceModels).HasForeignKey(x => x.bike_id).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(x => x.ServiceOrder).WithMany(x => x.BikeServiceModel).HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.ClientSetNull);



        }

    }
}
