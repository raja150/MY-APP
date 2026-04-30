import { faPlus, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Field } from 'formik';
import React, { Fragment } from 'react';
import { Button } from 'reactstrap';
import {
  Payment_Mode_Type_Values
} from '../../Constant/ModeOfTrans';
import { RWDropdownList } from '../dynamicform/Controls';
import { TransactionDetails } from './constValues';


function Transaction({ form, remove, push, props }) {

  const { values, errors, touched, setFieldValue } = form;

  const handleValueChange = async (name, value, { option, isCasecade }) => {
    form.setFieldValue(name, value);

  };


  return (
    <Fragment>
      {values.transaction.length > 0 &&
        values.transaction.map((lineItem, index) => {
          const lineErrors = errors && errors.transaction && errors.transaction[index] ? errors.transaction[index] : null;
          const lineTouched = touched && touched.transaction && touched.transaction[index] ? touched.transaction[index] : null;

          return (
            <tr>
              <td>
                <RWDropdownList
                  {...{
                    name: `transaction.${index}.mode`,
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
              <td>
                <RWDropdownList
                  {...{
                    name: `transaction.${index}.type`,
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
                  name={`transaction.${index}.bank`}
                  className={`form-control form-control-sm ${touched.bank && errors.bank ? 'is-invalid' : ''
                    }`}
                  value={lineItem.bank}
                />
                {lineErrors && lineErrors.bank ? <div className='text-danger'>{lineErrors.bank}</div> : ''}
              </td>
              <td>
                <Field
                  type="text"
                  name={`transaction.${index}.refNo`}
                  disabled={lineItem.mode === 1 ? true : false}
                  className={`form-control form-control-sm ${touched.orderId && errors.orderId ? 'is-invalid' : ''
                    }`}
                  value={lineItem.refNo}
                />
                {lineErrors && lineErrors.refNo ? <div className='text-danger'>{lineErrors.refNo}</div> : ''}
              </td>
              <td>
                <Field
                  type="number"
                  name={`transaction.${index}.amount`}
                  className={`form-control form-control-sm ${touched.orderDate && errors.orderDate ? 'is-invalid' : ''
                    }`}
                  value={lineItem.amount}
                />
                {lineErrors && lineErrors.amount ? <div className='text-danger'>{lineErrors.amount}</div> : ''}
              </td>

              <td className="text-center">
                {values.transaction.length === index + 1 ? (
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
          );
        })}
      {/* {values.transaction.length > 0 &&
                values.transaction.map((lineItem, index) => {

                    return (
                        <Fragment>
                            {

                                <Fragment>
                                    <Row className='align-items-center'>
                                        <Col md='6'><strong>Mode of Pay</strong></Col>
                                        <Col md='6'>
                                            <RWDropdownList {...{
                                                name: `transaction.${index}.modeOfPay`, label: '', valueField: 'value', textField: 'text',
                                                value: values['transaction.modeOfPay'], type: 'string',
                                                values: [{ value: 1, text: "Card" }, { value: 2, text: "Cash" },
                                                { value: 3, text: "Cheque" }, { value: 4, text: "Wallet" }],
                                                error: errors['transaction.modeOfPay'], touched: touched['transaction.modeOfPay']
                                            }}
                                                handlevaluechange={handleValueChange}
                                            />
                                        </Col>
                                      

                                    </Row>
                                    <Row className='align-items-center'>
                                    <Col md='6' ><strong>Transaction id/ cheque number</strong></Col>
                                    <Col md='6'>
                                       
                                         <Input {...{
                                        name: `transaction.${index}.transactionId`, label: '',
                                        value: values['lineItem.transactionId'], type: 'string', disabled: false,
                                        error: errors['lineItem.transactionId'], touched: touched['lineItem.transactionId']
                                    }}
                                        handlevaluechange={handleValueChange}
                                    />
                                    </Col>
                                    
                                    </Row>
                                    <Row>
                                     <Col md= '4'>
                                     </Col>
                                     <Col md='4'></Col>
                                     <Col md='4'>
                                     {values.transaction.length === index + 1 ?
                                            <Button onClick={() => { push(TransactionDetails()); }}
                                                className="p-1 border-0 btn-transition" outline color="primary">
                                                <FontAwesomeIcon icon={faPlus} />
                                            </Button>
                                            : <Button className="p-1 border-0 btn-transition" outline tabIndex="-1"
                                                color="danger" name="Action" onClick={() => remove(index)}>
                                                <FontAwesomeIcon icon={faTrashAlt} />
                                            </Button>
                                        }
                                     </Col>
                                    </Row>
                                </Fragment>
                            }
                        </Fragment>

                    )
                })} */}
    </Fragment>
  );
}

export default Transaction;
