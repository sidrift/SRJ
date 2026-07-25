using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using srj.Domain.Enums;

namespace srj.Application.Dtos.Request.Gold;

public class UpdateGoldJewelryRequest
{
    [Required] public string Sku { get; set; } = string.Empty;

    [Required] public long CategoryId { get; set; }

    [Required]
    [Range(typeof(decimal), "0.001", "10000", ErrorMessage = "Weight must be between 0.001g and 10,000g.")]
    public decimal WeightInGrams { get; set; }

    [DefaultValue(false)] public bool HasStones { get; set; } = false;

    [DefaultValue(0)] public decimal? StoneWeight { get; set; }

    [DefaultValue(null)] public string? StoneType { get; set; }

    [DefaultValue(0)] public decimal? StonePrice { get; set; }

    [DefaultValue(91.6)]
    [Range(0.001, 100)]
    public decimal Purity { get; set; } = 91.6m;

    [DefaultValue(MakingChargeType.Percentage)]
    public MakingChargeType MakingChargeType { get; set; } = MakingChargeType.Percentage;

    [DefaultValue(10)] public decimal? MakingChargePercentage { get; set; } = 10m;

    [DefaultValue(null)] public decimal? MakingChargeWeight { get; set; }

    [DefaultValue(250)] public decimal HallMarkCharge { get; set; } = 250m;
}