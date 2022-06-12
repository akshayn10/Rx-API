namespace Rx.Domain.Interfaces.Blob;

public interface IBlobStorage
{
    Task<string> UploadLogo(FileStream fileStream);

    Task<string> UploadProfile(FileStream stream);
    // Task<byte[]> DownloadFile(string fileName);
    Task DeleteLogo(string oldFileName);
    Task DeleteProfile(string oldFileName);

}