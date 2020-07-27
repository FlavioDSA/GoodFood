import axios, { AxiosResponse } from 'axios';
import { IUser, IUserFormValues } from '../models/user';
import { IIngredient } from '../models/ingredient';
import { IPagedRequest, IPagedResult } from '../models/paging/pagedRequest';
import { IRecipe } from '../models/recipe';
import { IRecipeCategory } from '../models/recipeCategory';

axios.defaults.baseURL = 'http://localhost:59277/api';

axios.interceptors.request.use(
  config => {
    const token = window.localStorage.getItem('jwt');
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);

axios.interceptors.response.use(undefined, error => {
  console.error(error);

  const { status, headers } = error.response;

  if (status === 401 && headers['www-authenticate'] === 'Bearer error="invalid_token", error_description="The token is expired"') {
    window.localStorage.removeItem('jwt');
  }

  throw error.response;
});

const responseBody = (response: AxiosResponse) => response.data;

const requests = {
  get: (url: string) =>
    axios
      .get(url)
      .then(responseBody),
  post: (url: string, body: {}) =>
    axios
      .post(url, body)
      .then(responseBody),
  put: (url: string, body: {}) =>
    axios
      .put(url, body)
      .then(responseBody),
  del: (url: string) =>
    axios
      .delete(url)
      .then(responseBody)
  // postForm: (url: string, file: Blob) => {
  //   let formData = new FormData();
  //   formData.append('File', file);
  //   return axios
  //     .post(url, formData, {
  //       headers: { 'Content-type': 'multipart/form-data' }
  //     })
  //     .then(responseBody);
  // }
};

const User = {
  login: (user: IUserFormValues): Promise<IUser> => {
    return requests.post(`/users/login`, user);
  },
  // register: (user: IUserFormValues): Promise<IUser> =>
  //   requests.post(`/user/register`, user)
};

const Ingredient = {
  listAsync: (pagedRequest: IPagedRequest): Promise<IPagedResult<IIngredient>> => {
    return requests.get(`/myingredients?pageSize=${pagedRequest.pageSize}&currentPage=${pagedRequest.currentPage}`);
  },
  getAsync: (slug: string): Promise<IIngredient> => {
    return requests.get(`/myingredients/${slug}`);
  },
  createAsync: (ingredient: IIngredient): Promise<IIngredient> => {
    return requests.post(`/myingredients`,
      {
        description: ingredient.description,
        slug: ingredient.slug,
        title: ingredient.title
      });
  },
  updateAsync: (ingredient: IIngredient): Promise<void> => {
    return requests.put(`/myingredients/${ingredient.id}`,
      {
        description: ingredient.description,
        title: ingredient.title
      });
  },
  deleteAsync: (id: string): Promise<void> => {
    return requests.del(`/myingredients/${id}`);
  }
};

const Recipe = {
  listAsync: (pagedRequest: IPagedRequest): Promise<IPagedResult<IRecipe>> => {
    return requests.get(`/myrecipes?pageSize=${pagedRequest.pageSize}&currentPage=${pagedRequest.currentPage}`);
  },
  getAsync: (slug: string): Promise<IRecipe> => {
    return requests.get(`/myrecipes/${slug}`);
  },
  createAsync: (recipe: IRecipe): Promise<IRecipe> => {
    return requests.post(`/myrecipes`,
      {
        description: recipe.description,
        slug: recipe.slug,
        title: recipe.title,
        recipeCategoryId: recipe.recipeCategoryId,
        recipeIngredients: recipe.recipeIngredients.map(x => {
          return {
            ingredientId: x.ingredientId,
            amount: x.amount
          };
        })
      });
  },
  updateAsync: (recipe: IRecipe): Promise<void> => {
    return requests.put(`/myrecipes/${recipe.id}`,
      {
        description: recipe.description,
        title: recipe.title,
        recipeCategoryId: recipe.recipeCategoryId,
        recipeIngredients: recipe.recipeIngredients.map(x => {
          return {
            ingredientId: x.ingredientId,
            amount: x.amount
          };
        })
      });
  },
  deleteAsync: (id: string): Promise<void> => {
    return requests.del(`/myrecipes/${id}`);
  }
};

const RecipeCategory = {
  listAsync: (): Promise<IRecipeCategory[]> => {
    return requests.get(`/recipeCategories`);
  }
};

export default {
  User,
  Ingredient,
  Recipe,
  RecipeCategory
};