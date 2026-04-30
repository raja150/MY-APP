import classNames from 'classnames';
import React from 'react';
import { FormGroup, Input } from 'reactstrap';
import Error from './Error';

const Input1 = (props) => {
     
    const { handlevaluechange, value, label, touched, error, className, name, type, dpchange,
        valueField, textField, showDate, showTime, ...rest } = props;
       
    const cssName = classNames(
        "form-control form-control-sm",
        {
            "is-invalid": touched && error
        },
        className
    );

    const handleChange = (e) => {
        if (props.type === 'decimal') {
            handlevaluechange(name, parseFloat(e.target.value || 0), {})
        } else if (props.type === 'number') {
            handlevaluechange(name, parseInt(e.target.value), {})
        } else {
            handlevaluechange(name, e.target.value, {})
        }
    }

    const handleFocus = (event) => {
        if (props.type === 'decimal' || props.type === 'number') {
            event.target.select();
        }
    }
    const vv = {
        ...rest, name, value, type: (props.type === 'decimal' ? 'number' : props.type)
        , onChange: handleChange
    };
    return (
        <FormGroup >
            <label className="text-capitalize" htmlFor={name}>{label}</label>
            <Input autoComplete='nope' className={cssName}  {...vv} onFocus={handleFocus} type={name.toLowerCase() == "password" ? "password" : type} />
            <Error touched={touched} error={error} />
            
        </FormGroup>
    );
};

export default Input1; 
