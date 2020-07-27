import React, { useContext, useEffect, useState } from 'react';
import { Form as FinalForm } from 'react-final-form';
import { observer } from 'mobx-react-lite';
import { Grid } from 'semantic-ui-react';
import RecipeForm from './RecipeForm';
import RecipeIngredientSelector from './RecipeIngredientSelector';
import { RouteComponentProps } from 'react-router-dom';
import { StoresContext } from '../../../app/stores/stores';
import agent from '../../../app/api/agent';
import { IRecipe } from '../../../app/models/recipe';
import arrayMutators from "final-form-arrays";
import { IRecipeIngredient } from '../../../app/models/recipeIngredient';
import { IIngredient } from '../../../app/models/ingredient';
import { combineValidators, isRequired } from 'revalidate';

const validate = combineValidators({
  title: isRequired('Title'),
  slug: isRequired('Slug'),
  description: isRequired('Description'),
  recipeCategoryId: isRequired('Category')
});

interface RouteParams {
  slug: string;
}

const RecipeEditDashboard: React.FC<RouteComponentProps<RouteParams>> = ({ match, history }) => {

  const stores = useContext(StoresContext);
  let { recipeCategories, createRecipeAsync, editRecipeAsync, loadRecipeCategoriesAsync } = stores.recipeStore;

  const [editingRecipe, setEditingRecipe] = useState<IRecipe>({
    id: '',
    title: '',
    slug: '',
    description: '',
    recipeCategoryId: '',
    recipeCategory: {
      id: '',
      name: ''
    },
    recipeIngredients: []
  });

  useEffect(() => {
    if (match?.params?.slug) {
      agent.Recipe.getAsync(match.params.slug)
        .then(recipe => {
          setEditingRecipe(recipe)
        });
    }
    else {
      loadRecipeCategoriesAsync()
        .then((categories) => {
          setEditingRecipe({
            id: '',
            title: '',
            slug: '',
            description: '',
            recipeCategoryId: categories[0].id,
            recipeCategory: categories[0],
            recipeIngredients: []
          });
        });
    }
  }, [match, match.params.slug, loadRecipeCategoriesAsync]);

  const isNewRecipe = () => !editingRecipe?.id;

  const onSubmit = async (recipe: IRecipe) => {
    recipe.recipeCategory = recipeCategories.find(x => x.id === recipe.recipeCategoryId)!;

    console.log(recipe);

    if (isNewRecipe()) {
      let newRecipe = await createRecipeAsync(recipe);
      history.push(`/myRecipes/${newRecipe?.slug}`);
    } else {
      await editRecipeAsync(recipe);
      history.push(`/myRecipes/${recipe?.slug}`);
    }
  };

  return (
    <FinalForm
      initialValues={editingRecipe}
      mutators={{
        ...arrayMutators
      }}
      onSubmit={onSubmit}
      validate={validate}
      render={({
        handleSubmit,
        form: {
          mutators: { push, remove }
        },
        values,
        invalid
      }) => (
          <Grid>
            <Grid.Column width='10'>
              <RecipeForm handleSubmit={handleSubmit} editingRecipe={values} isInvalid={invalid}></RecipeForm>
            </Grid.Column>
            <Grid.Column width='6'>
              <RecipeIngredientSelector
                addIngredient={(recipeIngredient: IRecipeIngredient) => push("recipeIngredients", recipeIngredient)}
                removeIngredient={(ingredient: IIngredient) => remove("recipeIngredients", values.recipeIngredients.findIndex(x => x.ingredientId === ingredient.id))}
                editingRecipe={values} />
            </Grid.Column>
          </Grid>
        )}
    />
  )
}

export default observer(RecipeEditDashboard);