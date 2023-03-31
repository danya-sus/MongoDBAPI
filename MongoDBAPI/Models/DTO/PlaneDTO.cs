namespace MongoDBAPI.Models.DTO
{
    public class PlaneDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int? CompanyId { get; set; }
    }
}
