
namespace ShopMVC2.Shared
{
    public interface IFileService
    {
        void DeleteFile(string fileName);
        Task<string> SaveFile(IFormFile file, string[] allowedExtentions);
    }
}