import { useField, useFormikContext } from 'formik';
import React from 'react';
import { Typeahead } from 'react-bootstrap-typeahead';
import { FormGroup } from 'reactstrap';
import Error from './Error';
// import FormFeedback from 'reactstrap/lib/FormFeedback'
// https://jaredpalmer.com/formik/docs/guides/arrays

const TypeHead = ({ label, ...props }) => {
    // useField() returns [formik.getFieldProps(), formik.getFieldMeta()]
    // which we can spread on <input> and also replace ErrorMessage entirely.
    const [field, meta] = useField(props);
    const { setFieldValue } = useFormikContext();
    const errClass = meta.touched && meta.error ? 'is-invalid' : meta.touched ? 'is-valid' : '';
    const className = `form-control-sm ${errClass}`;
    return (
        <FormGroup >
            <label className="text-capitalize" htmlFor={props.name}>{label}</label>

            <Typeahead className={className}
                name={props.name}
                onChange={(e) => { 
                    if (e.length > 0) {
                        setFieldValue(props.name, e[0][props.selId]);
                    }
                }}
                labelKey={option => `${option.name}`}
                options={props.options ? props.options : [{}]}
                id={props.id}
            />
            <Error touched={meta.touched} error={meta.error} />
        </FormGroup>
    );
};

export default TypeHead;
