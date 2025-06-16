using System.Net;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using RecipeShare.API;
using RecipeShare.Models.Shared;
using System.Net.Http.Json;

namespace RecipeShare.Tests
{
    public class RecipeApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public RecipeApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Recipes_Returns_OK()
        {
            var response = await _client.GetAsync("/api/recipes");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Post_ValidRecipeDto_Returns_Created()
        {
            var dto = new RecipeDto
            {
                Title = "Test Recipe",
                UserName = "DemoUser",
                Ingredients = "Salt, Water",
                Steps = "Step 1\nStep 2",
                CookingTime = 15,
                DietaryTags = "Low-Carb"
                // Tags, Images, StructuredSteps will be default empty
            };

            var response = await _client.PostAsJsonAsync("/api/recipes", dto);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Post_InvalidRecipeDto_Returns_BadRequest()
        {
            var dto = new RecipeDto
            {
                Title = "",
                UserName = "DemoUser",
                Ingredients = "Salt",
                Steps = "",
                CookingTime = 0,
                DietaryTags = ""
            };

            var response = await _client.PostAsJsonAsync("/api/recipes", dto);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
