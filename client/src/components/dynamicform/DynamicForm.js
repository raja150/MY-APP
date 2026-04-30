import React, { useEffect, useState } from 'react';
import { Card, CardBody, CardTitle, Col, Row } from 'reactstrap';
import * as formUtil from '../../utils/form';
import { AadharNumber, CheckBox, EmailInput, Weekdays, Input, SwitchInput, PanNumber, 
    PhoneNumber, PinCode, Radio, RWDatePicker, RWDropdownList, RWMultiSelect, TextAreaInput, Upload } from './Controls';
import vm from 'vm';

import Moment from 'moment';
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
    // TypeAhead: TypeAhead,
    PhoneNumber: PhoneNumber,
    AadharNumber: AadharNumber,
    PinCode: PinCode,
    PanNumber: PanNumber,
    Upload: Upload,
    Weekdays: Weekdays,
    // Label:Label,
    // Reference:Reference,
};

const DynamicForm = (props) => {
    //Dropdowns values
    const [dpValues, setDpValues] = useState(props.lookUpData);
    const [json, setJson] = useState(props.json);
    useEffect(() => {
        setJson(props.json);
    }, [props.json]) //When props updated by parent component

    const dpChange = async (name, value) => {
        const fields = json.fields.filter(v => v.dependent == name);
        if (fields.length > 0) {
            const ddData = await formUtil.getCaseCadeFieldData(fields, value);
            const dd = dpValues;
            fields.map((item, k) => {
                if (dd[item.name]) {
                    dd[item.name].data = ddData[item.name].data;
                } else {
                    dd[item.name] = ddData[item.name];
                }
            })
            setDpValues(dd)
        }
        return fields;
    }

    const handleValueChange = async (name, value, { option, isCasecade }) => { 
        //Add create option into dropdowns
        if (option) {
            const dd = dpValues;
            if (dd[name]) {
                dd[name].data = [{ text: option.text, value: option.value }, ...dd[name].data];
            }
            setDpValues(dd)
        }
        if (isCasecade) {
            const fdls = await dpChange(name, value);
            if (fdls) {
                fdls.map(async (item, k) => {
                    props.setFieldValue(item.name, '');
                })
            }
        }

        const data = props.values;
        if (data[name] != null) {
            data[name] = value;
        }

        const calcJson = json.fields.map((item) => {
            if (item.calculation) {
                const context = {
                    data: data
                };

                try {
                    const sandbox = vm.createContext(context);
                    const script = new vm.Script(item.calculation);
                    script.runInContext(sandbox, { timeout: 500 });
                    props.setFieldValue(item.name, sandbox.value);
                }
                catch (e) {
                    props.setFieldValue(item.name, data[name]);
                }
            }
            
            if (item.conditional) { 
                const { field, hasValue, required, display } = item.conditional;
                if (String(data[field]) === String(hasValue)) {
                    item['hidden'] = !display; //hidden is Opp to [This Field Should Display]
                    item['required'] = required;
                } else {
                    item['hidden'] = display;
                }

                //When any file is hidden then clear the value of the filed before it hide
                //For hidden fields remove validation
                if (item['hidden'] == true) { 
                    item['required'] = false;
                    if (item.jsType.toLowerCase() === "bool") {
                        props.setFieldValue(item.name, item.default || false);
                    } else {
                        props.setFieldValue(item.name, item.default || '');
                    }
                }
            }

            //This is for advanced logic, right it not using in HMS. 
            // if (item.logic) {
            //     let logic = JSON.parse(item.logic);
            //     logic && logic.map((l) => {
            //         if (String(data[l.when]) === String(l.value)) {
            //             l.then && l.then.map((p) => {
            //                 if (p.type === 'property') {
            //                     switch (p.name) {
            //                         case "hide":
            //                             item[p.name] = p.value;
            //                             props.setFieldValue(item.name, item.type === "number" || item.type === "decimal" ? 0 : "");
            //                             break;
            //                     }
            //                 }
            //             })
            //         } else {
            //             l.then && l.then.map((p) => {
            //                 if (p.type === 'property') {
            //                     switch (p.name) {
            //                         case "hide":
            //                             item[p.name] = !p.value;
            //                             break;
            //                     }
            //                 }
            //             })
            //         }
            //     })
            // }

            //All reference fields source is dropdowns only and the values comes from other entity
            //When user value changed and field name is equal to the map item field name
            //Get the select value object from the dpValues(dropdown values)
            //Update all reference fields
            if (item.referennceTo == name) {
                if (dpValues[item.referennceTo] && dpValues[item.referennceTo].data) {
                    const v = dpValues[item.referennceTo].data.filter((i) => i.id == value)
                    if (v.length > 0) {
                        if (item.isReference) {
                            props.setFieldValue(item.name, v[0][item.reference.entityPropInfo.name]);
                        } else {
                            props.setFieldValue(item.name, v[0][item.name]);
                        }

                    }
                }
            }
            return item;
        })
        json.fields = calcJson;
        setJson({ ...json });
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
                values={field.dataValues && field.dataValues.length>0 ? field.dataValues : (fieldvals ? fieldvals : [])}
                valueField={field.addValues ? 'value' : (field.dataValues ? 'value' : 'id')}
                disabled={field.disabled || field.auto || false} dpchange={dpChange} handlevaluechange={handleValueChange}
                fieldcode={field.lookupCode} addoptions={fieldAdd ? 1 : 0} 
                showDate={field.disableDate ? false : true}
                showTime={field.enableTime ? true : false}
                default={field.default}
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
            } else {
                matrix[k].push(json.fields[i]);
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

export default DynamicForm;