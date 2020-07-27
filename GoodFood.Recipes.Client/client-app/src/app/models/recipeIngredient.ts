import { IIngredient } from './ingredient';

export interface IRecipeIngredient {
  id: string;
  amount: string;
  ingredientId: string;
  ingredient: IIngredient;
}