using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace UrbanBikesCompany.Models
{
    public class ServiceOrder
    {
        public int? OrderId { get; set; }

        public string vehiclenum { get; set; }


        public DateTime recievedDate { get; set; }


        public DateTime deliveryDate { get; set; }

        public int? bs_id { get; set; }


        public string Name { get; set; }

        public string Contact { get; set; }

        public virtual List<BikeServiceModel> BikeServiceModel { get; set; }
    }

    public class ServiceOrderModelConfiguration : IEntityTypeConfiguration<ServiceOrder>
    {
        public void Configure(EntityTypeBuilder<ServiceOrder> builder)
        {
            builder.HasKey(x => new { x.OrderId });


                }

    }
}
