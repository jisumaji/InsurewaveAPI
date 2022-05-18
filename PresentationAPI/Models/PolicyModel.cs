using System;
using System.Collections.Generic;

namespace PresentationAPI.Models;

public class PolicyModel
{
    public int PolicyId { get; set; }
    public int AssetId { get; set; }
    public string InsurerId { get; set; } = null!;
    public string BrokerId { get; set; } = null!;
    public int Duration { get; set; }
    public decimal Premium { get; set; }
    public decimal LumpSum { get; set; }
    public DateTime StartDate { get; set; }
    public int PremiumInterval { get; set; }
    public decimal MaturityAmount { get; set; }
    public string? PolicyStatus { get; set; }
    public string? ReviewStatus { get; set; }
    public string? Feedback { get; set; }
}
