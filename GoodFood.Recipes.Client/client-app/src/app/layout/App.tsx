import React, { Fragment } from 'react';
import { Container } from 'semantic-ui-react';
import { Route, Switch } from 'react-router-dom';
import NavBar from './NavBar';
import RecipesDashboard from '../../features/recipes/dashboard/RecipesDashboard';
import IngredientsDashboard from '../../features/ingredients/dashboard/IngredientsDashboard';
import PrivateRoute from './PrivateRoute';
import LoginForm from '../../features/user/LoginForm';
import Home from './Home';
import { ToastContainer } from 'react-toastify';
import { observer } from 'mobx-react-lite';


const App = () => {
  return (
    <Fragment>
      <ToastContainer position='bottom-right' />
      <NavBar />
      <Container style={{ marginTop: '7em' }}>
        <Switch>
          <Route exact path='/' component={Home} />
          <Route exact path='/login' component={LoginForm} />
          <PrivateRoute path='/myIngredients' component={IngredientsDashboard} />
          <PrivateRoute path='/myRecipes' component={RecipesDashboard} />
        </Switch>
      </Container>
    </Fragment>
  );
}

export default observer(App);
