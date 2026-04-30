import { faPlus, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Input, Radio } from 'components/dynamicform/Controls';
import { Field } from 'formik';
import React, { Fragment, useState } from 'react';
import { Button, Col, Label, Row } from 'reactstrap';
import * as compare from '../../../utils/Compare';
import { LetOutEmptyItem } from './ConstValues';
import Error from '../../../components/dynamicform/Controls/Error';

const LetOutItem = ({ form, remove, push, ...props }) => {
    const { values, errors, touched, setFieldValue } = form;
    const [index, setIndex] = useState(0)
    const handleValueChange = async (name, value, { index }) => {
        form.setFieldValue(name, value);

        if (name === `letOutItems.${index}.repayingHomeLoan`) {
            const netAnnualValue = values.letOutItems[index].netAnnualValue;
            const standardDeduction = values.letOutItems[index].standardDeduction;
            const netIncome = netAnnualValue - standardDeduction;

            form.setFieldValue(`letOutItems.${index}.interestPaid`, 0);
            form.setFieldValue(`letOutItems.${index}.netIncome`, netIncome);

        }
    }

    const lineItemErrorMsg = (error, name, i) => {
        if (error && error[i] && error[i][name]) {
            return error[i][name];
        }
        return "";
    }

    const lineItemIsTouched = (touch, name, i) => {
        if (touch && touch[i] && touch[i][name]) {
            return touch[i][name];
        }
        return false;
    }

    const updatedDetails = (lineItem) => {
        const netAnnualValue = parseInt(lineItem.annualRentReceived) - parseInt(lineItem.municipalTaxPaid)
        const standardDeduction = (30 * netAnnualValue) / 100;
        const netIncome = netAnnualValue - standardDeduction - lineItem.interestPaid;
        return {
            ...lineItem,
            netAnnualValue,
            standardDeduction,
            netIncome
        }
    }

    return (
        <Fragment>
            {values.letOutItems.length > 0 &&
                values.letOutItems.map((item, index) => {

                    return (
                        <div>
                            <Row>
                                <Col md='6'>
                                    <Label> Annual Rent Received</Label>
                                    <Field
                                        type="number"
                                        name={`letOutItems.${index}.annualRentReceived`}
                                        className={`form-control form-control-sm ${lineItemIsTouched(touched.letOutItems, 'annualRentReceived', index) && lineItemErrorMsg(errors.letOutItems, 'annualRentReceived', index) ? 'is-invalid' : ''
                                            }`}
                                        value={item.annualRentReceived}
                                        onChange={(e) => {
                                            const calcDetails = updatedDetails({
                                                ...item,
                                                annualRentReceived: e.target.value

                                            });
                                            setFieldValue(`letOutItems.${index}.netAnnualValue`, calcDetails.netAnnualValue);
                                            setFieldValue(`letOutItems.${index}.standardDeduction`, calcDetails.standardDeduction);
                                            setFieldValue(`letOutItems.${index}.netIncome`, calcDetails.netIncome);
                                            setFieldValue(`letOutItems.${index}.annualRentReceived`, e.target.value)
                                        }}
                                    />
                                    <Error touched={touched} error={lineItemErrorMsg(errors.letOutItems, 'annualRentReceived', index)} />
                                </Col>
                                <Col md='6'>
                                    <Label>Municipal/Local Tax Paid</Label>
                                    <Field
                                        type="number"
                                        name={`letOutItems.${index}.municipalTaxPaid`}
                                        className={`form-control form-control-sm ${lineItemIsTouched(touched.letOutItems, 'municipalTaxPaid', index)
                                            && lineItemErrorMsg(errors.letOutItems, 'municipalTaxPaid', index) ? 'is-invalid' : ''
                                            }`}
                                        value={item.municipalTaxPaid}
                                        onChange={(e) => {
                                            const calcDetails = updatedDetails({
                                                ...item,
                                                municipalTaxPaid: e.target.value

                                            });

                                            setFieldValue(`letOutItems.${index}.netAnnualValue`, calcDetails.netAnnualValue);
                                            setFieldValue(`letOutItems.${index}.standardDeduction`, calcDetails.standardDeduction);
                                            setFieldValue(`letOutItems.${index}.netIncome`, calcDetails.netIncome);
                                            setFieldValue(`letOutItems.${index}.municipalTaxPaid`, e.target.value)
                                        }}
                                    />

                                    <Error touched={touched} error={lineItemErrorMsg(errors.letOutItems, 'municipalTaxPaid', index)} />
                                </Col>
                            </Row>
                            <Row>
                                <Col md='6'>
                                    <Label>Net Annual Value</Label>
                                    <Field
                                        type="number"
                                        name={`letOutItems.${index}.netAnnualValue`}
                                        className={`form-control form-control-sm ${lineItemIsTouched(touched.letOutItems, 'netAnnualValue', index) && lineItemErrorMsg(errors.letOutItems, 'netAnnualValue', index) ? 'is-invalid' : ''
                                            }`}
                                        value={item.netAnnualValue}
                                        disabled
                                    />
                                </Col>
                                <Col md='6'>
                                    <Label>Standard Deduction(30% of Net Annual Value)</Label>
                                    <Field
                                        type="number"
                                        name={`letOutItems.${index}.standardDeduction`}
                                        className={`form-control form-control-sm ${lineItemIsTouched(touched.letOutItems, 'standardDeduction', index) && lineItemErrorMsg(errors.letOutItems, 'standardDeduction', index) ? 'is-invalid' : ''
                                            }`}
                                        value={item.standardDeduction}
                                        disabled
                                    />
                                </Col>
                            </Row>
                            <Row>
                                <Col md='4'>
                                    <Radio {...{
                                        name: `letOutItems.${index}.repayingHomeLoan`, label: 'Repaying Home Loan For This Property', index: index,
                                        values: [{ text: 'Yes', value: '1' }, { text: 'No', value: '0' }], value: item.repayingHomeLoan
                                    }} handlevaluechange={handleValueChange}
                                    />
                                </Col>

                            </Row>
                            {compare.isEqual(item.repayingHomeLoan, "1") ?
                                <Row>
                                    <Col md='6' >
                                        <Label>Interest Paid On Home Loan</Label>
                                        <Field
                                            type="number"
                                            name={`letOutItems.${index}.interestPaid`}
                                            label=""
                                            className={`form-control form-control-sm ${lineItemIsTouched(touched.letOutItems, 'interestPaid', index) 
                                                && lineItemErrorMsg(errors.letOutItems, 'interestPaid', index) ? 'is-invalid' : ''
                                                }`}
                                            value={item.interestPaid}
                                            onChange={(e) => {
                                                const calcDetails = updatedDetails({
                                                    ...item,
                                                    interestPaid: e.target.value

                                                });

                                                // setFieldValue(`letOutItems.${index}.netAnnualValue`, calcDetails.netAnnualValue);
                                                setFieldValue(`letOutItems.${index}.netIncome`, calcDetails.netIncome);
                                                setFieldValue(`letOutItems.${index}.interestPaid`, e.target.value)
                                            }}
                                        />
                                        <Error touched={touched} error={lineItemErrorMsg(errors.letOutItems, 'interestPaid', index)} />
                                        {/* <Input {...{
                                            name: `letOutItems.${index}.interestPaid`, label: 'Interest Paid On Home Loan',
                                            value: item.interestPaid, type: 'number',
                                            error: lineItemErrorMsg(errors.letOutItems, 'interestPaid', index), touched: lineItemIsTouched(touched.letOutItems, 'interestPaid', index)
                                        }} handlevaluechange={handleValueChange}
                                        /> */}
                                    </Col>
                                    <Col md='6'>
                                        <Input {...{
                                            name: `letOutItems.${index}.principle`, label: 'Principle Of Home Loan',
                                            value: item.principle, type: 'number',
                                            error: lineItemErrorMsg(errors.letOutItems, 'principle', index), touched: lineItemIsTouched(touched.letOutItems, 'principle', index)
                                        }} handlevaluechange={handleValueChange}
                                        />
                                    </Col>
                                    <Col md='6'>
                                        <Input {...{
                                            name: `letOutItems.${index}.lenderPAN`, label: 'Lender PAN',
                                            value: item.lenderPAN, type: 'string',
                                            error: lineItemErrorMsg(errors.letOutItems, 'lenderPAN', index), touched: lineItemIsTouched(touched.letOutItems, 'lenderPAN', index)
                                        }} handlevaluechange={handleValueChange}
                                        />
                                    </Col>
                                    <Col md='6'>
                                        <Input {...{
                                            name: `letOutItems.${index}.nameOfLender`, label: 'Name Of The Lender',
                                            value: item.nameOfLender, type: 'string',
                                            error: lineItemErrorMsg(errors.letOutItems, 'nameOfLender', index), touched: lineItemIsTouched(touched.letOutItems, 'nameOfLender', index)
                                        }} handlevaluechange={handleValueChange}
                                        />
                                    </Col>

                                </Row> : ''}
                            <Row>
                                <Col md='6'>
                                    <Input {...{
                                        name: `letOutItems.${index}.netIncome`, label: 'Net Income / Loss From House Property',
                                        value: item.netIncome, type: 'number',
                                        error: lineItemErrorMsg(errors.letOutItems, 'netIncome', index), touched: lineItemIsTouched(touched.letOutItems, 'netIncome', index)
                                    }} handlevaluechange={handleValueChange}
                                        disabled
                                    />
                                </Col>
                            </Row>
                            {
                                values.letOutItems.length > 1 ?
                                    <Row>
                                        <Col md='6'>
                                            <Button
                                                className="p-1 border-0 btn-transition"
                                                outline
                                                tabIndex="-1"
                                                color="danger"
                                                name="Action"
                                                onClick={() => { remove(index) }}
                                            >   Remove Let-out Property Income&nbsp;
                                                <FontAwesomeIcon icon={faTrashAlt} />
                                            </Button>
                                        </Col>
                                    </Row> : ''
                            }
                            <hr />
                        </div>
                    )
                })}
            <Button
                onClick={() => {
                    push(LetOutEmptyItem());
                }}
                className="p-1 border-0 btn-transition"
                outline
                color="primary"
            >   Add Let-out Property Income&nbsp;
                <FontAwesomeIcon icon={faPlus} />
            </Button>
        </Fragment>
    )
}
export default LetOutItem;