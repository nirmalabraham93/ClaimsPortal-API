namespace ClaimsPortalAPI.Models
{
    public class AddVehicleDto
    {
        public int Id { get; set; }
        public string Make { get; set; } = null!;

        public string Model { get; set; } = null!;

        public string Year { get; set; } = null!;

        public string Vin { get; set; } = null!;

        public string EngineNumber { get; set; } = null!;

        public string ChasisNumber { get; set; } = null!;

        public int PolicyHolderId { get; set; }
    }
}
