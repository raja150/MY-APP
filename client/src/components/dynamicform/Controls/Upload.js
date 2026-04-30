import classNames from 'classnames';
import React from 'react';
import { FormFeedback, FormGroup, Input } from 'reactstrap'; 

const Upload = (props) => {
    const { dpchange, handlevaluechange, value, label, touched, error, className, name, max, type, ...rest } = props;
    const cssClass = classNames(
        "form-control-sm",
        {
            "is-invalid": touched && error
        },
        className
    );

    const onChange = (event) => {
        handlevaluechange(name, event.currentTarget.files[0],{});
    }
    return (
        <FormGroup >
            <label className="text-capitalize" htmlFor={props.name}>{label}</label> 
            <Input className={cssClass} type='file' onChange={onChange} />
            <FormFeedback>
                {error}
            </FormFeedback>
        </FormGroup>
    );
};

export default Upload; 
