import { faPlus, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { RWDropdownList } from 'components/dynamicform/Controls';
import { Field } from 'formik';
import React, { Fragment } from 'react';
import { Button, Label } from 'reactstrap';
import { EmptyDeductionDetails } from './ConstValues';


function DeductionLineItem({ form, push, remove, ...props }) {
    const { values, errors, touched, setFieldValue } = form

    const handleValueChange = async (name, value) => {

        setFieldValue(name, value)
    }
    const deductionItemErrMsg = (error, name, i) => {

        if (error && error[i] && error[i][name]) {
            return error[i][name];
        }
        return "";
    }
    const deductionItemTouched = (touch, name, i) => {

        if (touch && touch[i] && touch[i][name]) {
            return touch[i][name];
        }
        return false;
    }
    return (
        <Fragment>
            {values.salaryDeductions.length > 0 &&
                values.salaryDeductions.map((deduction, index) => {
                    return (
                        <tr key={index + 1}>
                            <td >
                                <RWDropdownList {...{
                                    name: `salaryDeductions.${index}.deductionId`, value: deduction.deductionId,
                                    error: errors.salaryDeductions && deductionItemErrMsg(errors.salaryDeductions, 'deductionId', index),
                                    touched: touched.salaryDeductions && deductionItemTouched(touched.salaryDeductions, 'deductionId', index),
                                    values: values.deductions,
                                    valueField: 'id', textField: 'name'
                                }} handlevaluechange={handleValueChange} />
                            </td>
                            <td>
                                <Field type='number' name={`salaryDeductions.${index}.monthly`} value={deduction.monthly}
                                    onChange={(e) => handleValueChange(e.target.name, e.target.value)}
                                    className={`form-control form-control-sm text-right ${touched.monthly && errors.monthly ? "is-invalid" : ""}`} />
                            </td>
                            <td>
                                <Button onClick={() => { remove(index); }}
                                    className="p-1 border-0 btn-transition" outline color="danger">
                                    <FontAwesomeIcon icon={faTrash} />
                                </Button>
                            </td>
                        </tr>

                    )
                })
            }
            <tr>
                <td colSpan="4">
                    <Button onClick={() => { push(EmptyDeductionDetails()); }}
                        className="p-1 border-0 btn-transition" outline color="primary">
                        <Label>Add Deduction Type</Label> <FontAwesomeIcon icon={faPlus} ></FontAwesomeIcon>
                    </Button>
                </td>
            </tr>

        </Fragment>
    )
}
export default DeductionLineItem