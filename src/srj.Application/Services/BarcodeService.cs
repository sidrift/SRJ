using System.Runtime.InteropServices;
using SkiaSharp;
using srj.Application.Interface.Services;
using ZXing;
using ZXing.Common;

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
        // -----------------------------
        // Data Matrix
        // -----------------------------
        const int matrixSize = 120;

        var writer = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.DATA_MATRIX,
            Options = new EncodingOptions
            {
                Width = matrixSize,
                Height = matrixSize,
                Margin = 1,
                PureBarcode = true
            }
        };

        var pixelData = writer.Write(sku);

        using var matrixBitmap = new SKBitmap(
            new SKImageInfo(
                pixelData.Width,
                pixelData.Height,
                SKColorType.Bgra8888,
                SKAlphaType.Premul));

        Marshal.Copy(
            pixelData.Pixels,
            0,
            matrixBitmap.GetPixels(),
            pixelData.Pixels.Length);

        // -----------------------------
        // Label dimensions
        // -----------------------------
        const int width = 260;
        const int height = 120;

        // Displayed Data Matrix size
        const int matrixDrawSize = 80;

        // -----------------------------
        // Create canvas
        // -----------------------------
        using var surface = SKSurface.Create(
            new SKImageInfo(width, height));

        var canvas = surface.Canvas;

        canvas.Clear(SKColors.White);

        // -----------------------------
        // Paint
        // -----------------------------
        using var paint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true
        };

        // -----------------------------
        // Font
        // -----------------------------
        using var font = new SKFont
        {
            Size = 12
        };

        // -----------------------------
        // WEIGHT - TOP
        // -----------------------------
        canvas.DrawText(
            $"Weight: {weightInGrams:F2} g",
            width / 2f,
            13f,
            SKTextAlign.Center,
            font,
            paint);

        // -----------------------------
        // DATA MATRIX - MIDDLE
        // -----------------------------
        const float matrixY = 16f;

        var matrixX = (width - matrixDrawSize) / 2f;

        canvas.DrawBitmap(
            matrixBitmap,
            SKRect.Create(
                matrixX,
                matrixY,
                matrixDrawSize,
                matrixDrawSize),
            paint);

        // -----------------------------
        // SKU - BOTTOM
        // -----------------------------
        const float skuGap = 5f;

        var skuY = matrixY + matrixDrawSize + skuGap + 10f;

        canvas.DrawText(
            sku,
            width / 2f,
            skuY,
            SKTextAlign.Center,
            font,
            paint);

        // -----------------------------
        // Generate PNG
        // -----------------------------
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