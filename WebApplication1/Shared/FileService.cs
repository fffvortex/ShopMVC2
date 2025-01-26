namespace ShopMVC2.Shared
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> SaveFile(IFormFile file, string[] allowedExtentions)
        {
            var wwwPath = _environment.WebRootPath;
            var path = Path.Combine(wwwPath, "img");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var extention = Path.GetExtension(file.FileName);
            if (!allowedExtentions.Contains(extention))
            {
                throw new InvalidOperationException($"Only {string.Join(",", allowedExtentions)} files allowed");
            }
            string fileName = $"{Guid.NewGuid()}{extention}";
            string fileNameWithPath = Path.Combine(path, fileName);

            using var stream = new FileStream(fileNameWithPath, FileMode.Create);
            await file.CopyToAsync(stream);
            return fileName;
        }
        public void DeleteFile(string fileName)
        {
            var wwwPath = _environment.WebRootPath;
            var fileNameWithPath = Path.Combine(wwwPath, "img\\", fileName);
            if (!File.Exists(fileNameWithPath))
            {
                throw new FileNotFoundException(fileName);
            }
            File.Delete(fileNameWithPath);
        }
    }
}
