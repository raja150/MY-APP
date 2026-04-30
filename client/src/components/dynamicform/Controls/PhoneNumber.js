import classNames from 'classnames';
import React from 'react';
import InputMask from 'react-input-mask';
import { FormGroup } from 'reactstrap';
import Error from './Error';
 
const PhoneNumber = (props) => {
    const { dpchange, handlevaluechange, value, label, touched, error, className, 
        name, valueField, textField, showDate, showTime, type, ...rest } = props; 
    const cssName = classNames(
        "form-control form-control-sm",
        {
            "is-invalid": touched && error
        },
        {
            "is-invalid": value && value.length < 10
        },
        className
    ); 

    return (
        <FormGroup >
            <label className="text-capitalize" htmlFor={props.name}>{label}</label>
            <InputMask className={cssName} mask="(9999) 999 999" alwaysShowMask
            name={name} value={value} {...rest}
            onChange={(e)=>handlevaluechange(name, e.target.value.replace(/\D/g, ''),{})} />
            <Error touched={touched} error={error} />
        </FormGroup>
    );
}; 
export default PhoneNumber;
