namespace ClaimsPortalAPI.Models
{
    public class AddPolicyDto
    {
        public int Id { get; set; }
        public string PolicyType { get; set; } = null!;

        public string PolicyNumber { get; set; } = null!;

        public DateTime CoverageStartDate { get; set; }

        public DateTime CoverageEndDate { get; set; }

        public decimal CoverageAmount { get; set; }

        public decimal PremiumAmount { get; set; }

        public int PolicyHolderId { get; set; }

        public int VehicleId { get; set; }
    }
}
