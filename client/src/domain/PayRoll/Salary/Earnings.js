import { faPlus, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { RWDropdownList } from 'components/dynamicform/Controls';
import { Field } from 'formik';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, InputGroup, InputGroupAddon, InputGroupText, Label } from 'reactstrap';
import * as NumberUtil from '../../../utils/number';
import { EmptyItemDetails } from './ConstValues';
function LineItem({ form, push, remove, ...props }) {

    const [isLoading, setIsLoading] = useState(false);
    const { values, errors, touched, setFieldValue } = form

    useEffect(() => {
        // Calculate amount for percent  on CTC components.
        // Any non depended component like Basic 
        //percentOn 1 On CTC, 2 On Header
        values.templateComponents.map(temp => {
            if (temp.percentOn === 1 && temp.percentage) {
                temp["monthly"] = NumberUtil.RoundOff((temp.percentage / 100) * values.monthly);
                temp["annually"] = temp["monthly"] * 12
            } else if (temp.type === 2) {
                temp["annually"] = temp["monthly"] * 12
            }
        });

        // Calculate amount for percent on components .
        // Any depended on component like HRA (On basic amuont) 
        values.templateComponents.map((temp, i) => {
            if (temp.percentOn === 2 && temp.percentage) {
                const componentby = values.templateComponents.find(f => temp.percentOnCompId == f.componentId);
                if (componentby) {
                    temp["monthly"] = NumberUtil.RoundOff((temp.percentage / 100) * componentby.monthly);
                    temp["annually"] = temp["monthly"] * 12;
                } else {
                    temp["monthly"] = 0;
                    temp["annually"] = 0;
                }
            }
        });
        setFieldValue('annually', NumberUtil.RoundOff(values.monthly * 12));
    }, [values.monthly]);


    const updatedDetails = (lineItem) => {
        const amount = NumberUtil.RoundOff(((lineItem.percentage / 100) * values.annually) / 12);
        const annually = NumberUtil.RoundOff(amount * 12);

        return {
            ...lineItem,
            monthly: amount,
            annually: annually,
            ctc: annually
        }
    }

    const updatedFixedAmount = (lineItem) => {
        const monthly = NumberUtil.RoundOff(lineItem.monthly * 1);
        const annually = NumberUtil.RoundOff(monthly * 12)

        return {
            ...lineItem,
            monthly: monthly,
            annually: annually,
            ctc: annually,
            percentage: null
        }
    }
    const earningItemErrMsg = (error, name, i) => {
        
        if (error && error[i] && error[i][name]) {
            return error[i][name];
        }
        return "";
    }
    const earningItemTouched = (touch, name, i) => {
        
        if (touch && touch[i] && touch[i][name]) {
            return touch[i][name];
        }
        return false;
    }
    const handleValueChange = async (name, value) => {
        setFieldValue(name, value)
    }

    //First display all template components and then by selected components    
    return (
        <Fragment>
            {values.templateComponents.length > 0 &&
                values.templateComponents.map((lineItem, index) => {
                    return (
                        lineItem.fromTemplate ?
                            <tr key={index + 1}>
                                <td >
                                    {lineItem.componentName}
                                </td>
                                {/* Calculation Type */}
                                <td>
                                    {lineItem.type === 2 ? <div>Fixed Amount</div> :
                                        <InputGroup className='input-group-sm'>
                                            <Field type="number" name={`templateComponents.${index}.percentage`} value={lineItem.percentage}
                                                className={`form-control form-control-sm w-60 text-right ${touched.percentage && errors.percentage ? "is-invalid" : ""}`}
                                                onChange={(e) => {
                                                    const calcDetails = updatedDetails({
                                                        ...lineItem,
                                                        percentage: e.target.value

                                                    });

                                                    setFieldValue(`templateComponents.${index}.monthly`, NumberUtil.RoundOff(calcDetails.monthly));
                                                    setFieldValue(`templateComponents.${index}.annually`, NumberUtil.RoundOff(calcDetails.annually));
                                                    setFieldValue(`templateComponents.${index}.percentage`, e.target.value)
                                                }}
                                            />
                                            <InputGroupAddon addonType="append" className="form-control form-control-sm p-0 w-40">
                                                <InputGroupText className='w-100'>{lineItem.percentOn == 1 ? "%of CTC" : `%of ${lineItem.percentOnCompName}`}</InputGroupText>
                                            </InputGroupAddon>
                                        </InputGroup>
                                    }
                                </td>
                                {/* Amount Monthly	 */}
                                <td>
                                    <Field type="number" name={`templateComponents.${index}.monthly`} value={lineItem.monthly}
                                        className={`form-control form-control-sm text-right ${touched.monthly && errors.monthly ? "is-invalid" : ""}`}
                                        onChange={(e) => {
                                            const calcDetails = updatedFixedAmount({
                                                ...lineItem,
                                                monthly: e.target.value
                                            });

                                            setFieldValue(`templateComponents.${index}.percentage`, calcDetails.percentage);
                                            setFieldValue(`templateComponents.${index}.annually`, NumberUtil.RoundOff(calcDetails.annually));
                                            setFieldValue(`templateComponents.${index}.monthly`, NumberUtil.RoundOff(calcDetails.monthly))
                                        }} />
                                </td>
                                {/* Amount Annually */}
                                <td className='text-right'>
                                    {lineItem.annually && NumberUtil.RoundOff(lineItem.annually)}
                                </td>
                            </tr>
                            : ''
                    )
                })}
            {values.templateComponents.length > 0 &&
                values.templateComponents.map((lineItem, index) => { 
                    return (lineItem.fromTemplate === false ?
                        <tr key={index + 1}>
                            <td >
                                <RWDropdownList {...{
                                    name: `templateComponents.${index}.componentId`, value: lineItem.componentId,type:'string',
                                    error: errors.templateComponents && earningItemErrMsg(errors.templateComponents, 'componentId',index),
                                    touched: touched.templateComponents && earningItemTouched(touched.templateComponents, 'componentId',index),
                                    values: values.components,
                                    valueField: 'id', textField: 'name'
                                }} handlevaluechange={handleValueChange} />


                            </td>
                            {/* Calculation Type */}
                            <td>
                                {
                                    <Label>Fixed Amount</Label>
                                }

                            </td>
                            {/* Amount Monthly	 */}
                            <td>
                                <Field type='number' name={`templateComponents.${index}.monthly`} value={lineItem.monthly}
                                    onChange={(e) => {
                                        const calcDetails = updatedFixedAmount({
                                            ...lineItem,
                                            monthly: e.target.value
                                        });
                                        setFieldValue(`templateComponents.${index}.annually`, NumberUtil.RoundOff(calcDetails.annually));
                                        setFieldValue(`templateComponents.${index}.monthly`, NumberUtil.RoundOff(calcDetails.monthly))

                                    }}
                                    className={`form-control form-control-sm text-right ${touched.monthly && errors.monthly ? "is-invalid" : ""}`} />
                            </td>
                            {/* Amount Annually */}
                            <td className='text-right'>

                                <Label htmlFor='annually'><strong>{lineItem.annually > 0 ? NumberUtil.RoundOff(lineItem.annually) : 0}</strong></Label>
                            </td>
                            <td>
                                <Button onClick={() => { remove(index); }}
                                    className="p-1 border-0 btn-transition" outline color="danger">
                                    <FontAwesomeIcon icon={faTrash} />
                                </Button>
                            </td>
                        </tr>
                        : ''
                    )
                })}
            <tr>
                <td colSpan="4">
                    <Button onClick={() => { push(EmptyItemDetails()); }}
                        className="p-1 border-0 btn-transition" outline color="primary">
                        <Label>Add Component</Label> <FontAwesomeIcon icon={faPlus} ></FontAwesomeIcon>
                    </Button>
                </td>
            </tr>

        </Fragment>
    )
}

export default LineItem
