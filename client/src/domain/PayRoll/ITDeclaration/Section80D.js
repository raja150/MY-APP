import { faPlus, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { RWDropdownList } from 'components/dynamicform/Controls';
import { Field } from 'formik';
import React, { Fragment } from 'react';
import { Button, Col, Label, Row } from 'reactstrap';
import { EightyDItem } from './ConstValues'; 
import Error from '../../../components/dynamicform/Controls/Error';

const EightyD = ({ form, remove, push, ...props }) => {
    const { values, errors, touched } = form;

    const handleValueChange = async (name, value, { selected, index }) => {
        form.setFieldValue(name, value);
        if (selected !== undefined) {
            form.setFieldValue(`eightyDs.${index}.limit`, selected.limit)
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

    return (
        <Fragment>
            {values.eightyDs.length > 0 &&
                values.eightyDs.map((item, index) => {
                    return (
                        <div>
                            <Row>
                                <Col md='5'>
                                    <RWDropdownList
                                        {...{
                                            name: `eightyDs.${index}.section80DId`,
                                            label: 'Medi Claim Policy',
                                            valueField: 'id',
                                            textField: 'name',
                                            value: item.section80DId,
                                            values: values.eightyDItems,
                                            index: index,
                                            error: lineItemErrorMsg(errors.eightyDs, 'section80DId'),
                                            touched: lineItemIsTouched(touched.eightyDs, 'section80DId'),
                                        }}
                                        handlevaluechange={handleValueChange}
                                    />
                                    <span><strong>Max Limit : </strong> {item.limit}</span>
                                </Col>
                                <Col md='4'>
                                    <Label>Amount</Label>
                                    <Field
                                        type="number"
                                        name={`eightyDs.${index}.amount`}
                                        className={`form-control form-control-sm ${lineItemIsTouched(errors.eightyDs, 'amount', index)
                                            && lineItemErrorMsg(errors.eightyDs, 'amount', index) ? 'is-invalid' : ''
                                            }`}
                                        value={item.amount}
                                    /> 
                                    <Error touched={touched} error={lineItemErrorMsg(errors.eightyDs, 'amount', index)} />
                                </Col>
                                <Col md="3">
                                    {
                                        values.eightyDs.length > 1 ?
                                            <Row>
                                                <Col md='12' className='mt-3'>
                                                    <Button
                                                        className="p-1 border-0 btn-transition"
                                                        outline
                                                        tabIndex="-1"
                                                        color="danger"
                                                        name="Action"
                                                        onClick={() => { remove(index) }}
                                                    >   Remove 80D Investment&nbsp;
                                                        <FontAwesomeIcon icon={faTrashAlt} />
                                                    </Button>
                                                </Col>
                                            </Row> : ''
                                    }
                                </Col>
                            </Row>
                            <hr />
                        </div>
                    )
                })}
            {/* <tr>
                <th className="text-left">
                    Add Investments
                    <Button
                        onClick={() => {
                            push(EightyDItem());
                        }}
                        className="p-1 border-0 btn-transition"
                        outline
                        color="primary"
                    >
                        <FontAwesomeIcon icon={faPlus} />
                    </Button>
                </th>
            </tr> */}
            <Button
                onClick={() => {
                    push(EightyDItem());
                }}
                className="p-1 border-0 btn-transition"
                outline
                color="primary"
            >   Add 80D Investments&nbsp;
                <FontAwesomeIcon icon={faPlus} />
            </Button>
        </Fragment>
    )
}
export default EightyD;