namespace Rx.Domain.Interfaces.Blob;

public interface IBlobStorage
{
    Task<string> UploadFile(FileStream fileStream,string name);
    // Task<byte[]> DownloadFile(string fileName);
    // Task<bool> DeleteFile(string fileName);
}