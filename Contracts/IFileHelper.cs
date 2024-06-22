namespace ProductManagement.Contracts
{
    public interface IFileHelper
    {
        IFormFile GetFormFile(string uniqueFileName);
        string GetMimeTypeForFileExtension(string filePath);
        string ProcessUploadedFile(IFormFile productImageFile);
    }
}