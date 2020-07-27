import React, { useContext, useEffect, useState } from 'react'
import { observer } from 'mobx-react-lite';
import { Grid, Container, Header, Icon } from 'semantic-ui-react';
import IngredientsList from './IngredientsList';
import IngredientDetails from '../details/IngredientDetails';
import IngredientForm from '../form/IngredientForm';
import { StoresContext } from '../../../app/stores/stores';
import PrivateRoute from '../../../app/layout/PrivateRoute';
import { SemanticWIDTHS } from 'semantic-ui-react/dist/commonjs/generic';

const IngredientsDashboard: React.FC = () => {

  const stores = useContext(StoresContext);
  const { editMode, selectedIngredient, cancelSelectedIngredient, cancelFormOpen } = stores.ingredientStore;

  var [listWidth, setListWidth] = useState<SemanticWIDTHS>('16');

  useEffect(() => {
    cancelSelectedIngredient();
    cancelFormOpen();
  }, [cancelSelectedIngredient, cancelFormOpen]);

  useEffect(() => {
    if (selectedIngredient)
      setListWidth('8');
    else
      setListWidth('16');
  }, [selectedIngredient]);

  return (
    <Container>
      <Grid>
        <Grid.Column width={16}>
          <Header as='h2'>
            <Icon name='food' />
            <Header.Content>
              My ingredients
            </Header.Content>
          </Header>
        </Grid.Column>
      </Grid>
      <Grid>
        <Grid.Column width={listWidth}>
          <IngredientsList></IngredientsList>
        </Grid.Column>
        <Grid.Column width={8}>
          {editMode && (
            <PrivateRoute path={`/myIngredients`} component={IngredientForm} />
          )}
          {!editMode && (
            <PrivateRoute exact path={`/myIngredients/:slug`} component={IngredientDetails} />
          )}
        </Grid.Column>
      </Grid>
    </Container>
  )
}

export default observer(IngredientsDashboard);