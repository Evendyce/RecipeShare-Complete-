using Microsoft.AspNetCore.Identity;
using RecipeShare.Web.Data.Models;
using RecipeShare.Web.Data;
using Microsoft.EntityFrameworkCore;
using Azure;

namespace RecipeShare.Web.Services
{
    public class SeedService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDbContextFactory<ApplicationDbContext> _identityContextFactory;
        private readonly IDbContextFactory<RecipeShareContext> _recipeContextFactory;
        private readonly ILogger<SeedService> _logger;

        public SeedService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IDbContextFactory<ApplicationDbContext> identityFactory, IDbContextFactory<RecipeShareContext> recipeFactory, ILogger<SeedService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _identityContextFactory = identityFactory;
            _recipeContextFactory = recipeFactory;
            _logger = logger;
        }

        public async Task<bool> IsSeededAsync()
        {
            await using var identityDb = await _identityContextFactory.CreateDbContextAsync();
            await using var recipeDb = await _recipeContextFactory.CreateDbContextAsync();

            var identitySeeded = await identityDb.Users.AnyAsync(u => u.UserName == "DemoUser")
                              && await identityDb.Roles.AnyAsync(r => r.Name == "User");

            var recipeSeeded = await recipeDb.Recipes.AnyAsync();

            return identitySeeded || recipeSeeded;
        }

        public async Task<string> SeedAsync()
        {
            await using var ctx = await _recipeContextFactory.CreateDbContextAsync();

            const string roleName = "User";
            const string userName = "DemoUser";
            const string email = "demo@recipeshare.com";
            const string password = "Password123!";

            if (await IsSeededAsync())
                return "Database already seeded.";

            try
            {
                _logger.LogInformation("Seeding user: {userName}", userName);

                // Role
                if (!await _roleManager.RoleExistsAsync(roleName))
                    await _roleManager.CreateAsync(new IdentityRole(roleName));

                // User
                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                    return $"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}";

                await _userManager.AddToRoleAsync(user, roleName);

                var userProfile = new UserProfile
                {
                    UserId = user.Id,
                    DisplayName = "Demo User",
                    Bio = "Just a demo foodie sharing love and carbs.",
                    CreatedAt = DateTime.UtcNow
                };

                await ctx.UserProfiles.AddAsync(userProfile);

                // Step 1: Seed Tags
                var tagNames = new[] { "Pasta", "Meat", "Vegetarian", "Gluten-Free", "Spicy" };

                var tagEntities = tagNames
                    .Select(name => new Tag { Name = name })
                    .ToList();

                await ctx.Tags.AddRangeAsync(tagEntities);
                await ctx.SaveChangesAsync();

                // Map for reuse
                var tagMap = tagEntities.ToDictionary(t => t.Name, t => t);

                // Step 2: Create Recipes with RecipeTags
                var recipes = new List<Recipe>
                {
                    new Recipe
                    {
                        Title = "Spaghetti Carbonara",
                        Ingredients = "Spaghetti, Eggs, Parmesan, Bacon, Pepper",
                        CookingTime = 25,
                        UserId = user.Id,
                        Tags = new List<Tag>
                        {
                            tagMap["Pasta"],
                            tagMap["Meat"]
                        }
                    },
                    new Recipe
                    {
                        Title = "Veggie Stir-Fry",
                        Ingredients = "Broccoli, Bell Pepper, Carrots, Soy Sauce, Garlic",
                        CookingTime = 15,
                        UserId = user.Id,
                        Tags = new List<Tag>
                        {
                            tagMap["Vegetarian"],
                            tagMap["Gluten-Free"]
                        }
                    },
                    new Recipe
                    {
                        Title = "Chicken Curry",
                        Ingredients = "Chicken, Curry Powder, Onion, Tomato, Coconut Milk",
                        CookingTime = 40,
                        UserId = user.Id,
                        Tags = new List<Tag>
                        {
                            tagMap["Meat"],
                            tagMap["Spicy"]
                        }
                    }
                };

                await ctx.Recipes.AddRangeAsync(recipes);
                await ctx.SaveChangesAsync();

                // Step 3: Add steps
                var steps = new List<RecipeStep>
                {
                    new() { Recipe = recipes[0], StepNumber = 1, Instruction = "Boil the spaghetti." },
                    new() { Recipe = recipes[0], StepNumber = 2, Instruction = "Fry the bacon." },
                    new() { Recipe = recipes[0], StepNumber = 3, Instruction = "Mix eggs and cheese, then combine all." },

                    new() { Recipe = recipes[1], StepNumber = 1, Instruction = "Chop vegetables." },
                    new() { Recipe = recipes[1], StepNumber = 2, Instruction = "Stir-fry in pan." },
                    new() { Recipe = recipes[1], StepNumber = 3, Instruction = "Add soy sauce and serve." },

                    new() { Recipe = recipes[2], StepNumber = 1, Instruction = "Cook onions and tomatoes." },
                    new() { Recipe = recipes[2], StepNumber = 2, Instruction = "Add chicken and spices." },
                    new() { Recipe = recipes[2], StepNumber = 3, Instruction = "Pour coconut milk and simmer." }
                };

                await ctx.RecipeSteps.AddRangeAsync(steps);
                await ctx.SaveChangesAsync();

                return "Seeding complete.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Seeding failed");
                return "Seeding failed due to unexpected error.";
            }
        }
    }
}
