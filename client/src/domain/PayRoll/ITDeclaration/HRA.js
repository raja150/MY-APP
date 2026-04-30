import { faPlus, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Input, RWDatePicker, RWDropdownList, TextAreaInput } from 'components/dynamicform/Controls';
import { Field } from 'formik';
import React, { Fragment, useEffect } from 'react';
import { Button, Col, Label, Row } from 'reactstrap';
import * as dateUtil from "../../../utils/date";
import { HRAEmptyItem } from './ConstValues';
import { FieldError } from '../../Error/ErrorMessage';

const LineItem = ({ form, remove, push, ...props }) => {
    const { values, errors, touched, setFieldValue } = form;
    //const [index, setIndex] = useState(0)

    useEffect(() => {

    }, [values])
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
    const handleExpiryOnChange = async (name, value) => {
        // if(name == "")
        //Get last day of a month
        handleValueChange(name, dateUtil.lastDayOfMOnth(value));
    }

    return (
        <Fragment>
            {values.hraItems.length > 0 &&
                values.hraItems.map((lineItem, index) => {
                    return (
                        <div>
                            <Row>
                                <Col md='3'>
                                    <RWDatePicker {...{
                                        name: `hraItems.${index}.rentalFrom`, label: 'Rental From',
                                        views: ["year", "decade", "century"], format: "MM/YY",
                                        editFormat: "MM/YY", dateType: "Start",
                                        // min: moment(new Date(values.fromMonth)).format('MM/YY'),
                                        // max: moment(new Date(values.toMonth)).format('MM/YY'),
                                        min: new Date(values.fromMonth),
                                        max: new Date(values.toMonth),
                                        value: lineItem.rentalFrom, showDate: true, showTime: false, disabled: false,
                                        error: lineItemErrorMsg(errors.hraItems, 'rentalFrom', index),
                                        touched: lineItemIsTouched(touched.hraItems, 'rentalFrom', index),
                                    }} handlevaluechange={handleExpiryOnChange} />
                                </Col>
                                <Col md='3'>
                                    <RWDatePicker {...{
                                        name: `hraItems.${index}.rentalTo`, label: 'Rental To',
                                        views: ["year", "decade", "century"],
                                        format: "MM/YY",
                                        editFormat: "MM/YY", dateType: "End",
                                        min: new Date(values.fromMonth),
                                        max: new Date(values.toMonth),
                                        value: lineItem.rentalTo, showDate: true, showTime: false, disabled: false,
                                        error: lineItemErrorMsg(errors.hraItems, 'rentalTo', index),
                                        touched: lineItemIsTouched(touched.hraItems, 'rentalTo', index),
                                    }} handlevaluechange={handleExpiryOnChange} />
                                </Col>
                                <Col md='3'>
                                    <Label>Amount Per Month</Label>
                                    <Field
                                        type="number"
                                        name={`hraItems.${index}.amount`}
                                        className={`form-control form-control-sm ${lineItemIsTouched(errors.hraItems, 'amount', index)
                                            && lineItemErrorMsg(errors.hraItems, 'amount', index) ? 'is-invalid' : ''}`}
                                        value={lineItem.amount}
                                    // error={lineItemErrorMsg(errors.hraItems, 'amount', index)}
                                    // touched={lineItemErrorMsg(touched.hraItems, 'amount', index)}
                                    />
                                    <FieldError errors={errors.hraItems && errors.hraItems[index]}
                                        touched={touched.hraItems && touched.hraItems[index]}
                                        name={`hraItems.${index}.amount`} />
                                </Col>
                                <Col md='3'>
                                    <RWDropdownList
                                        {...{
                                            name: `hraItems.${index}.city`,
                                            label: 'city',
                                            valueField: 'value',
                                            textField: 'text',
                                            value: lineItem.city, index: index,
                                            values: [{ value: 1, text: 'Metro' }, { value: 2, text: 'Non Metro' }],
                                            error: lineItemErrorMsg(errors.hraItems, 'city', index),
                                            touched: lineItemIsTouched(touched.hraItems, 'city', index),
                                        }}
                                        handlevaluechange={handleValueChange}
                                    />

                                    <span><strong>{lineItem.city == "" ? "" : lineItem.city === 1 ? "50% Of Amount" : "40% Of Amount"} </strong></span>

                                </Col>
                            </Row>
                            <Row>
                                <Col md='6'>
                                    <TextAreaInput {...{
                                        name: `hraItems.${index}.address`, label: 'Address', value: lineItem.address, values: ['address'],
                                        error: lineItemErrorMsg(errors.hraItems, 'address', index), touched: lineItemIsTouched(touched.hraItems, 'address', index)
                                    }} handlevaluechange={handleValueChange}
                                    />
                                </Col>
                                <Col md='3'>
                                    <Input {...{
                                        name: `hraItems.${index}.pan`, label: 'Landlord PAN',
                                        value: lineItem.pan, values: ['pan'], type: 'string', disabled: lineItem.amount * 12 > 100000 ? false : true,
                                        error: lineItemErrorMsg(errors.hraItems, 'pan', index), touched: lineItemIsTouched(touched.hraItems, 'pan', index)
                                    }} handlevaluechange={handleValueChange}
                                    />
                                </Col>
                                <Col md='3'>
                                    <Input {...{
                                        name: `hraItems.${index}.landlord`, label: 'Landlord Name',
                                        value: lineItem.landlord, type: 'string', disabled: lineItem.amount * 12 > 100000 ? false : true,
                                        error: lineItemErrorMsg(errors.hraItems, 'landlord', index), touched: lineItemIsTouched(touched.hraItems, 'landlord', index)
                                    }} handlevaluechange={handleValueChange}
                                    />
                                </Col>
                            </Row>
                            {
                                values.hraItems.length > 1 ?
                                    <Row>
                                        <Col md='6'>
                                            <Button
                                                className="p-1 border-0 btn-transition"
                                                outline
                                                tabIndex="-1"
                                                color="danger"
                                                name="Action"
                                                onClick={() => { remove(index) }}
                                            >   Remove Rent House&nbsp;
                                                <FontAwesomeIcon icon={faTrashAlt} />
                                            </Button>
                                        </Col>
                                    </Row> : ''
                            }
                            <hr />
                        </div>
                    );
                })}
            <Button
                onClick={() => {
                    push(HRAEmptyItem());
                }}
                className="p-1 border-0 btn-transition"
                outline
                color="primary"
            >   Add Rented House&nbsp;
                <FontAwesomeIcon icon={faPlus} />
            </Button>
        </Fragment>
    )
}
export default LineItem
