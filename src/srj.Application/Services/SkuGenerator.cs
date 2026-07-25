using srj.Application.Interface.Services;
using srj.Domain.Enums;
using srj.Domain.Models;

namespace srj.Application.Services;

public class SkuGenerator : ISkuGenerator
{
    public Task<string> GenerateAsync(Metal metal, ItemCategory category)
    {
        var metalCode = metal == Metal.Gold ? "G" : "S";
        var categoryCode = category.Name
            .Replace(" ", "")
            .ToUpperInvariant();

        // 12-character unique identifier
        var uniqueId = Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();

        var sku = $"{metalCode}-{categoryCode}-{uniqueId}";

        return Task.FromResult(sku);
    }
}