import React, { Fragment } from 'react'
import { Button, Card, CardBody, Col, Row, Container, FormGroup, Label } from 'reactstrap';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { FieldError } from '../../../domain/Error/ErrorMessage'
import { RWDropdownList, Input, RWDatePicker } from '../../dynamicform/Controls';


function VendorSummary(props) {
    const { errors, touched, values, branchs, vendors } = props
    const handleValueChange = async (name, value, { option, isCasecade }) => {

        props.setFieldValue(name, value);
    }

    return (
        <Fragment>
            <Row>
                <Col md="12">
                    <Card className="mb-3">
                        <CardBody>
                            <Row>
                                <Col xs='6'>
                                    <Input {...{
                                        name: 'paymentId', label: 'Payment Id',
                                        value: values['paymentId'], type: 'string', disabled: true,
                                        error: errors['paymentId'], touched: touched['paymentId']
                                    }}
                                        handlevaluechange={handleValueChange}
                                    />
                                </Col>
                                <Col xs='6'>
                                    <RWDatePicker {...{
                                        name: 'orderDate', label: 'Date',
                                        value: values['orderDate'], showDate: true, showTime: false,
                                        error: errors['orderDate'], touched: touched['orderDate']
                                    }}
                                        handlevaluechange={handleValueChange}
                                    />
                                </Col>
                            </Row>

                            <Row>
                                <Col xs="6">
                                   
                                     <RWDropdownList {...{
                                        name: 'vendorName', label: 'Vendor Name', valueField: 'id', textField: 'vendorName',
                                        value: values['vendorName'], type: 'string', values: branchs,
                                        error: errors['vendorName'], touched: touched['vendorName']
                                    }}
                                        handlevaluechange={handleValueChange}
                                    />
                                </Col>
                                <Col xs="6">
                                    <Input {...{
                                        name: 'registrationNo', label: 'Registration No',
                                        value: values['registrationNo'], type: 'string', disabled: false,
                                        error: errors['registrationNo'], touched: touched['registrationNo']
                                    }}
                                        handlevaluechange={handleValueChange}
                                    />
                                </Col>
                            </Row>
                            <Row>
                                <Col xs="6">
                                    <Input {...{
                                        name: 'gstId', label: 'GST Id',
                                        value: values['gstId'], type: 'string', disabled: false,
                                        error: errors['gstId'], touched: touched['gstId']
                                    }}
                                        handlevaluechange={handleValueChange}
                                    />
                                </Col>

                            </Row>

                        </CardBody>
                    </Card>
                </Col>

            </Row>
        </Fragment>
    )
}

export default VendorSummary
