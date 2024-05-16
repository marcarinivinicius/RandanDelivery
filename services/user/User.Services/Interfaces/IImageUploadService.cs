namespace User.Services.Interfaces
{
    public interface IClientImageUploadService
    {
        public Task<string> UploadImageAsync(string pathS3, string filePath, string contentType, Stream? imageStream = null);
    }
}
