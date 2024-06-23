namespace ProductManagement.Contracts
{
    public interface IFileService
    {
        IFormFile GetFormFile(string uniqueFileName);
        string GetMimeTypeForFileExtension(string filePath);
        Task<string> ProcessUploadedExcelFileAsync(IFormFile productExcelFile);
        string ProcessUploadedFile(IFormFile productImageFile);
    }
}