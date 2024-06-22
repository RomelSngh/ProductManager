namespace ProductManagement.Contracts
{
    public interface IFileService
    {
        IFormFile GetFormFile(string uniqueFileName);
        string GetMimeTypeForFileExtension(string filePath);
        string ProcessUploadedFile(IFormFile productImageFile);
    }
}