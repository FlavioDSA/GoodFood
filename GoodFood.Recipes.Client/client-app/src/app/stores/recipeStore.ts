import { observable, action, runInAction } from 'mobx';
import { Stores } from './stores';
import agent from '../api/agent';
import { IPagedRequest, IPagedResult } from '../models/paging/pagedRequest';
import { IRecipe } from '../models/recipe';
import { IRecipeCategory } from '../models/recipeCategory';

export default class RecipeStore {

  stores: Stores;

  @observable pagedRequest: IPagedRequest = { currentPage: 1, pageSize: 5 };
  @observable pagedResult: IPagedResult<IRecipe> = { currentPage: 1, pageSize: 5, totalItems: 0, totalPages: 0, data: [] };
  @observable selectedRecipe: IRecipe | undefined | null;
  @observable recipeCategories: IRecipeCategory[] = [];

  constructor(stores: Stores) {
    this.stores = stores;

    this.loadRecipeCategoriesAsync();
  }

  @action setPagedRequest = (pagedRequest: IPagedRequest) => {
    this.pagedRequest = pagedRequest;
  };

  @action loadRecipeCategoriesAsync = async (): Promise<IRecipeCategory[]> => {
    if (this.recipeCategories && this.recipeCategories.length > 0)
      return this.recipeCategories;

    try {
      let categories = await agent.RecipeCategory.listAsync();
      
      runInAction('loading recipe categories', () => {
        this.recipeCategories = categories;
      });

      return this.recipeCategories;
    } catch (error) {
      runInAction('loading recipe categories error', () => {
      });
      return [];
    }
  };

  @action loadRecipes = async () => {
    try {
      let pagedResult = await agent.Recipe.listAsync(this.pagedRequest);

      runInAction('loading recipes', () => {
        this.pagedResult = pagedResult;
      });

    } catch (error) {
      runInAction('loading recipes error', () => {
      });
    }
  };

  @action createRecipeAsync = async (recipe: IRecipe): Promise<IRecipe | undefined | null> => {
    try {
      let newRecipe = await agent.Recipe.createAsync(recipe);

      runInAction('creating recipe', () => {
        this.pagedRequest = { currentPage: 1, pageSize: 5 };
        this.selectRecipe(newRecipe);
        this.loadRecipes();
      });

      return this.selectedRecipe;

    } catch (error) {
      runInAction('creating recipe error', () => {
      });
      return null;
    }
  };

  @action editRecipeAsync = async (recipe: IRecipe) => {
    try {
      await agent.Recipe.updateAsync(recipe);

      runInAction('editing recipe', () => {
        this.pagedRequest = { currentPage: 1, pageSize: 5 };
        this.selectedRecipe = recipe;
        this.loadRecipes();
      });

    } catch (error) {
      runInAction('editing recipe error', () => {
      });
    }
  };

  @action deleteRecipeAsync = async (recipe: IRecipe) => {
    try {
      await agent.Recipe.deleteAsync(recipe.id);

      runInAction('deleting recipe', () => {
        this.pagedRequest = { currentPage: 1, pageSize: 5 };
        this.selectedRecipe = null;
        this.loadRecipes();
      });

    } catch (error) {
      runInAction('deleting recipe error', () => {
      });
    }
  };

  @action openCreateForm = () => {
    this.selectedRecipe = {
      id: '',
      title: '',
      slug: '',
      description: '',
      recipeCategoryId: this.recipeCategories[0].id,
      recipeCategory: this.recipeCategories[0],
      recipeIngredients: []
    };
  };

  @action cancelSelectedRecipe = () => {
    this.selectedRecipe = null;
  }

  @action selectRecipe = (recipe: IRecipe | undefined | null) => {
    this.selectedRecipe = recipe;
  };
}
