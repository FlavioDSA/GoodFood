import React from 'react'
import { FormFieldProps, Form, Label, Select } from 'semantic-ui-react';

interface IProps extends FormFieldProps { }

const SelectInput: React.FC<IProps> = ({
  input,
  label,
  width,
  options,
  placeholder,
  meta: { touched, error }
}) => {
  return (
    <Form.Field error={touched && !!error} width={width}>
      <label>{label}</label>
      <Select
        value={input.value}
        onChange={(e, data) => input.onChange(data.value)}
        placeholder={placeholder}
        options={options}
      />
      {touched && error && (
        <Label basic color='red'>
          {error}
        </Label>
      )}
    </Form.Field>
  )
}

export default SelectInput
