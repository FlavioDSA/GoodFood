import React, { useContext } from 'react';
import { Menu, Container, Dropdown, Image, Icon } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';
import { StoresContext } from '../stores/stores';
import { NavLink, Link } from 'react-router-dom';

const NavBar: React.FC = () => {

  const stores = useContext(StoresContext);
  const { user, logout } = stores.userStore;

  return (
    <Menu fixed='top' inverted>
      <Container>
        <Menu.Item header as={NavLink}  exact to='/'>
          <Icon name='food' />
          GoodFood
        </Menu.Item>
        {user && (<Menu.Item name='My Recipes' as={NavLink} to='/myRecipes' />)}
        {user && (<Menu.Item name='My Ingredients' as={NavLink} to='/myIngredients' />)}
        {user && (
          <Menu.Item position='right'>
            <Image avatar spaced='right' src={user.image || '/assets/user.png'} />
            <Dropdown pointing='top left' text={user.displayName}>
              <Dropdown.Menu>
                <Dropdown.Item
                  as={Link}
                  to={`/myRecipes`}
                  text='My recipes'
                  icon='user'
                />
                <Dropdown.Item
                  as={Link}
                  to={`/myIngredients`}
                  text='My ingredients'
                  icon='user'
                />
                <Dropdown.Item onClick={logout} text='Logout' icon='power' />
              </Dropdown.Menu>
            </Dropdown>
          </Menu.Item>
        )}
        {!user && (<Menu.Item  position='right' name='Log in' as={NavLink} exact to='/login' />)}
      </Container>
    </Menu>
  );
};

export default observer(NavBar);
