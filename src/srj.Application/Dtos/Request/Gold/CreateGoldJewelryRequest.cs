using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using srj.Domain.Enums;

namespace srj.Application.Dtos.Request.Gold;

public class CreateGoldJewelryRequest
{
    [Required] public long CategoryId { get; set; }

    [Range(typeof(decimal), "0.001", "10000", ErrorMessage = "Weight must be between 0.001g and 10,000g.")]
    public decimal WeightInGrams { get; set; }

    [DefaultValue(false)] public bool HasStones { get; set; } = false;

    [DefaultValue(null)] public decimal? StoneWeight { get; set; } = null;

    [DefaultValue(null)] public string? StoneType { get; set; } = null;

    [DefaultValue(null)] public decimal? StonePrice { get; set; } = null;

    [DefaultValue(91.6)] public decimal Purity { get; set; } = 91.6m;

    [DefaultValue(10)] public decimal? MakingChargePercentage { get; set; } = 10m;

    [DefaultValue(250)] public decimal HallMarkCharge { get; set; } = 250m;

    [DefaultValue(MakingChargeType.Percentage)]
    public MakingChargeType MakingChargeType { get; set; }

    [DefaultValue(0)] public decimal? MakingChargeWeight { get; set; } = 0m;
}