using Microsoft.EntityFrameworkCore;
using RecipeShare.Models.Shared;
using RecipeShare.Web.Data;

namespace RecipeShare.Web.Services
{
    public interface IRecipeService
    {
        Task<List<RecipeTileDto>> GetRecipeTilesAsync(RecipeSearchDto filter);
    }

    public class RecipeService : IRecipeService
    {
        private readonly HttpClient _http;
        private readonly ILogger<RecipeService> _logger;

        public RecipeService(HttpClient http, ILogger<RecipeService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<List<RecipeTileDto>> GetRecipeTilesAsync(RecipeSearchDto filter)
        {
            try
            {
                var url = $"/api/recipes?{BuildQueryString(filter)}";

                var response = await _http.GetFromJsonAsync<List<RecipeTileDto>>(url);

                return response ?? new List<RecipeTileDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RecipeService] Failed to load recipe tiles");
                return new List<RecipeTileDto>();
            }
        }

        private static string BuildQueryString(RecipeSearchDto filter)
        {
            var queryParams = new Dictionary<string, string> { { "useTile", "true" } };

            if (!string.IsNullOrWhiteSpace(filter.Title))
                queryParams["Title"] = filter.Title;

            if (filter.Tags?.Any() == true)
                foreach (var tag in filter.Tags)
                    queryParams.Add("Tags", tag);

            if (filter.MinCookingTime.HasValue)
                queryParams["MinCookingTime"] = filter.MinCookingTime.Value.ToString();

            if (filter.MaxCookingTime.HasValue)
                queryParams["MaxCookingTime"] = filter.MaxCookingTime.Value.ToString();

            return string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));
        }
    }
}
