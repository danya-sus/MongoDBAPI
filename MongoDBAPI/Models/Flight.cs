﻿using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBAPI.Models
{
    public class Flight
    {
        [BsonId]
        public int Id { get; set; }

        [BsonElement("from")]
        public string From { get; set; }

        [BsonElement("to")]
        public string To { get; set; }

        [BsonElement("plane_id")]
        public int? PlaneId { get; set; }
    }
}
