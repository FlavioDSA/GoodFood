import React, { useContext, useEffect } from 'react'
import { observer } from 'mobx-react-lite';
import { Segment, Item, Button, Pagination, PaginationProps } from 'semantic-ui-react'
import { StoresContext } from '../../../app/stores/stores';
import { history } from '../../../index';
import { IIngredient } from '../../../app/models/ingredient';

const IngredientsList: React.FC = () => {
  
  const stores = useContext(StoresContext);
  const {pagedResult, pagedRequest, setPagedRequest, loadIngredients, openCreateForm, deleteIngredientAsync, cancelFormOpen } = stores.ingredientStore;

  useEffect(() => {
    loadIngredients();
  }, [loadIngredients]);

  const handlePaging = (event: React.MouseEvent<HTMLAnchorElement>, paginationProps: PaginationProps) => { 
    setPagedRequest({ ...pagedRequest, currentPage: paginationProps.activePage ?? 1 });
    loadIngredients();
  };

  const onDelete = (ingredient: IIngredient) => {
    deleteIngredientAsync(ingredient);
  };

  const onAdd = () => {
    openCreateForm();
    history.push('/myIngredients');
  };

  const onSelect = (ingredient: IIngredient) => {
    cancelFormOpen();
    history.push(`/myingredients/${ingredient.slug}`)
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
        {pagedResult?.data.map(ingredient => (
          <Item key={ingredient.id}>
            <Item.Content>
              <Item.Header as='a' onClick={() => onSelect(ingredient)}>{ingredient.title}</Item.Header>
              <Item.Meta>{ingredient.description}</Item.Meta>
            </Item.Content>
            <Item.Image size='mini' style={{ marginRight: 10 }}>
              <Button circular basic icon='delete' floated='right' color='red' onClick={() => onDelete(ingredient)} />
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
        defaultActivePage={1}
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

export default observer(IngredientsList);