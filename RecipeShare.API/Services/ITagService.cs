using Microsoft.EntityFrameworkCore;
using RecipeShare.API.Data;
using RecipeShare.API.Helpers;
using RecipeShare.Models.Shared;

namespace RecipeShare.API.Services
{
    public interface ITagService
    {
        Task<List<TagDto>> GetAllTagsAsync();
        // Future expansion: Task<Tag?> GetByIdAsync(int id); etc.
    }

    public class TagService : ITagService
    {
        private readonly IDbContextFactory<RecipeShareContext> _contextFactory;
        private readonly ILogger<TagService> _logger;

        public TagService(IDbContextFactory<RecipeShareContext> contextFactory, ILogger<TagService> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<List<TagDto>> GetAllTagsAsync()
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();

                var tagDtos = await context.Tags
                    .OrderBy(t => t.Name)
                    .Select(t => CustomMapper.TagMapper.ToDto(t))
                    .ToListAsync();

                return tagDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[TagService] Failed to fetch tag list");
                return new();
            }
        }
    }
}
