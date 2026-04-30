import { faPlus, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { RWDropdownList } from 'components/dynamicform/Controls';
import { Field } from 'formik';
import React, { Fragment, useState } from 'react';
import { Button, Col, Label, Row } from 'reactstrap';
import { EightyCItem } from './ConstValues';
import Error from '../../../components/dynamicform/Controls/Error';

const EightyC = ({ form, remove, push, ...props }) => {
    const { values, errors, touched, setFieldValue } = form;
    const [index, setIndex] = useState(0)
    const handleValueChange = async (name, value) => {
        form.setFieldValue(name, value);
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
            {values.eightyCs.length > 0 &&
                values.eightyCs.map((item, index) => {

                    return (
                        <div>
                            <Row>
                                <Col md='5'>
                                    <RWDropdownList
                                        {...{
                                            name: `eightyCs.${index}.section80CId`,
                                            label: 'Section 80C',
                                            valueField: 'id',
                                            textField: 'name',
                                            value: item.section80CId,
                                            values: values.eightyCItems,
                                            error: lineItemErrorMsg(errors.eightyCs, 'section80CId'),
                                            touched: lineItemIsTouched(touched.eightyCs, 'section80CId'),
                                        }}
                                        handlevaluechange={handleValueChange}
                                    />
                                </Col>
                                <Col md='4'>
                                    <Label>Amount</Label>
                                    <Field
                                        type="number"
                                        name={`eightyCs.${index}.amount`}
                                        className={`form-control form-control-sm ${lineItemIsTouched(errors.eightyCs, 'amount', index) && lineItemErrorMsg(errors.eightyCs, 'amount', index) ? 'is-invalid' : ''
                                            }`}
                                        value={item.amount}
                                    />
                                    <Error touched={touched} error={lineItemErrorMsg(errors.eightyCs, 'amount', index)} />
                                </Col>
                                <Col md='3'>
                                    {
                                        values.eightyCs.length > 1 ?
                                            <Row>
                                                <Col md='12' className="mt-3">
                                                    <Button
                                                        className="p-1 border-0 btn-transition"
                                                        outline
                                                        tabIndex="-1"
                                                        color="danger"
                                                        name="Action"
                                                        onClick={() => { remove(index) }}
                                                    >   Remove 80C Investment&nbsp;
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
                            push(EightyCItem());
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
                    push(EightyCItem());
                }}
                className="p-1 border-0 btn-transition"
                outline
                color="primary"
            >   Add 80C Investments&nbsp;
                <FontAwesomeIcon icon={faPlus} />
            </Button>
        </Fragment>
    )
}
export default EightyC;