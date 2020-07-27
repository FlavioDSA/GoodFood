import React, { useContext, useState, useEffect } from 'react';
import { Form as FinalForm, Field } from 'react-final-form';
import { Form, Button, Header, Message } from 'semantic-ui-react';
import TextInput from '../../app/common/form/TextInput';
import { StoresContext } from '../../app/stores/stores';
import { IUserFormValues } from '../../app/models/user';
import { combineValidators, isRequired } from 'revalidate';
import { RouteComponentProps } from 'react-router-dom';

const validate = combineValidators({
  email: isRequired('Email'),
  password: isRequired('Password')
});

const LoginForm: React.FC<RouteComponentProps> = ({ history }) => {
  const stores = useContext(StoresContext);
  const [authenticationError, setAuthenticationError] = useState(false);

  useEffect(() => {
    if (stores.userStore.isLoggedIn)
      history.push('/');
  }, [history, stores.userStore.isLoggedIn]);

  return (
    <FinalForm
      onSubmit={(values: IUserFormValues) =>
        stores.userStore.loginAsync(values)
          .then(() => history.push('/myRecipes'))
          .catch(error => setAuthenticationError(true))
      }
      validate={validate}
      render={({
        handleSubmit,
        invalid,
        dirtySinceLastSubmit
      }) => (
          <Form onSubmit={handleSubmit} error>
            <Header as='h2' content='Login to GoodFood' color='teal' textAlign='center' />
            <Field component={TextInput} name='email' placeholder='Email' />
            <Field component={TextInput} name='password' placeholder='Password' type='password' />

            {authenticationError && !dirtySinceLastSubmit && (
              <Message error>Invalid email or password</Message>
            )}

            <Button disabled={invalid} type='submit' color='teal' content='Login' fluid />
          </Form>
        )}
    />
  );
};

export default LoginForm;
