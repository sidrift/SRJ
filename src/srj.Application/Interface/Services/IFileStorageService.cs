namespace srj.Application.Interface.Services;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(
        byte[] fileBytes,
        string folder,
        string fileName);

    Task DeleteFileAsync(string filePath);
}