using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace srj.Application.Dtos.Request.Silver;

public class UpdateSilverJewelryRequest
{
    [Required] public string Sku { get; set; } = string.Empty;

    [Required] public long CategoryId { get; set; }

    [Required]
    [Range(typeof(decimal), "0.001", "10000", ErrorMessage = "Weight must be between 0.001g and 10,000g.")]
    public decimal WeightInGrams { get; set; }

    /// <summary>
    ///     Additional amount charged per gram above today's silver rate.
    ///     Example:
    ///     Today's rate = ₹230/g
    ///     Flat rate = ₹10/g
    ///     Customer price = ₹240/g
    /// </summary>
    [DefaultValue(10)]
    [Range(0, double.MaxValue)]
    public decimal SilverMakingChargePerGram { get; set; } = 10m;

    [Required] [Range(0.001, 100)] public decimal Purity { get; set; }
}