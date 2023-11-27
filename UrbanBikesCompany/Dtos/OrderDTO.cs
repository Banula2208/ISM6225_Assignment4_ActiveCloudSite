namespace UrbanBikesCompany.DTos
{
    public class OrderDTO
    {

        public int OrderId { get; set; }

        public string vehiclenum { get; set; }


        public DateTime recievedDate { get; set; }


        public DateTime deliveryDate { get; set; }

        public int bs_id { get; set; }


        public string Name { get; set; }

        public string Contact { get; set; }

        public string bikeServicetype { get; set; }
    }
}
