import React, { Fragment } from 'react'
import { Button, Card, CardBody, Col, Row, Container, FormGroup, Label } from 'reactstrap';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { FieldError } from '../../../domain/Error/ErrorMessage'
import { RWDropdownList, Input, RWDatePicker, PhoneNumber } from '../../dynamicform/Controls';


function ExpireAlert(props) {
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
                                <RWDropdownList {...{
                                        name: 'branchId', label: 'Branch', valueField: 'id', textField: 'branchName',
                                        value: values['branchId'], type: 'string', values: branchs, 
                                        error: errors['branchId'], touched: touched['branchId']
                                    }} handlevaluechange={handleValueChange} />
                                </Col>
                                <Col xs='6'>
                                    <Input {...{
                                        name: 'no', label: 'Stock Id',
                                        value: values['no'], type: 'string', disabled: true,
                                        error: errors['no'], touched: touched['no']
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

export default ExpireAlert
