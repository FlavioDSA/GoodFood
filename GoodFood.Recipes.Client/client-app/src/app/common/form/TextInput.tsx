import React from 'react';
import { FormFieldProps, Form, Label } from 'semantic-ui-react';

interface IProps extends FormFieldProps {}

const TextInput: React.FC<IProps> = ({
  input,
  label,
  width,
  type,
  placeholder,
  meta: { touched, error }
}) => {
  return (
    <Form.Field error={touched && !!error} type={type} width={width}>
      <label>{label}</label>
      <input {...input} placeholder={placeholder} autoComplete='off' />
      {touched && error && (
        <Label basic color='red'>
          {error}
        </Label>
      )}
    </Form.Field>
  );
};

export default TextInput;
