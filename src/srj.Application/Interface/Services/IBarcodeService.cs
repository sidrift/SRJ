namespace srj.Application.Interface.Services;

public interface IBarcodeService
{
    Task<string> GenerateBarcodeAsync(string sku, decimal weightInGrams);
}