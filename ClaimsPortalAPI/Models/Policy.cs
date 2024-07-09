using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ClaimsPortalAPI.Models;

public partial class Policy
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
    [JsonIgnore]
    public virtual PolicyHolder PolicyHolder { get; set; } = null!;
    [JsonIgnore]
    public virtual Vehicle Vehicle { get; set; } = null!;
}
