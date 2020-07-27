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
    internal class IngredientsRepository : IIngredientsRepository, IPagedRepository<Ingredient>
    {
        private readonly IMapper mapper;
        private readonly RecipesDbContext recipesDbContext;

        public IngredientsRepository(RecipesDbContext recipesDbContext, IMapper mapper)
        {
            this.recipesDbContext = recipesDbContext;
            this.mapper = mapper;
        }

        public async Task<Ingredient> AddAsync(Ingredient ingredient)
        {
            var ingredientDbModel = this.mapper.Map<IngredientDbModel>(ingredient);

            await this.recipesDbContext.Ingredients.AddAsync(ingredientDbModel).ConfigureAwait(false);
            await this.recipesDbContext.SaveChangesAsync().ConfigureAwait(false);

            return this.mapper.Map<Ingredient>(ingredientDbModel);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var ingredientDbModel = await this.recipesDbContext.Ingredients
                .FirstOrDefaultAsync(x => x.Id == id)
                .ConfigureAwait(false);

            if (ingredientDbModel == null)
            {
                return false;
            }

            try
            {
                this.recipesDbContext.Ingredients.Remove(ingredientDbModel);
                var affectedItems = await this.recipesDbContext.SaveChangesAsync().ConfigureAwait(false);

                return affectedItems > 0;
            }
            catch (DbUpdateException ex)
            {
                // TODO: avoid throwing Exception and create domain logic to handle this
                throw new InvalidOperationException("Unable to delete ingredient", ex);
            }
        }

        public async Task<IEnumerable<Ingredient>> GetAsync(string userId)
        {
            var ingredientsFromDb = await this.recipesDbContext.Ingredients
                .Where(x => x.AppUserId == userId)
                .ToListAsync()
                .ConfigureAwait(false);

            return this.mapper.Map<IEnumerable<Ingredient>>(ingredientsFromDb);
        }

        public async Task<PagedResult<Ingredient>> GetAsync(string userId, PagedRequest pagedRequest)
        {
            var totalItems = await this.recipesDbContext.Ingredients
                .Where(x => x.AppUserId == userId)
                .CountAsync()
                .ConfigureAwait(false);

            var ingredientsFromDb = await this.recipesDbContext.Ingredients
                .Where(x => x.AppUserId == userId)
                .OrderBy(x => x.Title)
                .Skip(pagedRequest.RowsToSkip)
                .Take(pagedRequest.PageSize)
                .ToListAsync()
                .ConfigureAwait(false);

            return new PagedResult<Ingredient>(
                pagedRequest: pagedRequest,
                totalItems: totalItems,
                data: this.mapper.Map<IEnumerable<Ingredient>>(ingredientsFromDb)
            );
        }

        public async Task<Ingredient> GetAsync(Guid id)
        {
            var ingredientDbModel = await this.recipesDbContext.Ingredients
                .FirstOrDefaultAsync(x => x.Id == id)
                .ConfigureAwait(false);

            if (ingredientDbModel == null)
            {
                return null;
            }

            return this.mapper.Map<Ingredient>(ingredientDbModel);
        }

        public async Task<Ingredient> GetAsync(string userId, string slug)
        {
            var ingredientDbModel = await this.recipesDbContext.Ingredients
                .FirstOrDefaultAsync(x => x.AppUserId == userId && x.Slug == slug)
                .ConfigureAwait(false);

            if (ingredientDbModel == null)
            {
                return null;
            }

            return this.mapper.Map<Ingredient>(ingredientDbModel);
        }

        public async Task<bool> UpdateAsync(Ingredient ingredient)
        {
            var ingredientDbModel = await this.recipesDbContext.Ingredients
                .FirstOrDefaultAsync(x => x.Id == ingredient.Id)
                .ConfigureAwait(false);

            if (ingredientDbModel == null || ingredientDbModel.AppUserId != ingredient.AppUserId)
            {
                return false;
            }

            ingredientDbModel.Description = ingredient.Description;
            ingredientDbModel.Title = ingredient.Title;

            await this.recipesDbContext.SaveChangesAsync().ConfigureAwait(false);

            return true;
        }
    }
}