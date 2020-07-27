import React from 'react'
import { observer } from 'mobx-react-lite';
import { Grid, Container, Header, Icon } from 'semantic-ui-react';
import { Switch } from 'react-router-dom';
import RecipeList from './RecipeList';
import RecipeDetails from '../details/RecipeDetails';
import RecipeEditDashboard from '../edit/RecipeEditDashboard';
import PrivateRoute from '../../../app/layout/PrivateRoute';

const RecipesDashboard: React.FC = () => {
  return (
    <Container>
      <Grid>
        <Grid.Column width={16}>
          <Header as='h2'>
            <Icon name='food' />
            <Header.Content>
              My recipes
            </Header.Content>
          </Header>
        </Grid.Column>
      </Grid>
      <Grid>
        <Grid.Column width='16'>
          <Switch>
            <PrivateRoute exact path='/myRecipes' component={RecipeList} />
            <PrivateRoute exact path='/myRecipes/edit' component={RecipeEditDashboard} />
            <PrivateRoute exact path='/myRecipes/:slug' component={RecipeDetails} />
            <PrivateRoute exact path='/myRecipes/edit/:slug' component={RecipeEditDashboard} />
          </Switch>
        </Grid.Column>
      </Grid>
    </Container>

  )
}

export default observer(RecipesDashboard);