using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBAPI.Models
{
    public class Plane
    {
        public Plane()
        {
            this.Flights = new HashSet<Flight>();
        }

        [BsonId]
        public int Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("code")]
        public string Code { get; set; }

        [BsonElement("company_id")]
        public int? CompanyId { get; set; }

        [BsonElement("flights")]
        public ICollection<Flight>? Flights { get; set; }
    }
}
