import React, { useState, FormEvent, useContext, useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Segment, Form, Button } from 'semantic-ui-react';
import { IIngredient } from '../../../app/models/ingredient';
import { StoresContext } from '../../../app/stores/stores';
import slugify from 'react-slugify';
import { RouteComponentProps } from 'react-router-dom';
import agent from '../../../app/api/agent';

const IngredientForm: React.FC<RouteComponentProps> = ({ history }) => {

  const stores = useContext(StoresContext);
  const { editIngredient, createIngredient, selectedIngredient, cancelFormOpen } = stores.ingredientStore;

  const [editingIngredient, setEditingIngredient] = useState<IIngredient>(
    {
      id: '',
      title: '',
      description: '',
      slug: ''
    }
  );

  useEffect(() => {
    if (selectedIngredient?.slug) {
      agent.Ingredient.getAsync(selectedIngredient.slug)
        .then(ingredient => setEditingIngredient(ingredient));
    } else {
      setEditingIngredient({
        id: '',
        title: '',
        description: '',
        slug: ''
      })
    }
  }, [selectedIngredient]);
  
  const isNewIngredient = () => !editingIngredient.id;

  const onSubmit = () => {
    if (isNewIngredient()) {
      createIngredient(editingIngredient);
      history.push(`/myIngredients/${editingIngredient.slug}`);
    } else {
      editIngredient(editingIngredient);
    }
  };

  const handleInputChange = (event: FormEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = event.currentTarget;

    if(isNewIngredient() && (name === 'title' || name === 'slug'))
      setEditingIngredient({ ...editingIngredient, [name]: value, slug: slugify(value) });
    else 
      setEditingIngredient({ ...editingIngredient, [name]: value });
  };

  return (
    <Segment clearing>
      <Form onSubmit={onSubmit}>
        <Form.Input
          label='Title'
          onChange={handleInputChange}
          name='title'
          placeholder='Title'
          autoComplete='Off'
          value={editingIngredient.title}
        />
        {isNewIngredient() && (
          <Form.Input
            label='Slug'
            onChange={handleInputChange}
            name='slug'
            placeholder='Slug'
            autoComplete='Off'
            value={editingIngredient.slug}
          />
        )}
        <Form.TextArea
          label='Description'
          onChange={handleInputChange}
          name='description'
          rows={2}
          placeholder='Description'
          value={editingIngredient.description}
        />
        <Button.Group widths={2}>
          <Button basic color='grey' content='Cancel' onClick={() => cancelFormOpen()} />
          <Button basic color='teal' type='submit' content='Save' />
        </Button.Group>
      </Form>
    </Segment>
  );
};

export default observer(IngredientForm);