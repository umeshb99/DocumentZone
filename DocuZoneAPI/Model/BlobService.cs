using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace DocuZoneAPI.Model
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _containerClient;
        public BlobService(IConfiguration _config)
        {
            var Connectionstring = _config["ConnectionStrings:AzureBlobStorageURL"];
            var StorageaccountName = _config["ConnectionStrings:AzureBlobStorageName"];
            var blobServiceClient = new BlobServiceClient(Connectionstring);
            _containerClient = blobServiceClient.GetBlobContainerClient(StorageaccountName);
            _containerClient.CreateIfNotExists(PublicAccessType.None);
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            var blobClient = _containerClient.GetBlobClient(file.FileName);
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);
            return blobClient.Uri.ToString();
        }

        public async Task<Stream> DownloadAsync(string fileName)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }

        public async Task<List<string>> ListFilesAsync()
        {
            var files = new List<string>();
            await foreach (BlobItem item in _containerClient.GetBlobsAsync())
            {
                files.Add(item.Name);
            }
            return files;
        }

    }
}

