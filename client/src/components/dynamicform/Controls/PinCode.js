import classNames from 'classnames';
import React from 'react';
import InputMask from 'react-input-mask';
import { FormGroup } from 'reactstrap';
import Error from './Error';

const PinCode = ( props ) => {
    const { dpchange, handlevaluechange, value, label, touched, error, className, 
        name, textField, valueField, showTime, showDate, type, ...rest } = props;
    const cssName = classNames(
        "form-control form-control-sm",
        {
            "is-invalid": touched && error
        },
        {
            "is-invalid": value && value.length < 6
        },
        className
    ); 
    const onChange = (event) => {
        handlevaluechange(name, event.target.value.replace(/\D/g, ''),{});
    } 
    return (
        <FormGroup >
            <label className="text-capitalize" htmlFor={props.name}>{label}</label>
            <InputMask className={cssName} mask="999 999" name={props.name} alwaysShowMask
                value={value} {...rest}
                onChange={onChange} />
            <Error touched={touched} error={error} />
        </FormGroup>
    );
};

export default PinCode;
