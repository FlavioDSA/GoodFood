import React, { useContext, useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Card, Button, Segment, Item } from 'semantic-ui-react';
import { StoresContext } from '../../../app/stores/stores';
import { RouteComponentProps } from 'react-router-dom';
import agent from '../../../app/api/agent';

interface RouteParams {
  slug: string;
}

const RecipeDetails: React.FC<RouteComponentProps<RouteParams>> = ({ match, history }) => {

  const stores = useContext(StoresContext);
  let { selectedRecipe, selectRecipe, cancelSelectedRecipe, recipeCategories, deleteRecipeAsync } = stores.recipeStore;

  useEffect(() => {
    agent.Recipe.getAsync(match.params.slug)
      .then(recipe => { 
        selectRecipe(recipe);
      });
  }, [match.params.slug, selectRecipe]);

  const onCancel = () => {
    cancelSelectedRecipe();
    history.push('/myRecipes')
  };

  const onDelete = () => {
    deleteRecipeAsync(selectedRecipe!);
    history.push('/myRecipes');
  };

  return (
    <Card fluid>
      <Card.Content>
        <Card.Header>{selectedRecipe && (selectedRecipe.title)}</Card.Header>
        <Card.Meta>
          <p style={{paddingLeft: '2px'}}>
            {selectedRecipe && (recipeCategories.find(x => x.id === selectedRecipe?.recipeCategoryId)?.name)}
          </p>
        </Card.Meta>
        <Card.Description>
          <Segment clearing>
            <p style={{padding: '5px'}}>
              {selectedRecipe && (selectedRecipe.description)}
            </p>
          </Segment>
        </Card.Description>
        <Segment clearing>
          <Item.Group divided>
          <Item>
              <Item.Content>
                <Item.Meta>Ingredients</Item.Meta>
                </Item.Content>
              </Item>
            {selectedRecipe?.recipeIngredients.map(recipeIngredient => recipeIngredient?.ingredient && (
              <Item key={recipeIngredient.ingredient && (recipeIngredient.ingredient.id)}>
                <Item.Content>
                  <Item.Header>{recipeIngredient.ingredient && (recipeIngredient.ingredient.title)}</Item.Header>
                  <Item.Meta>{recipeIngredient && (recipeIngredient.amount)}</Item.Meta>
                </Item.Content>
              </Item>
            ))}
          </Item.Group>
        </Segment>
      </Card.Content>
      <Card.Content extra>
        <Button.Group widths={3}>
          <Button basic color='red' content='Delete' onClick={onDelete} />
          <Button basic color='grey' content='Return' onClick={onCancel} />
          <Button basic color='teal' content='Edit' onClick={() => history.push(`/myRecipes/edit/${selectedRecipe?.slug}`)} />
        </Button.Group>
      </Card.Content>
    </Card>
  );
};

export default observer(RecipeDetails);