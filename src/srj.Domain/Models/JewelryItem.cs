using srj.Domain.Enums;

namespace srj.Domain.Models;

public class JewelryItem
{
    // Primary Key
    public Guid Id { get; set; }

    // Identification
    public string Sku { get; set; } = string.Empty;

    public long CategoryId { get; set; }
    public ItemCategory Category { get; set; } = null!;

    // Common
    public decimal WeightInGrams { get; set; }

    public decimal PurityInPercentage { get; set; }

    public string BarcodeImageUrl { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    #region Stone Details

    public bool HasStones { get; set; }

    public decimal? StoneWeight { get; set; }

    public string? StoneType { get; set; }

    public decimal? StonePrice { get; set; }

    #endregion

    #region Gold Only

    public MakingChargeType? MakingChargeType { get; set; }

    public decimal? MakingChargePercentage { get; set; }

    public decimal? MakingChargeWeight { get; set; }

    public decimal? HallMarkCharge { get; set; }

    #endregion

    #region Silver Only

    public decimal? SilverMakingChargePerGram { get; set; }

    #endregion
}