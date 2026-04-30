import classNames from 'classnames';
import React from 'react';
import { FormGroup, Input } from 'reactstrap';
import Error from './Error';

const TextAreaInput = (props) => {
    const { dpchange, handlevaluechange, value, label, touched, error, className, disabled, name, max, type, ...rest } = props;
    
    const cssClass = classNames(
        "form-control-sm",
        {
            "is-invalid": touched && error
        },
        className
    );

    return (
        <FormGroup >
            <label className="text-capitalize" htmlFor={props.name}>{label}</label>
            <Input className={cssClass} type="textarea" maxLength={max}
                value={value} disabled={disabled ? true : false}
                onChange={(e) => handlevaluechange(name, e.target.value, {})} />
            <Error touched={touched} error={error} />
        </FormGroup>
    );
};

export default TextAreaInput; 
