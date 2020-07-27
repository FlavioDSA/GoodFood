import { IRecipeIngredient } from './recipeIngredient';
import { IRecipeCategory } from './recipeCategory';

export interface IRecipe {
  id: string;
  title: string;
  slug: string;
  description: string;
  recipeCategoryId: string;
  recipeCategory: IRecipeCategory;
  recipeIngredients: IRecipeIngredient[];
}