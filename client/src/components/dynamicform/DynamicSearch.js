import React, { useEffect, useState } from 'react';
import { Card, CardBody, Col, Row } from 'reactstrap';
import {
    AadharNumber, CheckBox, Input, PanNumber,
    PhoneNumber, PinCode, Radio, RWDatePicker, RWDropdownList, RWMultiSelect, SwitchInput, TextAreaInput, Upload, Weekdays
} from './Controls';

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
    PhoneNumber: PhoneNumber,
    AadharNumber: AadharNumber,
    PinCode: PinCode,
    PanNumber: PanNumber,
    Upload: Upload,
    Weekdays: Weekdays,
};

const DynamicSearch = (props) => {

    //Dropdowns values
    const [dpValues, setDpValues] = useState(props.lookUpData);
    const [json, setJson] = useState(props.json);

    useEffect(() => {
        setJson(props.json);
    }, [props.json])

    const handleValueChange = async (name, value) => {
        //if condition for update the search data 
        props.handleOnChange(name, value)
        props.setFieldValue(name, value)
    }

    const getComponents = (field) => {
        const Component = props.SearchPlus ? (field.showInSearch && fieldMap[field.type]) : fieldMap[field.type];
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
                touched={props.touched[field.name]}
                value={props.values[field.name] || ''}
                values={field.dataValues && field.dataValues.length > 0 ? field.dataValues : (fieldvals ? fieldvals : [])}
                valueField={field.addValues ? 'value' : (field.dataValues.length > 0 ? 'value' : 'id')}
                disabled={field.disabled || field.auto || false}
                handlevaluechange={handleValueChange}
                fieldcode={field.lookupCode} addoptions={fieldAdd ? 1 : 0}
                showDate={field.disableDate ? false : true}
                showTime={field.enableTime ? true : false}
                default={field.default}
                //valueAndEmpty={true}
            /> : ""
        );
    }

    const renderComponents = () => {
        const matrix = [[]];
        let i, k = 0, size = 0;
        //props.SearchPlus? (field.showInSearch &&fieldMap[field.type]):fieldMap[field.type]
        for (i = 0; i < json.length; i++) {

            if (json[i].showInSearch) {
                if ((json[i].hidden || false) === false) {
                    if ((size + parseInt(json[i].colSize)) > 12) {
                        k++;
                        matrix[k] = [];
                        size = 0;
                    }

                    matrix[k].push(json[i]);
                    size = size + parseInt(json[i].colSize);
                } else {
                    matrix[k].push(json[i]);
                }
            }
        }

        return matrix.map((row, r) => {
            return <Row form key={r}>
                {row.map((field, f) => {
                    if (field.hidden) {
                        return <input type="hidden" name={field.name} value={props.values[field.name] || ''} />
                    }
                    return <Col md={field.colSize} key={f}>
                        {getComponents(field)}
                    </Col>
                })}
            </Row>
        })

    }

    return (
        <Card key="formcard" className="main-card mb-2">
            <CardBody>
                {/* <CardTitle>{json.displayName}</CardTitle> */}
                {
                    renderComponents()
                }
            </CardBody>
        </Card>
    )
}

export default DynamicSearch;