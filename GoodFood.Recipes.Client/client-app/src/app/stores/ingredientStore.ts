import { observable, action, runInAction } from 'mobx';
import { IPagedRequest, IPagedResult } from '../models/paging/pagedRequest';
import { IIngredient } from '../models/ingredient';
import { Stores } from './stores';
import agent from '../api/agent';
import { toast } from 'react-toastify';


export default class IngredientStore {

  stores: Stores;

  @observable pagedRequest: IPagedRequest = { currentPage: 1, pageSize: 5 };
  @observable pagedResult: IPagedResult<IIngredient> = { currentPage: 1, pageSize: 5, totalItems:0, totalPages: 0, data: [] };
  @observable selectedIngredient: IIngredient | undefined | null;
  @observable editMode = false;

  constructor(stores: Stores) {
    this.stores = stores;
  }

  @action setPagedRequest = (pagedRequest: IPagedRequest) => { 
    this.pagedRequest = pagedRequest;
  };

  @action loadIngredients = async () => {
    try {
      let pagedResult = await agent.Ingredient.listAsync(this.pagedRequest);

      runInAction('loading ingredients', () => {
        this.pagedResult = pagedResult;
      })

    } catch (error) {
      runInAction('loading ingredients error', () => {
      })
    }
  };

  @action createIngredient = async (ingredient: IIngredient) => {
    try {
      let newIngredient = await agent.Ingredient.createAsync(ingredient);

      runInAction('creating ingredient', () => {
        this.editMode = false;
        this.selectedIngredient = newIngredient;
        this.pagedRequest = { currentPage: 1, pageSize: 5 };
        this.loadIngredients();
      });

    } catch (error) {
      runInAction('creating ingredient error', () => {
      });
      toast.error('Error creating ingredient.');
    }
  };

  @action editIngredient = async (ingredient: IIngredient) => {
    try {
      await agent.Ingredient.updateAsync(ingredient);

      runInAction('editing ingredient', () => {
        this.editMode = false;
        this.selectedIngredient = ingredient;
        this.pagedRequest = { currentPage: 1, pageSize: 5 };
        this.loadIngredients();
      });

    } catch (error) {
      runInAction('editing ingredient error', () => {
      });
      toast.error('Error editing ingredient.');
    }
  };

  @action deleteIngredientAsync = async (ingredient: IIngredient) => {
    try {
      await agent.Ingredient.deleteAsync(ingredient.id);

      runInAction('deleting ingredient', () => {
        this.pagedRequest = { currentPage: 1, pageSize: 5 };
        this.selectedIngredient = null;
        this.loadIngredients();
      });

    } catch (error) {
      runInAction('deleting ingredient error', () => {
      });
      toast.error('Can´t delete this ingredient. Please make sure it is not used by any recipe.');
    }
  };

  @action openCreateForm = () => {
    this.editMode = true;
    this.selectedIngredient = {
      id: '',
      title: '',
      description: '',
      slug: ''
    };
  };

  @action openEditForm = () => {
    this.editMode = true;
  }

  @action cancelSelectedIngredient = () => {
    this.selectedIngredient = null;
  }

  @action cancelFormOpen = () => {
    this.editMode = false;
    if (!this.selectedIngredient?.id)
      this.selectedIngredient = null;
  }

  @action selectIngredient = (ingredient: IIngredient) => {
    this.editMode = false;
    this.selectedIngredient = ingredient;
  };
}
