namespace DocuZoneAPI.Model
{
    public interface IBlobService
    {
        Task<string> UploadAsync(IFormFile file);
        Task<Stream> DownloadAsync(string fileName);
        Task<List<string>> ListFilesAsync();
    }
}
