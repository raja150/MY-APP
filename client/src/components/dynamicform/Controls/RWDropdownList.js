import classNames from 'classnames';
import React, { useState } from 'react';
import { DropdownList } from 'react-widgets'; //https://jquense.github.io/react-widgets/api/DropdownList/#value
import { FormGroup } from 'reactstrap';
import APIService from '../../../services/apiservice';
import camelcase from 'camelcase';

const RWDropdownList = (props) => { 
    const { handlevaluechange, values, value, label, touched, error,
        className, name, showDate, showTime, disabled, index, ...rest } = props;
    const valueField = props.valueField ? props.valueField : 'value';
  
    const textField = props.textField ? camelcase(props.textField) : 'text';

    const cssClass = classNames(
        "form-control p-0 form-control-sm",
        {
            "is-invalid": touched && error
        },
        className
    );

    const [isBusy, setIsBusy] = useState(false);
    const handleCreate = async (name) => {
        setIsBusy(true);
        //Call API
        const listresult = await APIService.postAsync(`LookupValues`, { code: props.fieldcode, text: name });
        //Update dropdown data
        if (listresult.data) {
            const optValue = { text: listresult.data.text, value: listresult.data.value };
            await handlevaluechange(props.name, parseInt(listresult.data.value), { option: optValue })
        }
        setIsBusy(false);
    };

    const onSelectionClear = async () => {
        await handlevaluechange(props.name, '', {})
    }

    const WithClearableSelection = (item, onClear) => {
        if (disabled) {
            return (
                <React.Fragment>
                    <span >{item.item[textField]}</span>
                </React.Fragment>
            )
        }
        return (
            <React.Fragment>
                <span >{item.item[textField]}</span>
                <span><i onClick={onClear} className="pe-7s-close btnclear font-weight-bolder"> </i></span>
            </React.Fragment>
        );
    }
    return (
        <FormGroup>
            {label ? <label className="text-capitalize" htmlFor={props.name}>{label}</label> : ''}
            {/* https://github.com/jquense/react-widgets/issues/848 */}
            <DropdownList busy={isBusy} disabled={disabled}
                allowCreate={props.addoptions === 1 ? "onFilter" : false} filter='contains'
                onCreate={name => handleCreate(name)}
                value={value || value != "" ? (!isNaN(value) ? parseInt(value) : value) : ""}
                data={values} className={cssClass}
                valueField={valueField}
                textField={textField}
                placeholder={label ? 'Select a ' + label : 'Select'}
                onChange={async (value) => {
                    //https://codesandbox.io/s/328038x19q
                    // https://github.com/jaredpalmer/formik/issues/86 
                    setIsBusy(true);
                    await props.handlevaluechange(props.name, value[valueField], { isCasecade: true, selected: value, index : index });
                    setIsBusy(false);
                }}
                id={props.name} name={props.name}
                valueComponent={(item) => WithClearableSelection(item, onSelectionClear)}
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


export default RWDropdownList;