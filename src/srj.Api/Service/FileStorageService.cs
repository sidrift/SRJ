using srj.Application.Interface.Services;

namespace TheSRJProject.Service;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;

    public FileStorageService(
        IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveFileAsync(
        byte[] fileBytes,
        string folder,
        string fileName)
    {
        var path = Path.Combine(
            _environment.WebRootPath,
            folder);


        Directory.CreateDirectory(path);


        var fullPath = Path.Combine(
            path,
            fileName);


        await File.WriteAllBytesAsync(
            fullPath,
            fileBytes);


        return $"/{folder}/{fileName}";
    }

    public Task DeleteFileAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return Task.CompletedTask;


        var fullPath = Path.Combine(
            _environment.WebRootPath,
            filePath.TrimStart('/')
                .Replace('/', Path.DirectorySeparatorChar));


        if (File.Exists(fullPath)) File.Delete(fullPath);


        return Task.CompletedTask;
    }
}