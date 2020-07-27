import React, { useContext, useEffect } from 'react';
import { Field } from 'react-final-form';
import { FieldArray } from 'react-final-form-arrays';
import { observer } from 'mobx-react-lite';
import { Segment, Form, Button, DropdownItemProps, Item, Header, Icon } from 'semantic-ui-react';
import { StoresContext } from '../../../app/stores/stores';
import { IRecipe } from '../../../app/models/recipe';
import TextInput from '../../../app/common/form/TextInput';
import SelectInput from '../../../app/common/form/SelectInput';
import TextAreaInput from '../../../app/common/form/TextAreaInput';
import { history } from '../../..';
import slugify from 'react-slugify';

interface IProps {
  handleSubmit: (event: any) => any;
  editingRecipe: IRecipe;
  isInvalid: boolean;
}

const RecipeForm: React.FC<IProps> = ({ editingRecipe, handleSubmit, isInvalid }) => {

  const stores = useContext(StoresContext);
  const { selectedRecipe, recipeCategories } = stores.recipeStore;

  const categoryOptions: DropdownItemProps[] = recipeCategories.map(x => {
    return { key: x.id, text: x.name, value: x.id }
  });

  const isNewRecipe = () => !selectedRecipe;

  useEffect(() => {
    if(isNewRecipe())
      editingRecipe.slug = slugify(editingRecipe.title);
  }, [editingRecipe.title, isNewRecipe]);

  useEffect(() => {
    if(isNewRecipe())
      editingRecipe.slug = slugify(editingRecipe.slug);
  }, [editingRecipe.slug, isNewRecipe]);

  const onCancel = () => {
    if (isNewRecipe()) {
      history.push(`/myRecipes`);
    } else {
      history.push(`/myRecipes/${selectedRecipe?.slug}`);
    }
  };

  return (
    <Segment clearing>
      <Form onSubmit={handleSubmit}>
        <Field label='Title' component={TextInput} name='title' placeholder='Title' autocomplete={'off'} />
        {isNewRecipe() && (<Field label='Slug' component={TextInput} name='slug' placeholder='Slug' autocomplete={'off'} />)}
        <Field label='Description' component={TextAreaInput} name='description' placeholder='Description' rows={2} />
        <Field label='Category' component={SelectInput} name='recipeCategoryId' placeholder='Category' options={categoryOptions} />
        <Segment clearing>
          <Header as='h4'>
            Ingredients
            {editingRecipe.recipeIngredients.length === 0 && (
              <Header.Subheader>
                <div style={{ padding: '3px' }}>
                  Please add at least one ingredient
                  <Icon name="arrow right" ></Icon>
                </div>
              </Header.Subheader>
            )}
          </Header>
          <Item.Group divided>
            <FieldArray name="recipeIngredients">
              {({ fields }) =>
                fields.map((recipeIngredient, i) => (
                  <Item key={i}>
                    <Item.Content>
                      <Item.Meta>{editingRecipe.recipeIngredients[i].ingredient.title}</Item.Meta>
                      <Field key={i} component={TextInput} name={`${recipeIngredient}.amount`} placeholder='Amount' />
                    </Item.Content>
                  </Item>
                ))
              }
            </FieldArray>
          </Item.Group>
        </Segment>
        <Button.Group widths={2}>
          <Button basic color='grey' type='button' content='Cancel' onClick={onCancel} />
          <Button basic color='teal' type='submit' content='Save' disabled={isInvalid || editingRecipe.recipeIngredients.length === 0} />
        </Button.Group>
      </Form>
    </Segment>
  );
};

export default observer(RecipeForm);