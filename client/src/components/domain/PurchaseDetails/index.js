import React, { Fragment } from 'react'
import { Button, Card, CardBody, Col, Row, Container, FormGroup, Label } from 'reactstrap';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { FieldError } from '../../../domain/Error/ErrorMessage'
import { RWDropdownList, Input, RWDatePicker, PhoneNumber } from '../../dynamicform/Controls';
import { DropdownList } from 'react-widgets';
import * as dateUtil from "../../../utils/date";


function PurchaseDetails(props) {

    const { errors, touched, values, vendors } = props
    const handleValueChange = async (name, value, { selected }) => {
        props.setFieldValue(name, value);
        if (name === 'vendorId') {
            props.setFieldValue('vendor', selected);
        }
    }

    return (
        <Fragment>
            <Row>
                <Col xs="4">
                    <Row>
                        <Col sm={4}>
                            <Label > Order No.</Label>
                        </Col>
                        <Col sm={6} >
                            <h6>{values['no']} </h6>
                        </Col>
                    </Row>
                </Col>
                <Col xs="4"></Col>
                <Col xs="4">
                    <Row>
                        <Col sm={4}>
                            <Label > Order Date</Label>
                        </Col>
                        <Col sm={6} >
                           
                            <td>{dateUtil.getTodayDate(values['orderDate'])}</td>
                        </Col>
                    </Row>
                </Col>
            </Row>
            <Row>
                <Col xs="4">
                    <RWDropdownList {...{
                        name: 'vendorId', label: 'Vendor', valueField: 'id', textField: 'vendorName', className: 'form-control',
                        value: values['vendorId'], type: 'string', values: vendors,
                        error: errors['vendorId'], touched: touched['vendorId']
                    }} handlevaluechange={handleValueChange} />
                </Col>

                <Col xs="4">
                    <RWDatePicker {...{
                        name: 'expectedDate', label: 'Expected Date',
                        value: values['expectedDate'], showDate: true, showTime: false,
                        error: errors['expectedDate'], touched: touched['expectedDate']
                    }} handlevaluechange={handleValueChange} />
                </Col>
                <Col xs="4">
                    <RWDropdownList {...{
                        name: 'status', label: 'Status', valueField: 'value', textField: 'text',
                        value: values['status'], type: 'string', values: props.status,
                        error: errors['status'], touched: touched['status'],
                    }} handlevaluechange={handleValueChange} />
                </Col>
            </Row>
            <Row>
                <Col xs="4">
                    <div>
                        GST No: {values.vendor ? values.vendor.gstid : ''}
                    </div>
                    <div>
                        Contact Person Name: {values.vendor ? values.vendor.contactPersonName : ''}
                    </div>
                    <div>
                        Vendor Mobile No: {values.vendor ? values.vendor.vendorMobileno : ''}
                    </div>
                </Col>
               
            </Row>  
        </Fragment>
    )
}

export default PurchaseDetails
