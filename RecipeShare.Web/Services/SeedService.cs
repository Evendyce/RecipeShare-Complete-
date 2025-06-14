using Microsoft.AspNetCore.Identity;
using RecipeShare.Web.Data.Models;
using RecipeShare.Web.Data;
using Microsoft.EntityFrameworkCore;

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

                // Create Role
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                // Create User
                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, roleName);
                    if (!roleResult.Succeeded)
                    {
                        var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                        return $"User created but role assignment failed: {errors}";
                    }

                    var userProfile = new UserProfile
                    {
                        UserId = user.Id,
                        DisplayName = "Demo User",
                        Bio = "Just a demo foodie sharing love and carbs.",
                        ProfileImageUrl = null, // or some placeholder image URL
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = null
                    };

                    await ctx.UserProfiles.AddAsync(userProfile);
                    await ctx.SaveChangesAsync();
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return $"Failed to create user: {errors}";
                }

                // Create Recipes
                var recipes = new List<Recipe>
                {
                    new Recipe
                    {
                        Title = "Spaghetti Carbonara",
                        Ingredients = "Spaghetti, Eggs, Parmesan, Bacon, Pepper",
                        CookingTime = 25,
                        DietaryTags = "Pasta,Meat",
                        UserId = user.Id
                    },
                    new Recipe
                    {
                        Title = "Veggie Stir-Fry",
                        Ingredients = "Broccoli, Bell Pepper, Carrots, Soy Sauce, Garlic",
                        CookingTime = 15,
                        DietaryTags = "Vegetarian,Gluten-Free",
                        UserId = user.Id
                    },
                    new Recipe
                    {
                        Title = "Chicken Curry",
                        Ingredients = "Chicken, Curry Powder, Onion, Tomato, Coconut Milk",
                        CookingTime = 40,
                        DietaryTags = "Meat,Spicy",
                        UserId = user.Id
                    }
                };

                await ctx.Recipes.AddRangeAsync(recipes);
                await ctx.SaveChangesAsync();

                var steps = new List<RecipeStep>
                {
                    new RecipeStep { Recipe = recipes[0], StepNumber = 1, Instruction = "Boil the spaghetti." },
                    new RecipeStep { Recipe = recipes[0], StepNumber = 2, Instruction = "Fry the bacon." },
                    new RecipeStep { Recipe = recipes[0], StepNumber = 3, Instruction = "Mix eggs and cheese, then combine all." },

                    new RecipeStep { Recipe = recipes[1], StepNumber = 1, Instruction = "Chop vegetables." },
                    new RecipeStep { Recipe = recipes[1], StepNumber = 2, Instruction = "Stir-fry in pan." },
                    new RecipeStep { Recipe = recipes[1], StepNumber = 3, Instruction = "Add soy sauce and serve." },

                    new RecipeStep { Recipe = recipes[2], StepNumber = 1, Instruction = "Cook onions and tomatoes." },
                    new RecipeStep { Recipe = recipes[2], StepNumber = 2, Instruction = "Add chicken and spices." },
                    new RecipeStep { Recipe = recipes[2], StepNumber = 3, Instruction = "Pour coconut milk and simmer." }
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
