import React, { useState, useEffect } from 'react'
// import PropTypes from 'prop-types'
import { useField, useFormikContext } from 'formik'
// import _startCase from 'lodash/startCase'
// import moment from 'moment'
import Error from './Error'
import classNames from 'classnames'
import {
    CustomInput, FormGroup, Label
} from 'reactstrap';

// import FormFeedback from 'reactstrap/lib/FormFeedback'
// https://jaredpalmer.com/formik/docs/guides/arrays

const Radio = (props) => {
    const { dpchange, handlevaluechange, value, label, touched, error, className, 
        name, type, index, valueField, textField, showDate, showTime, ...rest } = props;

    const divClasses = classNames(
        {
            "is-invalid": touched && error
        },
        className
    );

    return (
        <FormGroup>
            <Label className="text-capitalize">{label}</Label>
            <div className={divClasses}>
                {props.values && props.values.map((op, k) => {
                    return <CustomInput key={props.name + k} invalid={touched && error ? true : false}
                        type="radio" value={op.value} id={props.name + k} name={props.name}
                        {...rest}
                        checked={value == op.value ? true : false}
                        label={op.text} inline
                        onChange={(event) => handlevaluechange(name, event.target.value, {index})}
                    />
                })} 
            </div>
            <Error touched={touched} error={error} />
        </FormGroup>
    );
};

export default Radio;
