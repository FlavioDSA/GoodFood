import React, { useEffect, useState } from 'react'
import { observer } from 'mobx-react-lite';
import { Segment, Item, Button, Pagination, PaginationProps } from 'semantic-ui-react';
import { IIngredient } from '../../../app/models/ingredient';
import { IRecipeIngredient } from '../../../app/models/recipeIngredient';
import { IRecipe } from '../../../app/models/recipe';
import { IPagedRequest, IPagedResult } from '../../../app/models/paging/pagedRequest';
import agent from '../../../app/api/agent';

interface IProps {
  addIngredient: (recipeIngredient: IRecipeIngredient) => void;
  removeIngredient: (ingredient: IIngredient) => void;
  editingRecipe: IRecipe;
}

const RecipeIngredientSelector: React.FC<IProps> = ({ editingRecipe, addIngredient, removeIngredient }) => {

  const [pagedRequest, setPagedRequest] = useState<IPagedRequest>(
    { currentPage: 1, pageSize: 5 }
  );

  const [pagedResult, setPagedResult] = useState<IPagedResult<IIngredient>>(
    { currentPage: 1, pageSize: 5, totalItems: 0, totalPages: 0, data: [] }
  );

  const loadIngredients = async () => {
    let pagedResult = await agent.Ingredient.listAsync(pagedRequest);
    setPagedResult(pagedResult);
  };

  useEffect(() => {
    loadIngredients();
  }, [pagedRequest]);

  const handlePaging = (event: React.MouseEvent<HTMLAnchorElement>, paginationProps: PaginationProps) => {
    setPagedRequest({ ...pagedRequest, currentPage: paginationProps.activePage ?? 1 });
  };

  const onAddIngredient = (ingredient: IIngredient) => {
    if (editingRecipe.recipeIngredients.find(x => x.ingredient.id === ingredient.id))
      return;

    let recipeIngredient: IRecipeIngredient = {
      id: '',
      amount: '',
      ingredientId: ingredient.id,
      ingredient: ingredient
    };

    addIngredient(recipeIngredient);
  };

  const onRemoveIngredient = (ingredient: IIngredient) => {
    if (!editingRecipe.recipeIngredients.find(x => x.ingredient.id === ingredient.id))
      return;

    removeIngredient(ingredient);
  };

  return (
    <Segment clearing>
      <Item.Group divided>
        <Item>
          <Item.Meta content='Select ingredients' />
        </Item>
        {pagedResult?.data.map(ingredient => (
          <Item key={ingredient.id}>
            <Item.Content>
              <Item.Header>
                {ingredient.title}
              </Item.Header>
              {editingRecipe.recipeIngredients.find(x => x.ingredient.id === ingredient.id) && (
                <Button basic circular negative icon='remove' floated='right' size='small' onClick={() => onRemoveIngredient(ingredient)} />
              )}
              {!editingRecipe.recipeIngredients.find(x => x.ingredient.id === ingredient.id) && (
                <Button basic circular positive icon='plus' floated='right' size='small' onClick={() => onAddIngredient(ingredient)} />
              )}
              
            </Item.Content>
          </Item>
        ))}
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
  )
}

export default observer(RecipeIngredientSelector);