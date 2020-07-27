import UserStore from './userStore';
import IngredientStore from './ingredientStore';
import { createContext } from 'react';
import { configure } from 'mobx';
import RecipeStore from './recipeStore';

configure({enforceActions: 'always'});

export class Stores {
  userStore: UserStore;  
  ingredientStore: IngredientStore;
  recipeStore: RecipeStore;

    constructor() {
      this.userStore = new UserStore(this);
      this.ingredientStore = new IngredientStore(this);
      this.recipeStore = new RecipeStore(this);
    }
}

export const StoresContext = createContext(new Stores());