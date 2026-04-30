import React from 'react'
import { useField, useFormikContext } from 'formik'
import { FormGroup, Input } from 'reactstrap'
import Error from './Error'
import InputMask from 'react-input-mask';
import classNames from 'classnames';

const AadharNumber = (props) => {
    const { dpchange, handlevaluechange, value, label, touched, error, className,
        name, valueField, textField, showDate, showTime, ...rest } = props;
    const cssName = classNames(
        "form-control form-control-sm",
        {
            "is-invalid": touched && error
        },
        {
            "is-invalid": value && value.length < 12
        },
        className
    );

    return (
        <FormGroup >
            <label className="text-capitalize" htmlFor={name}>{label}</label>
            <InputMask className={cssName} mask="9999 9999 9999" name={name} alwaysShowMask {...rest} name={name} value={value}
                onChange={(e) => handlevaluechange(name, e.target.value.replace(/\D/g, ''), {})} />
            <Error touched={touched} error={error} />
        </FormGroup>
    );
};

export default AadharNumber;
