using RecipeShare.Models.Shared;

namespace RecipeShare.Web.Services
{
    public interface ITagService
    {
        Task<List<TagDto>> GetAllTagsAsync();
    }

    public class TagService : ITagService
    {
        private readonly HttpClient _http;
        private readonly ILogger<TagService> _logger;

        public TagService(HttpClient http, ILogger<TagService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<List<TagDto>> GetAllTagsAsync()
        {
            try
            {
                var tags = await _http.GetFromJsonAsync<List<TagDto>>("/api/tag");

                return tags ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[TagService] Failed to fetch tag list");
                return new();
            }
        }
    }
}
