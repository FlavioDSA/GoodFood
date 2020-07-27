import React from 'react'
import { observer } from 'mobx-react-lite';
import { Grid, Header, Icon } from 'semantic-ui-react';

const Home: React.FC = () => {

  return (
    <Grid>
      <Grid.Column width={16}>
        <Header as='h2'>
          <Icon name='food' />
          <Header.Content>
            GoodFood
          <Header.Subheader>Recipes</Header.Subheader>
          </Header.Content>
        </Header>
      </Grid.Column>
    </Grid>
  )
}

export default observer(Home);