import { FieldArray, Form, Formik, yupToFormErrors } from 'formik';
import _ from 'lodash';
import React, { Fragment, useEffect, useState } from 'react';
import {
    Button, Col, ModalFooter,
    Row
} from 'reactstrap';
import { Field } from 'formik';
import { TransactionDetails } from './constValues';
import {
    Payment_Mode_Type_Values
} from '../../Constant/ModeOfTrans';
import { RWDropdownList } from '../dynamicform/Controls';
import { faPlus, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

function Receipt({form, push, remove, ...props}) {
       
    const { values, errors, touched, handleChange } = form;
   

    const handleInvoice = (values) => {
        const invoiceTotal = _.round(_.sumBy(values.transactions, 'amount'), 2);                    
        return invoiceTotal             
    }

    const handleValueChange = async (name, value, index, vals) => {                
        form.setFieldValue(name, value);

        if(name === `transactions.${index}.amount`) {
            let ary = values.transactions.map((k, d) => {
                if (d === index) {
                    return {
                        ...k, amount : value                        
                    }
                }
                return k
            })
            const invoiceTotal = _.round(_.sumBy(ary, 'amount'), 2);                          
            await form.setFieldValue('totalInfo.invoiceTotal', invoiceTotal)
        }
    }


    return (
        <Fragment>
            <Row>
                <Col md='12'>
                    <h5>Receipt</h5>
                </Col>
            </Row>
            <Row className='mb-2'>
                <Col md='6'>
                    <h6>Receipt Amount</h6>
                </Col>
                <Col md='6'>
                    <h6>{values.totalInfo.receiptAmount}</h6>
                </Col>
            </Row>
            <Row>
                <Col md="12">

                    <table className="table table-bordered table-hover" id="tab_logic">
                        <thead>
                            <th className='text-center'>Mode Of Pay</th>
                            <th className='text-center'>Type</th>
                            <th className='text-center'>Bank</th>
                            <th className='text-center'>Ref No</th>
                            <th className='text-center'>Amount</th>
                            <th style={{ width: '60px' }}>Action</th>
                        </thead>
                        <tbody>
                            {
                                values.transactions.length > 0 && values.transactions.map((lineItem, index) => {
                                    const lineErrors = errors && errors.transactions && errors.transactions[index] ? errors.transactions[index] : null;
                                    const lineTouched = touched && touched.transactions && touched.transactions[index] ? touched.transactions[index] : null;
                          
                                    return (
                                        <tr>
                                            <td className='w-25'>
                                                <RWDropdownList
                                                    {...{
                                                        name: `transactions.${index}.mode`,
                                                        label: '',
                                                        valueField: 'value',
                                                        textField: 'text',
                                                        value: lineItem.mode,
                                                        type: 'string',
                                                        values: Payment_Mode_Type_Values,
                                                        error: lineErrors && lineErrors['mode'], touched: true,
                                                    }}
                                                    handlevaluechange={handleValueChange}
                                                />
                                            </td>
                                            <td className='w-25'>
                                                <RWDropdownList
                                                    {...{
                                                        name: `transactions.${index}.type`,
                                                        label: '',
                                                        valueField: 'value',
                                                        textField: 'text',
                                                        disabled: lineItem.mode == 1 ? true : false,
                                                        value: lineItem.type,
                                                        type: 'string',
                                                        values: [
                                                            { value: 1, text: 'Debit Card' },
                                                            { value: 2, text: 'Credit Card' },
                                                        ],
                                                        error: lineErrors && lineErrors['type'], touched: true,
                                                    }}
                                                    handlevaluechange={handleValueChange}
                                                />
                                            </td>
                                            <td>
                                                <Field
                                                    type="text"
                                                    disabled={lineItem.mode === 1 ? true : false}
                                                    name={`transactions.${index}.bank`}
                                                    className={`form-control form-control-sm ${lineErrors && lineErrors.bank ? 'is-invalid' : ''
                                                        }`}
                                                    value={lineItem.bank}
                                                />
                                                {lineErrors && lineErrors.bank ? <div className='text-danger'>{lineErrors.bank}</div> : ''}
                                            </td>
                                            <td>
                                                <Field
                                                    type="text"
                                                    name={`transactions.${index}.refNo`}
                                                    disabled={lineItem.mode === 1 ? true : false}
                                                    className={`form-control form-control-sm ${lineErrors && lineErrors.refNo ? 'is-invalid' : ''
                                                        }`}
                                                    value={lineItem.refNo}
                                                />
                                                {lineErrors && lineErrors.refNo ? <div className='text-danger'>{lineErrors.refNo}</div> : ''}
                                            </td>
                                            <td>
                                                <Field
                                                    type="number"
                                                    name={`transactions.${index}.amount`}
                                                    className={`form-control form-control-sm ${lineErrors && lineErrors.amount ? 'is-invalid' : ''
                                                        }`}
                                                    value={lineItem.amount}
                                                    onChange= {(e) => handleValueChange(e.target.name, e.target.value, index, values.transactions)}
                                                />
                                                {lineErrors && lineErrors.amount ? <div className='text-danger'>{lineErrors.amount}</div> : ''}
                                            </td>

                                            <td className="text-center">
                                                {values.transactions.length === index + 1 ? (
                                                    <Button
                                                        onClick={() => {
                                                            push(TransactionDetails());
                                                        }}
                                                        className="p-1 border-0 btn-transition"
                                                        outline
                                                        color="primary"
                                                    >
                                                        <FontAwesomeIcon icon={faPlus} />
                                                    </Button>
                                                ) : (
                                                        <Button
                                                            className="p-1 border-0 btn-transition"
                                                            outline
                                                            tabIndex="-1"
                                                            color="danger"
                                                            name="Action"
                                                            onClick={() => remove(index)}
                                                        >
                                                            <FontAwesomeIcon icon={faTrashAlt} />
                                                        </Button>
                                                    )}
                                            </td>
                                        </tr>
                                    )
                                })
                            }
                            <tr>
                                <td></td>
                                <td colSpan="3" className="text-center font-weight-bold">Total</td>
                                <td colSpan="2" className="font-weight-bold">
                                    {values.totalInfo.invoiceTotal}
                                </td>
                            </tr>
                        </tbody>
                    </table>

                </Col>
            </Row>

        </Fragment>
    );
}

export default Receipt;












