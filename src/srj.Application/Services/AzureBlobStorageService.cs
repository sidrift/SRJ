using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using srj.Application.Interface.Services;

namespace srj.Application.Services;

public class AzureBlobStorageService : IFileStorageService
{
    private readonly BlobContainerClient _container;
    private readonly ILogger<AzureBlobStorageService> _logger;

    public AzureBlobStorageService(
        BlobServiceClient blobServiceClient,
        IConfiguration configuration,
        ILogger<AzureBlobStorageService> logger
    )
    {
        _logger = logger;

        var containerName = configuration["AzureStorage:ContainerName"];

        if (string.IsNullOrWhiteSpace(containerName))
            throw new InvalidOperationException("Azure Storage ContainerName is missing.");

        _container = blobServiceClient.GetBlobContainerClient(containerName);
    }

    public async Task<string> SaveFileAsync(byte[] fileBytes, string folder, string fileName)
    {
        var blobName = $"{folder.Trim('/')}/{Path.GetFileName(fileName)}";

        var blob = _container.GetBlobClient(blobName);

        _logger.LogInformation("1. Starting Azure upload: {BlobName}", blobName);

        try
        {
            using var stream = new MemoryStream(fileBytes);

            _logger.LogInformation("2. Uploading file to Azure Blob Storage");

            await blob.UploadAsync(
                stream,
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = "image/png" },
                }
            );

            _logger.LogInformation("3. Azure upload successful: {BlobName}", blobName);

            return blobName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Azure upload failed for {BlobName}", blobName);

            throw;
        }
    }

    public async Task DeleteFileAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return;

        var blobName = filePath.TrimStart('/');
        var blob = _container.GetBlobClient(blobName);

        await blob.DeleteIfExistsAsync();
    }

    public async Task<byte[]?> GetFileAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return null;

        var blob = _container.GetBlobClient(filePath.TrimStart('/'));

        if (!await blob.ExistsAsync())
            return null;

        var response = await blob.DownloadContentAsync();

        return response.Value.Content.ToArray();
    }
}