using BarcodeStandard;
using SkiaSharp;
using srj.Application.Interface.Services;
using Type = BarcodeStandard.Type;

namespace srj.Application.Services;

public class BarcodeService : IBarcodeService
{
    private readonly IFileStorageService _fileStorage;

    public BarcodeService(IFileStorageService fileStorage)
    {
        _fileStorage = fileStorage;
    }

    public async Task<string> GenerateBarcodeAsync(
        string sku,
        decimal weightInGrams)
    {
        var barcode = new Barcode();

        using var barcodeImage = barcode.Encode(
            Type.Code128,
            sku,
            SKColors.Black,
            SKColors.White,
            400,
            100);


        const int width = 420;
        const int height = 180;


        using var surface = SKSurface.Create(
            new SKImageInfo(width, height));

        var canvas = surface.Canvas;

        canvas.Clear(SKColors.White);

        using var paint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true
        };

        using var font = new SKFont
        {
            Size = 18
        };

        canvas.DrawText(
            $"Weight: {weightInGrams:F2} g",
            width / 2f,
            22,
            SKTextAlign.Center,
            font,
            paint);

        canvas.DrawImage(
            barcodeImage,
            10,
            35);

        canvas.DrawText(
            sku,
            width / 2f,
            165,
            SKTextAlign.Center,
            font,
            paint);

        using var image = surface.Snapshot();

        using var data = image.Encode(
            SKEncodedImageFormat.Png,
            100);

        var fileName = $"{sku}.png";

        return await _fileStorage.SaveFileAsync(
            data.ToArray(),
            "barcodes",
            fileName);
    }
}