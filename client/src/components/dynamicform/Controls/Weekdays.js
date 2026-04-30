import classNames from 'classnames';
import React, { useState } from 'react';
import { Multiselect } from 'react-widgets'; //https://jquense.github.io/react-widgets/api/DropdownList/#value
import { FormGroup } from 'reactstrap';

const Weekdays = (props) => {
    const { dpchange, values, value, label, touched, error,
        className, name, type, showDate, showTime, ...rest } = props;
    const dataVls = [{ value: 1, text: 'Su' }, { value: 2, text: 'Mo' }, { value: 3, text: 'Tu' }, { value: 4, text: 'We' },
    { value: 5, text: 'Th' }, { value: 6, text: 'Fr' }, { value: 7, text: 'Sa' }];

    const cssClass = classNames(
        "form-control form-control-sm p-0",
        {
            "is-invalid": touched && error
        },
        className
    );

    const [isBusy, setIsBusy] = useState(false);
    //from API convert (ex 1,2) into int array
    let fieldVal = [];
    
    if (value && value.length > 0) {
        //Convert from string array to int array
        fieldVal = value.split(',').map(function (item) {
            return (!isNaN(item) ? parseInt(item) : item);
        });
    }

    return (
        <FormGroup>
            <label className="text-capitalize" htmlFor={props.name}>{label}</label>
            {/* https://github.com/jquense/react-widgets/issues/848 */}
            <Multiselect busy={isBusy}
                allowCreate={props.addoptions === 1 ? "onFilter" : false} filter={true} 
                value={value === "" ? [] : fieldVal}
                data={dataVls} className={cssClass}
                valueField='value'
                textField='text'
                placeholder={'Select a ' + label}
                onChange={async (selValue) => {
                    const val = [];
                    setIsBusy(true);
                    selValue.map((op, k) =>
                        val.push(op.value)
                    )
                    
                     await props.handlevaluechange(props.name, val.join(), { isCasecade: false }); 
                     setIsBusy(false);
                }}
                id={props.name} name={props.name}
            />
            {/* <Multiselect
                allowCreate={props.addoptions === 1 ? "onFilter" : false} filter={true} 
                data={dataVls}
                textField='text'

            /> */}
            {/* <Error touched={meta.touched} error={meta.error} /> */}
            {touched && error ?
                <div className="invalid-feedback">
                    {error}
                </div>
                : ''}
        </FormGroup>
    );
};


export default Weekdays;