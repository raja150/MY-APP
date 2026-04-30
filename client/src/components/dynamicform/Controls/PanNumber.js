import classNames from 'classnames'
import React from 'react'
import { FormGroup, Input } from 'reactstrap'
import Error from './Error'
// import FormFeedback from 'reactstrap/lib/FormFeedback'
// https://jaredpalmer.com/formik/docs/guides/arrays

const PanNumber = (props) => {
    const { dpchange, handlevaluechange, value, label, touched, error, className, 
        name, type, valueField, textField, showDate, showTime, ...rest } = props;
    const cssName = classNames(
        "form-control form-control-sm",
        {
            "is-invalid": touched && error
        },
        // {
        //     "is-invalid": value && value.length < 10
        // },
        className
    );

    return (
        <FormGroup>
            <label className="text-capitalize" htmlFor={name}>{label}</label>
            <Input className={cssName} type={type}
                name={name} value={value} {...rest}
                onChange={(e) => handlevaluechange(name, e.target.value, {})} />
            <Error touched={touched} error={error} />
        </FormGroup>

    );
};

export default PanNumber;
