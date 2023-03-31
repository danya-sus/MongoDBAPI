namespace MongoDBAPI.Models.DTO
{
    public class FlightDTO
    {
        public int Id { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public int? PlaneId { get; set; }
    }
}
