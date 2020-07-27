import React, { useContext, useEffect } from 'react'
import { observer } from 'mobx-react-lite';
import { Segment, Item, Button, Pagination, PaginationProps } from 'semantic-ui-react'
import { StoresContext } from '../../../app/stores/stores';
import { IRecipe } from '../../../app/models/recipe';
import { RouteComponentProps } from 'react-router-dom';

const RecipeList: React.FC<RouteComponentProps> = ({ history }) => {

  const stores = useContext(StoresContext);
  const { pagedResult, pagedRequest, setPagedRequest, loadRecipes, selectRecipe, cancelSelectedRecipe, deleteRecipeAsync } = stores.recipeStore;

  useEffect(() => {
    loadRecipes();
  }, [loadRecipes]);

  const handlePaging = (event: React.MouseEvent<HTMLAnchorElement>, paginationProps: PaginationProps) => {
    setPagedRequest({ ...pagedRequest, currentPage: paginationProps.activePage ?? 1 });
    loadRecipes();
  };

  const onView = (recipe: IRecipe) => {
    selectRecipe(recipe);
    history.push(`/myRecipes/${recipe.slug}`)
  };

  const onAdd = () => {
    cancelSelectedRecipe();
    history.push(`/myRecipes/edit`);
  };

  const onDelete = (recipe: IRecipe) => {
    deleteRecipeAsync(recipe);
  };

  return (
    <Segment clearing>
      <Item.Group divided>
        <Item>
          <Item.Content></Item.Content>
          <Item.Image size='mini' style={{ marginRight: 10 }}>
            <Button circular basic icon='plus' floated='right' color='grey' onClick={onAdd} />
          </Item.Image>
        </Item>
        {pagedResult?.data.map(recipe => (
          <Item key={recipe.id}>
            <Item.Content>
              <Item.Header as='a' onClick={() => onView(recipe)}>{recipe.title}</Item.Header>
              <Item.Meta>{recipe.recipeCategory.name}</Item.Meta>
            </Item.Content>
            <Item.Image size='mini' style={{ marginRight: 10 }}>
              <Button circular basic icon='delete' floated='right' color='red' onClick={() => onDelete(recipe)} />
            </Item.Image>
          </Item>
        ))}
        <Item>
          <Item.Content></Item.Content>
          <Item.Image size='mini' style={{ marginRight: 10 }}>
            <Button circular basic icon='plus' floated='right' color='grey' onClick={onAdd} />
          </Item.Image>
        </Item>
      </Item.Group>
      <Pagination
        onPageChange={handlePaging}
        activePage={pagedResult.currentPage}
        firstItem={null}
        lastItem={null}
        pointing
        secondary
        totalPages={pagedResult.totalPages}
      />
    </Segment>

  );
}

export default observer(RecipeList);