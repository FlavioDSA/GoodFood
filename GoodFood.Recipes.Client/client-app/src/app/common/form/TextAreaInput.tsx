import React from 'react';
import { FormFieldProps, Form, Label } from 'semantic-ui-react';

interface IProps extends FormFieldProps {}

const TextAreaInput: React.FC<IProps> = ({
  input,
  label,
  width,
  rows,
  placeholder,
  meta: { touched, error }
}) => {
  return (
    <Form.Field error={touched && !!error} width={width}>
      <label>{label}</label>
      <textarea rows={rows} {...input} placeholder={placeholder} />
      {touched && error && (
        <Label basic color='red'>
          {error}
        </Label>
      )}
    </Form.Field>
  );
};

export default TextAreaInput;
