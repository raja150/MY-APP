import { AadharNumber, CheckBox, Input, PanNumber, PhoneNumber, PinCode, Radio, RWDatePicker, RWDropdownList, RWMultiSelect, SwitchInput, TextAreaInput, Upload, Weekdays } from 'components/dynamicform/Controls';
import React, { Fragment, useEffect, useState } from 'react';
import { Col, Row } from 'reactstrap';
import * as formUtil from '../../utils/form';
import AsyncTypeaheadR from './AsyncTypeheadR';

const fieldMap = {
    Input: Input,
    NumberInput: Input,
    Textarea: TextAreaInput,
    CheckBox: CheckBox,
    Radio: Radio,
    SwitchInput: SwitchInput,
    RWDropdownList: RWDropdownList,
    RWMultiSelect: RWMultiSelect,
    RWDatePicker: RWDatePicker,
    TypeAhead: AsyncTypeaheadR,
    PhoneNumber: PhoneNumber,
    AadharNumber: AadharNumber,
    PinCode: PinCode,
    PanNumber: PanNumber,
    Upload: Upload,
    Weekdays: Weekdays,
}

const JSONData = {
    fields: [
        {
            addValues: false,
            auto: false,
            colSize: 6,
            decimals: false,
            disableDate: false,
            disabled: false,
            displayOrder: 1,
            enableTime: false,
            hidden: false,
            isReference: false,
            jsType: "number",
            label: "Applied To",
            name: "appliedTo",
            propName: "AppliedTo",
            required: true,
            showInList: true,
            showInSearch: false,
            type: "Input"
        },
        {
            addValues: false,
            auto: false,
            colSize: 6,
            decimals: false,
            disableDate: false,
            disabled: false,
            displayOrder: 1,
            enableTime: false,
            hidden: false,
            isReference: false,
            jsType: "number",
            label: "Applied To",
            name: "appliedTo",
            propName: "AppliedTo",
            required: true,
            showInList: true,
            showInSearch: false,
            type: "RWDropdownList"
        },
        {
            addValues: false,
            auto: false,
            colSize: 6,
            decimals: false,
            disableDate: false,
            disabled: false,
            displayOrder: 5,
            enableTime: false,
            hidden: false,
            isReference: false,
            jsType: "date",
            label: "From Date",
            name: "fromDate",
            propName: "FromDate",
            required: true,
            type: "RWDatePicker"
        },
        {
            addValues: false,
            auto: false,
            colSize: 6,
            decimals: false,
            disableDate: false,
            disabled: false,
            displayOrder: 4,
            enableTime: false,
            hidden: false,
            isReference: false,
            jsType: "number",
            label: "Typeahead",
            name: "typeHead",
            propName: "TypeHead",
            required: true,
            showInList: true,
            showInSearch: false,
            url : 'Organization/Department/GetList',
            type: "TypeAhead"
        },
    ]
}

const SearchComponent = (props) => {
    const [json, setJson] = useState(props.JSONData);
    const [dpValues, setDpValues] = useState(props.lookUpData);

    useEffect(() => {
        const fetchData = async () => {
            let formData = {};
            const iniValues = formUtil.getInitialValues(formData, json.fields);
        }
        fetchData();
    }, []);

    const handleValueChange = async (name, value, { option, isCasecade }) => {
        //Add create option into dropdowns
        props.setFieldValue(name, value);
    }

    const getComponents = (field) => {
        const Component = fieldMap[field.type];
        let fieldvals = [];
        const fieldAdd = field.addValues ? field.addValues : false;

        //Bind dropdown and multiselect dropdown data
        if (dpValues) {
            if (field) {
                if (dpValues[field.name] && dpValues[field.name].data) {
                    fieldvals = [...dpValues[field.name].data];
                }
            }
        }

        return (
            Component ? <Component
                key={field.name}
                label={field.label} textField={field.textField}
                name={field.name}
                type={field.type}
                placeholder={field.placeholder ? field.placeholder : ''}
                error={props.errors[field.name]}
                touched={props.touched[field.name]}
                value={props.values[field.name] || ''}
                values={field.dataValues ? field.dataValues : (fieldvals ? fieldvals : [])}
                valueField={field.addValues ? 'value' : (field.dataValues ? 'value' : 'id')}
                disabled={field.disabled || field.auto || false} handlevaluechange={handleValueChange}
                fieldcode={field.lookupCode} addoptions={fieldAdd ? 1 : 0} showDate={field.disableDate ? false : true}
                showTime={field.enableTime ? true : false}
                url={field.url}
            /> : ""
        );
    }

    const renderComponents = () => {
        const matrix = [[]];
        let i, k = 0, size = 0;
        for (i = 0; i < json.fields.length; i++) {
            if (json.fields[i].conditional) {
                const { field, hasValue, required, display } = json.fields[i].conditional;
                if (String(props.values[field]) === String(hasValue)) {
                    json.fields[i]['hidden'] = !display; //hidden is Opp to [This Field Should Display]
                    json.fields[i]['required'] = required;
                } else {
                    json.fields[i]['hidden'] = display
                }
            }
            if ((json.fields[i].hidden || false) === false) {
                if ((size + parseInt(json.fields[i].colSize)) > 12) {
                    k++;
                    matrix[k] = [];
                    size = 0;
                }

                matrix[k].push(json.fields[i]);
                size = size + parseInt(json.fields[i].colSize);
            }
        }

        return matrix.map((row, r) => {
            return <Row form key={r}>
                {row.map((field, f) => {
                    return <Col md={field.colSize} key={f}>
                        {getComponents(field)}
                    </Col>
                })}
            </Row>
        })

    }

    return (
        <Fragment>
            {
                renderComponents()
            }
        </Fragment>
    )
}

export default SearchComponent