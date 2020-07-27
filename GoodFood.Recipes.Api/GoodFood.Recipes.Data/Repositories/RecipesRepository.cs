using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoodFood.Recipes.Application.Common.Paging;
using GoodFood.Recipes.Data.DbModel;
using GoodFood.Recipes.Domain.Model;
using GoodFood.Recipes.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GoodFood.Recipes.Data.Repositories
{
    internal sealed class RecipesRepository : IRecipesRepository, IPagedRepository<Recipe>
    {
        private readonly IMapper mapper;
        private readonly RecipesDbContext recipesDbContext;

        public RecipesRepository(RecipesDbContext recipesDbContext, IMapper mapper)
        {
            this.recipesDbContext = recipesDbContext;
            this.mapper = mapper;
        }

        public async Task<Recipe> AddAsync(Recipe recipe)
        {
            var recipeDbModel = this.mapper.Map<RecipeDbModel>(recipe);

            await this.recipesDbContext.Recipes.AddAsync(recipeDbModel).ConfigureAwait(false);
            await this.recipesDbContext.SaveChangesAsync().ConfigureAwait(false);

            return this.mapper.Map<Recipe>(recipeDbModel);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var recipeDbModel = await this.recipesDbContext.Recipes
                .FirstOrDefaultAsync(x => x.Id == id)
                .ConfigureAwait(false);

            if (recipeDbModel == null)
            {
                return false;
            }

            this.recipesDbContext.Recipes.Remove(recipeDbModel);
            var affectedItems = await this.recipesDbContext.SaveChangesAsync().ConfigureAwait(false);

            return affectedItems > 0;
        }

        public async Task<IEnumerable<Recipe>> GetAsync(string userId)
        {
            var recipesFromDb = await this.recipesDbContext.Recipes
                .Where(x => x.AppUserId == userId)
                .Include(x => x.RecipeCategory)
                .ToListAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IEnumerable<Recipe>>(recipesFromDb);
        }

        public async Task<Recipe> GetAsync(Guid id)
        {
            var recipeDbModel = await this.recipesDbContext.Recipes
                .Include(x => x.RecipeCategory)
                .Include(x => x.RecipeIngredients).ThenInclude(x => x.Ingredient)
                .FirstOrDefaultAsync(x => x.Id == id)
                .ConfigureAwait(false);

            if (recipeDbModel == null)
            {
                return null;
            }

            return this.mapper.Map<Recipe>(recipeDbModel);
        }

        public async Task<Recipe> GetAsync(string userId, string slug)
        {
            var recipeDbModel = await this.recipesDbContext.Recipes
                .Include(x => x.RecipeCategory)
                .Include(x => x.RecipeIngredients).ThenInclude(x => x.Ingredient)
                .FirstOrDefaultAsync(x => x.AppUserId == userId && x.Slug == slug)
                .ConfigureAwait(false);

            if (recipeDbModel == null)
            {
                return null;
            }

            return this.mapper.Map<Recipe>(recipeDbModel);
        }

        public async Task<PagedResult<Recipe>> GetAsync(string userId, PagedRequest pagedRequest)
        {
            var totalItems = await this.recipesDbContext.Recipes
                .Where(x => x.AppUserId == userId)
                .CountAsync()
                .ConfigureAwait(false);

            var recipesFromDb = await this.recipesDbContext.Recipes
                .Include(x => x.RecipeCategory)
                .Where(x => x.AppUserId == userId)
                .OrderBy(x => x.Title)
                .Skip(pagedRequest.RowsToSkip)
                .Take(pagedRequest.PageSize)
                .ToListAsync()
                .ConfigureAwait(false);

            return new PagedResult<Recipe>(
                pagedRequest: pagedRequest,
                totalItems: totalItems,
                data: this.mapper.Map<IEnumerable<Recipe>>(recipesFromDb)
            );
        }

        public async Task<bool> UpdateAsync(Recipe recipe)
        {
            var recipeDbModel = await this.recipesDbContext.Recipes
                .Include(x => x.RecipeIngredients)
                .FirstOrDefaultAsync(x => x.Id == recipe.Id)
                .ConfigureAwait(false);

            if (recipeDbModel == null || recipeDbModel.AppUserId != recipe.AppUserId)
            {
                return false;
            }

            foreach (var recipeIngredient in recipeDbModel.RecipeIngredients)
            {
                this.recipesDbContext.RecipeIngredients.Remove(recipeIngredient);
            }

            recipeDbModel.RecipeIngredients.Clear();

            recipeDbModel.Description = recipe.Description;
            recipeDbModel.Title = recipe.Title;
            recipeDbModel.RecipeCategoryId = recipe.RecipeCategoryId;
            recipeDbModel.RecipeIngredients = recipe.RecipeIngredients.Select(x =>
                new RecipeIngredientDbModel
                {
                    IngredientId = x.IngredientId,
                    Amount = x.Amount
                }).ToList();

            await this.recipesDbContext.SaveChangesAsync().ConfigureAwait(false);

            return true;
        }
    }
}