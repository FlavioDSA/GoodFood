using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodFood.Recipes.Application.Users.Model;
using GoodFood.Recipes.Data.DbModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GoodFood.Recipes.Data
{
    public static class SeedDb
    {
        public static async Task SeedAsync(RecipesDbContext recipesDbContext, UserManager<AppUser> userManager)
        {
            await SeedUsersAsync().ConfigureAwait(false);
            await SeedRecipeCategoriesAsync().ConfigureAwait(false);
            await SeedIngredientsAsync().ConfigureAwait(false);
            await SeedRecipesAsync().ConfigureAwait(false);

            async Task SeedUsersAsync()
            {
                if (!(await userManager.Users.AnyAsync().ConfigureAwait(false)))
                {
                    await userManager.CreateAsync(
                        new AppUser
                        {
                            Id = "2dd5cc76-69c5-4e06-999f-e4729fae7007",
                            UserName = "flavio",
                            Email = "flavio@test.com",
                            DisplayName = "Flávio"
                        },
                        "P@$$w0rd"
                    ).ConfigureAwait(false);

                    await userManager.CreateAsync(
                        new AppUser
                        {
                            Id = "830b047d-688e-4b87-9dc8-0c43ce0b6bd6",
                            UserName = "bob",
                            Email = "bob@test.com",
                            DisplayName = "Bob"
                        },
                        "P@$$w0rd"
                    ).ConfigureAwait(false);

                    await userManager.CreateAsync(
                        new AppUser
                        {
                            Id = "482c108c-1aa8-470d-8da4-79c2fa4dacd3",
                            UserName = "ana",
                            Email = "ana@test.com",
                            DisplayName = "Ana"
                        },
                        "P@$$w0rd"
                    ).ConfigureAwait(false);
                }
            }

            async Task SeedRecipeCategoriesAsync()
            {
                if (!(await recipesDbContext.RecipeCategories.AnyAsync().ConfigureAwait(false)))
                {
                    var categories = new List<RecipeCategoryDbModel>
                    {
                        new RecipeCategoryDbModel
                        {
                            Id = Guid.Parse("DB1C9F39-B411-438E-A22F-08D82E94C492"),
                            Name = "starters"
                        },
                        new RecipeCategoryDbModel
                        {
                            Id = Guid.Parse("D96C9777-71F3-4B19-A230-08D82E94C492"),
                            Name = "main cours"
                        },
                        new RecipeCategoryDbModel
                        {
                            Id = Guid.Parse("728F3814-DD08-4303-A231-08D82E94C492"),
                            Name = "dessert"
                        }
                    };

                    await recipesDbContext.RecipeCategories.AddRangeAsync(categories).ConfigureAwait(false);
                    await recipesDbContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }

            async Task SeedIngredientsAsync()
            {
                if (!(await recipesDbContext.Ingredients.AnyAsync().ConfigureAwait(false)))
                {
                    var ingredients = new IngredientDbModel[]
                    {
                        // Flávio
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("d89903d1-9fec-4d15-8def-b0b7ad434626"),
                            Title = "Rice",
                            Description = "My rice",
                            Slug = "rice",
                            AppUserId = "2dd5cc76-69c5-4e06-999f-e4729fae7007"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("49d71246-3402-4091-bce4-41321418d56d"),
                            Title = "Bean",
                            Description = "My bean",
                            Slug = "bean",
                            AppUserId = "2dd5cc76-69c5-4e06-999f-e4729fae7007"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("4aea61e0-baae-441e-9cbf-7a084cc562f1"),
                            Title = "Onion",
                            Description = "My onion",
                            Slug = "onion",
                            AppUserId = "2dd5cc76-69c5-4e06-999f-e4729fae7007"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("9f888a25-5fe5-49f6-9fcd-44800cb71a53"),
                            Title = "Garlic",
                            Description = "My garlic",
                            Slug = "garlic",
                            AppUserId = "2dd5cc76-69c5-4e06-999f-e4729fae7007"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("5234729b-5ca9-4c4d-9ca4-672d60d4e935"),
                            Title = "Butter",
                            Description = "My butter",
                            Slug = "butter",
                            AppUserId = "2dd5cc76-69c5-4e06-999f-e4729fae7007"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("6727676d-633b-4d40-8d38-3d14e575f104"),
                            Title = "Sugar",
                            Description = "My sugar",
                            Slug = "sugar",
                            AppUserId = "2dd5cc76-69c5-4e06-999f-e4729fae7007"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("c0f24bb4-a82a-4ad8-aa45-db9d88f277f7"),
                            Title = "Pepper",
                            Description = "My pepper",
                            Slug = "pepper",
                            AppUserId = "2dd5cc76-69c5-4e06-999f-e4729fae7007"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("6f8a93b8-d413-4bbe-b02a-612251a3f39b"),
                            Title = "Lemon",
                            Description = "My lemon",
                            Slug = "lemon",
                            AppUserId = "2dd5cc76-69c5-4e06-999f-e4729fae7007"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("73cf361c-3638-4935-8ff7-2b5ed7e8e1fe"),
                            Title = "Water",
                            Description = "My water",
                            Slug = "water",
                            AppUserId = "2dd5cc76-69c5-4e06-999f-e4729fae7007"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("4e9163d8-66f4-4e2d-b7dc-b12d024bb86b"),
                            Title = "Milk",
                            Description = "My milk",
                            Slug = "milk",
                            AppUserId = "2dd5cc76-69c5-4e06-999f-e4729fae7007"
                        },

                        // Bob
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("04f2744b-3fda-48b2-a7ea-e5406e7f18a6"),
                            Title = "Rice",
                            Description = "My rice",
                            Slug = "rice",
                            AppUserId = "830b047d-688e-4b87-9dc8-0c43ce0b6bd6"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("56b05153-8171-4ee0-a6aa-c288a54ccc1d"),
                            Title = "Bean",
                            Description = "My bean",
                            Slug = "bean",
                            AppUserId = "830b047d-688e-4b87-9dc8-0c43ce0b6bd6"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("c655974f-8680-4437-8a0c-f10fd9d72689"),
                            Title = "Onion",
                            Description = "My onion",
                            Slug = "onion",
                            AppUserId = "830b047d-688e-4b87-9dc8-0c43ce0b6bd6"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("ab2b4b81-ce0b-49df-ac96-27e214ae2a16"),
                            Title = "Garlic",
                            Description = "My garlic",
                            Slug = "garlic",
                            AppUserId = "830b047d-688e-4b87-9dc8-0c43ce0b6bd6"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("f0103394-3e34-48f9-ba1d-4f5edf7d5575"),
                            Title = "Butter",
                            Description = "My butter",
                            Slug = "butter",
                            AppUserId = "830b047d-688e-4b87-9dc8-0c43ce0b6bd6"
                        },

                        // Ana
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("2e92ae30-ae77-4c51-8ef4-cd69d2ffe2da"),
                            Title = "Rice",
                            Description = "My rice",
                            Slug = "rice",
                            AppUserId = "482c108c-1aa8-470d-8da4-79c2fa4dacd3"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("3a502eb8-985a-446d-b1a4-3ba8b6ec11eb"),
                            Title = "Bean",
                            Description = "My bean",
                            Slug = "bean",
                            AppUserId = "482c108c-1aa8-470d-8da4-79c2fa4dacd3"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("e04eca0f-52dd-404f-8359-2159f467829a"),
                            Title = "Onion",
                            Description = "My onion",
                            Slug = "onion",
                            AppUserId = "482c108c-1aa8-470d-8da4-79c2fa4dacd3"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("84aae61f-f34b-4de9-b03c-d24f2be2fda2"),
                            Title = "Garlic",
                            Description = "My garlic",
                            Slug = "garlic",
                            AppUserId = "482c108c-1aa8-470d-8da4-79c2fa4dacd3"
                        },
                        new IngredientDbModel
                        {
                            Id = Guid.Parse("3b1dcda4-8b64-41b8-97bf-51d7f8352f2d"),
                            Title = "Butter",
                            Description = "My butter",
                            Slug = "butter",
                            AppUserId = "482c108c-1aa8-470d-8da4-79c2fa4dacd3"
                        }
                    };

                    await recipesDbContext.Ingredients.AddRangeAsync(ingredients).ConfigureAwait(false);
                    await recipesDbContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }

            async Task SeedRecipesAsync()
            {
                if (!(await recipesDbContext.Recipes.AnyAsync().ConfigureAwait(false)))
                {
                    var recipes = new List<RecipeDbModel>
                    {
                        // Flávio
                        new RecipeDbModel
                        {
                            Id = Guid.Parse("0c13696f-b432-4274-98f3-28be7f10273c"),
                            Title = "Recipe 1",
                            Description = "My recipe 1",
                            Slug = "my-recipe-1",
                            AppUserId = "2dd5cc76-69c5-4e06-999f-e4729fae7007",
                            RecipeCategoryId = Guid.Parse("D96C9777-71F3-4B19-A230-08D82E94C492"),
                            RecipeIngredients = new List<RecipeIngredientDbModel>
                            {
                                new RecipeIngredientDbModel
                                {
                                    Id = Guid.Parse("7faffc14-6b3a-425f-b1da-302a02f1fdbc"),
                                    Amount = "1 kg",
                                    IngredientId = Guid.Parse("d89903d1-9fec-4d15-8def-b0b7ad434626")
                                },
                                new RecipeIngredientDbModel
                                {
                                    Id = Guid.Parse("bb606617-911c-4f8b-a49a-027f8c41a3ce"),
                                    Amount = "1 kg",
                                    IngredientId = Guid.Parse("49d71246-3402-4091-bce4-41321418d56d")
                                }
                            }
                        },
                        new RecipeDbModel
                        {
                            Id = Guid.Parse("3074E662-BB75-435C-A68E-08D8310C7EE9"),
                            Title = "Recipe 2",
                            Description = "My recipe 2",
                            Slug = "my-recipe-2",
                            AppUserId = "2dd5cc76-69c5-4e06-999f-e4729fae7007",
                            RecipeCategoryId = Guid.Parse("D96C9777-71F3-4B19-A230-08D82E94C492"),
                            RecipeIngredients = new List<RecipeIngredientDbModel>
                            {
                                new RecipeIngredientDbModel
                                {
                                    Id = Guid.Parse("c8c7254a-2dd4-4612-869c-1b9885df9bc8"),
                                    Amount = "1 kg",
                                    IngredientId = Guid.Parse("d89903d1-9fec-4d15-8def-b0b7ad434626")
                                },
                                new RecipeIngredientDbModel
                                {
                                    Id = Guid.Parse("cee69c14-17a4-4f9c-8973-9ac55201e883"),
                                    Amount = "1 kg",
                                    IngredientId = Guid.Parse("49d71246-3402-4091-bce4-41321418d56d")
                                }
                            }
                        },

                        // Bob
                        new RecipeDbModel
                        {
                            Id = Guid.Parse("3d56291e-93df-4eac-9634-37aa128b590a"),
                            Title = "Recipe 1",
                            Description = "My recipe 1",
                            Slug = "my-recipe-1",
                            AppUserId = "830b047d-688e-4b87-9dc8-0c43ce0b6bd6",
                            RecipeCategoryId = Guid.Parse("D96C9777-71F3-4B19-A230-08D82E94C492"),
                            RecipeIngredients = new List<RecipeIngredientDbModel>
                            {
                                new RecipeIngredientDbModel
                                {
                                    Id = Guid.Parse("d1f5cf46-291c-4508-9052-0e8ffcd88665"),
                                    Amount = "1 kg",
                                    IngredientId = Guid.Parse("04f2744b-3fda-48b2-a7ea-e5406e7f18a6")
                                },
                                new RecipeIngredientDbModel
                                {
                                    Id = Guid.Parse("ff07d4d1-ebe1-4737-9172-f6ed069f4698"),
                                    Amount = "1 kg",
                                    IngredientId = Guid.Parse("56b05153-8171-4ee0-a6aa-c288a54ccc1d")
                                }
                            }
                        },

                        // Ana
                        new RecipeDbModel
                        {
                            Id = Guid.Parse("957abe28-d22f-45d1-baea-bd62a84810fc"),
                            Title = "Recipe 1",
                            Description = "My recipe 1",
                            Slug = "my-recipe-1",
                            AppUserId = "482c108c-1aa8-470d-8da4-79c2fa4dacd3",
                            RecipeCategoryId = Guid.Parse("D96C9777-71F3-4B19-A230-08D82E94C492"),
                            RecipeIngredients = new List<RecipeIngredientDbModel>
                            {
                                new RecipeIngredientDbModel
                                {
                                    Id = Guid.Parse("076c4c50-6a16-45df-8c7b-615f2b970709"),
                                    Amount = "1 kg",
                                    IngredientId = Guid.Parse("2e92ae30-ae77-4c51-8ef4-cd69d2ffe2da")
                                },
                                new RecipeIngredientDbModel
                                {
                                    Id = Guid.Parse("702e1ed0-453b-458c-bc73-ad68730625c3"),
                                    Amount = "1 kg",
                                    IngredientId = Guid.Parse("3a502eb8-985a-446d-b1a4-3ba8b6ec11eb")
                                }
                            }
                        }
                    };

                    await recipesDbContext.Recipes.AddRangeAsync(recipes).ConfigureAwait(false);
                    await recipesDbContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }
    }
}