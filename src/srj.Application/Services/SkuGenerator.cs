using srj.Application.Interface.Services;
using srj.Domain.Enums;
using srj.Domain.Models;

namespace srj.Application.Services;

public class SkuGenerator : ISkuGenerator
{
    public Task<string> GenerateAsync(Metal metal, ItemCategory category)
    {
        var categoryCode = category.Name
            .Replace(" ", "")
            .ToUpperInvariant();

        categoryCode = categoryCode.Length > 5
            ? categoryCode[..5]
            : categoryCode;

        // 12-character unique identifier
        var uniqueId = Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();

        var sku = $"{categoryCode}-{uniqueId}";

        return Task.FromResult(sku);
    }
}