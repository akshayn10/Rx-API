namespace Rx.Domain.Interfaces.Blob;

public interface IBlobStorage
{
    Task<string> UploadFile(FileStream fileStream);
    // Task<byte[]> DownloadFile(string fileName);
    Task DeleteFile(string oldFileName);
}