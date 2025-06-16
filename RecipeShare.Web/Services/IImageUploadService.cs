using Microsoft.AspNetCore.Components.Forms;

namespace RecipeShare.Web.Services
{
    public interface IImageUploadService
    {
        Task<string> SaveAsync(IBrowserFile file, long recipeId, bool isCover);
        Task<string> SaveTempAsync(IBrowserFile file, Guid tempId, bool isCover);
        Task DeleteAsync(string relativeUrl);
        Task FinalizeUploadAsync(Guid tempId, long newRecipeId);
    }

    public class ImageUploadService : IImageUploadService
    {
        private readonly IWebHostEnvironment _env;

        public ImageUploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveAsync(IBrowserFile file, long recipeId, bool isCover)
        {
            var folder = isCover ? "CoverImage" : "Additional";
            var uploadPath = Path.Combine(
                _env.WebRootPath,
                "uploads",
                "RecipeImages",
                recipeId.ToString(),
                folder);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";
            var fullPath = Path.Combine(uploadPath, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.OpenReadStream(maxAllowedSize: 10_000_000).CopyToAsync(stream);

            return $"/uploads/RecipeImages/{recipeId}/{folder}/{fileName}";
        }

        public async Task<string> SaveTempAsync(IBrowserFile file, Guid tempId, bool isCover)
        {
            var folder = isCover ? "CoverImage" : "Additional";
            var uploadPath = Path.Combine(
                _env.WebRootPath,
                "uploads",
                "TempRecipeImages",
                tempId.ToString(),
                folder);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";
            var fullPath = Path.Combine(uploadPath, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.OpenReadStream(maxAllowedSize: 10_000_000).CopyToAsync(stream);

            return $"/uploads/TempRecipeImages/{tempId}/{folder}/{fileName}";
        }

        public Task DeleteAsync(string relativeUrl)
        {
            var fullPath = Path.Combine(
                _env.WebRootPath,
                relativeUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }

        public Task FinalizeUploadAsync(Guid tempId, long newRecipeId)
        {
            var tempRoot = Path.Combine(
                _env.WebRootPath,
                "uploads",
                "TempRecipeImages",
                tempId.ToString());

            var finalRoot = Path.Combine(
                _env.WebRootPath,
                "uploads",
                "RecipeImages",
                newRecipeId.ToString());

            if (!Directory.Exists(tempRoot))
                return Task.CompletedTask;

            foreach (var dir in Directory.GetDirectories(tempRoot, "*", SearchOption.AllDirectories))
            {
                var relativeSubPath = Path.GetRelativePath(tempRoot, dir);
                var targetSubPath = Path.Combine(finalRoot, relativeSubPath);
                Directory.CreateDirectory(targetSubPath);
            }

            foreach (var file in Directory.GetFiles(tempRoot, "*", SearchOption.AllDirectories))
            {
                var relativeFile = Path.GetRelativePath(tempRoot, file);
                var destFile = Path.Combine(finalRoot, relativeFile);
                File.Move(file, destFile, overwrite: true);
            }

            Directory.Delete(tempRoot, recursive: true);
            return Task.CompletedTask;
        }
    }
}
