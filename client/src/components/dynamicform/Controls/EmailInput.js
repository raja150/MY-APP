import classNames from 'classnames';
import React from 'react';
import { FormGroup, Input } from 'reactstrap';
import Error from './Error';

const EmailInput = (props) => {
    const { dpchange, handlevaluechange, value, label, touched, error, className, name, ...rest } = props;
    const cssName = classNames(
        "form-control form-control-sm",
        {
            "is-invalid": touched && error
        },
        className
    );

    return (
        <FormGroup >
            <label className="text-capitalize" htmlFor={name}>{label}</label>
            <Input className={cssName} type='email' 
                value={value} {...rest}
                onChange={(e) => handlevaluechange(name, e.target.value,{})} />
            <Error touched={touched} error={error} />
        </FormGroup>
    );
};

export default EmailInput; 
