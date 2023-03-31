﻿namespace MongoDBAPI.Models.DTO
{
    public class CompanyDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Country { get; set; }

        public int YearOfFoundation { get; set; }
    }
}
