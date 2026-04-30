import React, { Fragment, useEffect, useState } from 'react'
import { Button, Card, CardBody, Col, Row, Container, FormGroup, Label } from 'reactstrap';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { FieldError } from '../../../domain/Error/ErrorMessage'
import { RWDropdownList, Input, RWDatePicker, PhoneNumber, TextAreaInput } from '../../dynamicform/Controls';
import { DropdownList } from 'react-widgets';
import apiservice from 'services/apiservice';
import { Slide, toast } from 'react-toastify';
import { EmptyItem } from '../../../domain/Inventory/OrderReceived/ConstValues';
import { useParams } from 'react-router-dom';
import { URL } from '../../../domain/Inventory/URL';

function OrderReceive(props) {
    const { errors, touched, values, rid, vendors, isEdit } = props
    const [vendorOrder, setVendorOrder] = useState([]);
    let { module, id } = useParams();

    const moduleName = URL.find(ele => ele.value == module)

    const handleValueChange = async (name, value, { selected }) => {
        props.setFieldValue(name, value);
        if (name == 'vendorId') {
            const id = value.id
            props.setFieldValue('vendor', selected);
            fetchVendorOrders(value)
        }

        if (name == 'purchaseOrderId') {
            if (value) {
                //props.setFieldValue('lineItems', [EmptyItem()])
                props.IntialIndex()
                await apiservice.getAsync(`${moduleName.text}/PurchaseOrder/${value}`).then((result) => {
                    let poData = result.data;

                    poData.purchaseOrderId = value;
                    let formValues = {
                        purchaseOrderId: value,
                        orderDate: poData.orderDate,
                        no: '',
                        receivedDate: '',
                        vendorInvoiceNo: '',
                        vendorId: poData.vendorId,
                        vendor: poData.vendor,
                        creditDays: '',
                        status: 1,
                        comments: '',
                        lineItems: [],
                        totalInfo: {
                            totalPurchasePrice: 0,
                            totalFinalPrice: 0,
                            totalsellPrice: 0,
                            totalTax: 0
                        }
                    }
                    poData.lines.map((item, index) => {
                        const val = EmptyItem();
                        val.purchaseOrderLineId = item.id;
                        val.productId = item.productId;
                        val.product = item.product;
                        val.type = item.type;
                        val.kind = item.kind;
                        val.manufacturer = item.manufacturer;
                        val.units = item.units;
                        val.hsnCode = item.product.hsnCode;
                        val.rackNo = item.product.location;
                        formValues.lineItems[index] = val;
                    });
                    props.setValues(formValues);
                }).catch((error) => {
                    notifyError(error.message);
                });
            }
        }

    }


    useEffect(() => {
        if (rid && values.vendorId) {
            fetchVendorOrders(values.vendorId)
        }
    }, [])

    const fetchVendorOrders = async (id) => {

        if (id) {
            await apiservice.getAsync(`${moduleName.text}/PurchaseOrder/VendorOpenOrders/${id}`).then((response) => {
                const data = response.data;
                setVendorOrder(data);
            }).catch((error) => {
                notifyError(error.message);
            })
        } else {
            setVendorOrder([]);
        }


    }

    const notifyError = (msg) => toast(msg, {
        transition: Slide,
        closeButton: true,
        autoClose: 2500,
        position: 'bottom-center',
        type: 'error'
    });

    const onSelectionClear = async () => {
        await handleValueChange('vendorId', '', {})
    }

    const WithClearableSelection = (item, onClear) => {
        return (
            <React.Fragment>
                <span >{item.item['vendorName']}</span>
                <span><i onClick={onClear} className="pe-7s-close btnclear font-weight-bolder"> </i></span>
            </React.Fragment>
        );
    }
    return (
        <Fragment>
            <Row>
                <Col xs='4'>
                    <Row>
                        <Col sm={4}>
                            <Label className='mt-3'>Received No.</Label>
                        </Col>
                        <Col sm={6}>
                            {values.no}
                        </Col>
                    </Row>
                </Col>
                <Col xs='4'>
                    {/* While edit the OR displaying value in disabled */}
                    {isEdit ?
                        <Input {...{
                            name: 'vendor', label: 'Purchase Order', disabled: true,
                            value: values['poNo'], type: 'string',
                            error: errors['purchaseOrderId'], touched: touched['purchaseOrderId']
                        }} handlevaluechange={handleValueChange} />
                        :
                        <RWDropdownList {...{
                            name: 'purchaseOrderId', label: 'Purchase Order', valueField: 'id', textField: 'no',
                            value: values['purchaseOrderId'], type: 'string', values: vendorOrder, disabled: rid ? true : false,
                            error: errors['purchaseOrderId'], touched: touched['purchaseOrderId']
                        }} handlevaluechange={handleValueChange} />
                    }
                </Col>
                <Col xs="4">
                    <RWDatePicker {...{
                        name: 'orderDate', label: 'Purchase Order Date',
                        value: values['orderDate'], showDate: true, showTime: false, disabled: true,
                        error: errors['orderDate'], touched: touched['orderDate']
                    }} handlevaluechange={handleValueChange} />
                </Col>
            </Row>
            <Row>
                <Col xs="4">
                    {isEdit ?
                        <Input {...{
                            name: 'vendor', label: 'Vendor', disabled: true,
                            value: values.vendor.vendorName, type: 'string',
                            error: errors['vendorId'], touched: touched['vendorId']
                        }} handlevaluechange={handleValueChange} />
                        :
                        <RWDropdownList {...{
                            name: 'vendorId', label: 'Vendor', valueField: 'id', textField: 'vendorName',
                            value: values['vendorId'], values: vendors, disabled: rid ? true : false,
                            error: errors['vendorId'], touched: touched['vendorId']
                        }} handlevaluechange={handleValueChange} />}
                </Col>
                <Col xs="4">
                    <RWDatePicker {...{
                        name: 'receivedDate', label: 'Received Order Date',
                        value: values['receivedDate'], showDate: true, showTime: false,
                        error: errors['receivedDate'], touched: touched['receivedDate']
                    }} handlevaluechange={handleValueChange} />
                </Col>
                <Col xs="4">
                    <Input {...{
                        name: 'creditDays', label: 'Credit Period in days',
                        value: values['creditDays'], type: 'string',
                        error: errors['creditDays'], touched: touched['creditDays']
                    }} handlevaluechange={handleValueChange} />
                </Col>
            </Row>
            <Row>
                <Col xs="4">
                    <div>Vendor Details</div>
                    <div>MobileNo :{values.vendor ? values.vendor.vendorMobileno : ''}</div>
                    <div>Contact Person :{values.vendor ? values.vendor.contactPersonName : ''}</div>
                    <div>GSTNo : {values.vendor ? values.vendor.gstid : ''}</div>
                </Col>
                <Col xs="4">
                    <RWDropdownList {...{
                        name: 'status', label: 'Order Recevied Status', valueField: 'value', textField: 'text',
                        value: values['status'], type: 'string',
                        values: props.status,
                        error: errors['status'], touched: touched['status']
                    }} handlevaluechange={handleValueChange} />
                </Col>
                <Col xs="4">
                    <Input {...{
                        name: 'vendorInvoiceNo', label: 'Vendor Inoice No',
                        value: values['vendorInvoiceNo'], type: 'string',
                        error: errors['vendorInvoiceNo'], touched: touched['vendorInvoiceNo']
                    }} handlevaluechange={handleValueChange} />
                </Col>
            </Row>
        </Fragment>
    )
}

export default OrderReceive
