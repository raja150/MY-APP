import classNames from 'classnames';
import React, { useState } from 'react';
import { Multiselect } from 'react-widgets'; //https://jquense.github.io/react-widgets/api/DropdownList/#value
import { FormGroup } from 'reactstrap';
import APIService from '../../../services/apiservice';
import camelcase from 'camelcase';

const RWMultiSelect = (props) => {

    const { dpchange, handlevaluechange, values, value, label, touched, error,
        className, name, type, showDate, showTime, ...rest } = props;
    const valueField = props.valueField ? props.valueField : 'value';
    const textField = props.textField ? camelcase(props.textField) : 'text';

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
    const handleCreate = async (name) => {
        setIsBusy(true);
        //Call API
        const listresult = await APIService.postAsync(`LookupValues`, { code: props.fieldcode, text: name });

        //Update dropdown data
        if (listresult.data) {
            fieldVal.push(listresult.data.value);
            const optValue = { text: listresult.data.text, value: listresult.data.value };
            await handlevaluechange(props.name, fieldVal.join(), { option: optValue });
        }
        setIsBusy(false);
    };

    return (
        <FormGroup>
            <label className="text-capitalize" htmlFor={props.name}>{label}</label>
            {/* https://github.com/jquense/react-widgets/issues/848 */}
            <Multiselect busy={isBusy}
                name={props.name}
                allowCreate={props.addoptions === 1 ? "onFilter" : false} filter={true}
                onCreate={name => handleCreate(name)}
                value={value == "" ? [] : fieldVal}
                data={values} className={cssClass}
                valueField={valueField}
                textField={textField}
                placeholder={'Select a ' + label}
                onChange={async (selValue) => {
                    const val = [];
                    setIsBusy(true);
                    selValue.map((op, k) =>
                        val.push(op[valueField])
                    )
                    await handlevaluechange(props.name, val.join(), { isCasecade: true });
                    setIsBusy(false);
                }}
                id={props.name} name={props.name}
            />
            {/* <Error touched={meta.touched} error={meta.error} /> */}
            {touched && error ?
                <div className="invalid-feedback">
                    {error}
                </div>
                : ''}
        </FormGroup>
    );
};


export default RWMultiSelect;