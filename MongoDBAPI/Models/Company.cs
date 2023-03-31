using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBAPI.Models
{
    public class Company
    {
        public Company()
        {
            this.Planes = new HashSet<Plane>();
        }

        [BsonId]
        public int Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("code")]
        public string Code { get; set; }

        [BsonElement("country")]
        public string Country { get; set; }

        [BsonElement("yearOfFoundation")]
        public int YearOfFoundation { get; set; }

        [BsonElement("planes")]
        public virtual ICollection<Plane>? Planes { get; set; }
    }
}
