import React, { useContext, useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Card, Button } from 'semantic-ui-react';
import { StoresContext } from '../../../app/stores/stores';
import { RouteComponentProps } from 'react-router-dom';
import agent from '../../../app/api/agent';

interface RouteParams {
  slug: string;
}

const IngredientDetails: React.FC<RouteComponentProps<RouteParams>> = ({ match, history }) => {

  const stores = useContext(StoresContext);
  let { selectedIngredient, selectIngredient, cancelSelectedIngredient, openEditForm } = stores.ingredientStore;

  useEffect(() => {
    if (match.params?.slug) { 
      agent.Ingredient.getAsync(match.params.slug)
        .then(ingredient => selectIngredient(ingredient));
    }
  }, [match.params, match.params.slug, selectIngredient]);

  const onCancel = () => {
    cancelSelectedIngredient();
    history.push('/myIngredients')
  };

  return (
    <Card fluid>
      <Card.Content>
        <Card.Header>{selectedIngredient && (selectedIngredient.title)}</Card.Header>
        <Card.Description>
          {selectedIngredient && (selectedIngredient.description)}
        </Card.Description>
      </Card.Content>
      <Card.Content extra>
        <Button.Group widths={2}>
          <Button basic color='grey' content='Close' onClick={onCancel} />
          <Button basic color='teal' content='Edit' onClick={openEditForm} />
        </Button.Group>
      </Card.Content>
    </Card>
  );
};

export default observer(IngredientDetails);