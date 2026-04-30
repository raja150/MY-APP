import React, { Fragment } from 'react'
import { Button, Card, CardBody, Col, Row, Container, FormGroup, Label } from 'reactstrap';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { FieldError } from '../../../domain/Error/ErrorMessage'
import { RWDropdownList, Input, RWDatePicker, PhoneNumber } from '../../dynamicform/Controls';


function PurchaseDetails(props) {
    const { errors, touched, values, branchs, vendors } = props
    const handleValueChange = async (name, value, { option, isCasecade }) => {
        props.setFieldValue(name, value);

    }

    return (
        <Fragment>
            <Row>
                <Col md="12">

                    <Row>
                        <Col xs='6'>
                            <RWDropdownList {...{
                                name: 'branchId', label: 'Branch', valueField: 'id', textField: 'branchName',
                                value: values['branchId'], type: 'string', values: branchs,
                                error: errors['branchId'], touched: touched['branchId']
                            }}
                                handlevaluechange={handleValueChange}
                            />
                        </Col>
                        <Col xs='6'>
                            <Input {...{
                                name: 'no', label: 'Purchase Order Id',
                                value: values['no'], type: 'string', disabled: true,
                                error: errors['no'], touched: touched['no']
                            }}
                                handlevaluechange={handleValueChange}
                            />
                        </Col>
                    </Row>

                    <Row>
                        <Col xs="6">
                            <RWDatePicker {...{
                                name: 'orderDate', label: 'Purchase Order Date',
                                value: values['orderDate'], showDate: true, showTime: false,
                                error: errors['orderDate'], touched: touched['orderDate']
                            }}
                                handlevaluechange={handleValueChange}
                            />
                        </Col>
                        <Col xs="6">
                            <RWDatePicker {...{
                                name: 'expectedDate', label: 'Order Expected Date',
                                value: values['expectedDate'], showDate: true, showTime: false,
                                error: errors['expectedDate'], touched: touched['expectedDate']
                            }}
                                handlevaluechange={handleValueChange}
                            />
                        </Col>
                    </Row>
                    <Row>
                        <Col xs="6">
                            <RWDropdownList {...{
                                name: 'vendorId', label: 'Vendor', valueField: 'id', textField: 'vendorName',
                                value: values['vendorId'], type: 'string', values: vendors,
                                error: errors['vendorId'], touched: touched['vendorId']
                            }}
                                handlevaluechange={handleValueChange}
                            />
                        </Col>
                        <Col xs="6">
                            <Input {...{
                                name: 'vendorMobileNo', label: 'Contact Person Mobile Number',
                                value: values['vendorMobileNo'], type: 'string',
                                error: errors['vendorMobileNo'], touched: touched['vendorMobileNo']
                            }}
                                handlevaluechange={handleValueChange}
                            />
                        </Col>
                    </Row>
                    <Row>
                        <Col xs="6">
                            <RWDropdownList {...{
                                name: 'status', label: 'Status', valueField: 'value', textField: 'text',
                                value: values['status'], type: 'string',
                                values: [{ value: 1, text: "Initiated" }, { value: 2, text: "Approved" },
                                { value: 3, text: "Rejected" },
                                { value: 4, text: "Sent" }],
                                error: errors['status'], touched: touched['status']
                            }}
                                handlevaluechange={handleValueChange}
                            />
                        </Col>
                    </Row>


                </Col>

            </Row>
        </Fragment>
    )
}

export default PurchaseDetails
