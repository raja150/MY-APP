import { notifySaved } from 'components/alert/Toast';
import { Input, RWDatePicker, CheckBox, RWDropdownList, TextAreaInput } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import { compareAsc } from 'date-fns';
import { Form, Formik } from 'formik';
import PageHeader from 'Layout/AppMain/PageHeader';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Label, Modal, ModalBody, Row } from 'reactstrap';
import WorkFromHomeService from 'services/Leave/WorkFromHome';
import * as crypto from 'utils/Crypto';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import { APPLY_WFH } from '../navigation';
import * as compare from 'utils/Compare'

export default function ApplyWFH(props) {
    const [state, setState] = useState({ isLoading: true, formValues: {} })
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    const [modal, setModal] = useState(false);
    useEffect(() => {
        let formValues = {}
        const fetchData = async () => {

            if (rid) {
                await WorkFromHomeService.getById(rid).then((res) => {
                    formValues = res.data;
                    formValues.disabled = res.data.status === 1 ? false : true;
                });

            }
            else {
                formValues = {
                    fromDateC: '',
                    toDateC: '',
                    reasonForWFH: '',
                    status: 1,
                    adminReason: '',
                    disabled: false,
                    fromHalf: '',
                    toHalf: ''
                }
            }
            setState({ ...state, isLoading: false, formValues: formValues })
        }
        fetchData()
    }, [])

    const handleCancel = async () => {
        setModal(true)
    }
    const handleSubmit = async (values, actions) => {
        const data = values;
        if (rid) {
            data['id'] = rid;
        }
        await WorkFromHomeService.UpdateAsync(data).then((res) => {
            notifySaved();
            props.history.push(APPLY_WFH);
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }
    const handleCancelled = async () => {
        await WorkFromHomeService.SelfServiceCancelAsync(rid).then((res) => {
            notifySaved();
            props.history.push(APPLY_WFH);
        }).catch((err) => {

            formUtil.displayErrors(err)
        })
    }
    const toggle = () => setModal(!modal);

    const validationSchema = Yup.object({
        fromDateC: Yup.string().required("From Date is required"),
        toDateC: Yup.string().required("To Date is required"),
        reasonForWFH: Yup.string().required("Reason For work from Home is required"),
    });
    return (
        <Fragment>
            <PageHeader title="Apply Work From Home" />
            <Card>
                <CardBody>
                    {state.isLoading ? <Loading /> :
                        <Formik
                            initialValues={state.formValues}
                            validationSchema={validationSchema}
                            onSubmit={(values, actions) => handleSubmit(values, actions)}>
                            {({ values, errors, touched, setFieldValue, isSubmitting, setValues }) => {
                                const handleValueChange = async (name, value, { selected }) => {
                                    setFieldValue(name, value);
                                }
                                return (
                                    <Form>
                                        <Row>
                                            <Col md='5'>
                                                <RWDatePicker {...{
                                                    name: 'fromDateC', label: 'From Date', showDate: true, showTime: false,
                                                    required: true, value: values['fromDateC'], disabled: values.disabled,
                                                    error: errors['fromDateC'], touched: touched['fromDateC'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='1'>
                                                <CheckBox {...{
                                                    name: 'fromHalf', label: 'Half day', values: [{ value: true, text: '' }],
                                                    value: values['fromHalf'], disabled: values.disabled,
                                                    error: errors['fromHalf'], touched: touched['fromHalf'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='5'>
                                                <RWDatePicker {...{
                                                    name: 'toDateC', label: 'To Date', showDate: true, showTime: false,
                                                    required: true, value: values['toDateC'], disabled: values.disabled,
                                                    error: errors['toDateC'], touched: touched['toDateC'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='1'>
                                                <CheckBox {...{
                                                    name: 'toHalf', label: 'Half day', values: [{ value: true, text: '' }],
                                                    value: values['toHalf'], disabled: values.disabled,
                                                    error: errors['toHalf'], touched: touched['toHalf'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col md='6'>
                                                <TextAreaInput {...{
                                                    name: 'reasonForWFH', label: 'Reason For work from Home',
                                                    value: values['reasonForWFH'], disabled: values.disabled,
                                                    error: errors['reasonForWFH'], touched: touched['reasonForWFH'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        {values.disabled ? '' : <Fragment>
                                            <Row>
                                                <Col md='2.5'>
                                                    <Button style={{ marginTop: '19px' }} color="success" key='button' type="submit" name="save"
                                                        disabled={isSubmitting} handlesubmit={handleSubmit}>
                                                        {isSubmitting ? "Please Wait..." : (values.id ? "Update" : "Apply")}
                                                    </Button>
                                                </Col>
                                                <Col>
                                                    {values.id ?
                                                        <Button style={{ marginTop: '19px' }} color="primary" key='button' name="cancel"
                                                            disabled={isSubmitting} onClick={handleCancel}>
                                                            {isSubmitting ? "Please Wait..." : "Cancel"}
                                                        </Button> : ''}
                                                    {values.id ?
                                                        <Modal isOpen={modal} toggle={toggle}>
                                                            <ModalBody>
                                                                <Col ms="4">
                                                                    <Label><h6>Are you sure want to cancel your WFH request?</h6></Label>
                                                                </Col>
                                                                <Row>
                                                                    <div style={{ paddingLeft: '30px' }} >
                                                                        <Button type="button" color="success" onClick={() => handleCancelled()}>
                                                                            Yes
                                                                        </Button>
                                                                    </div>
                                                                    <div style={{ paddingLeft: '10px' }} >
                                                                        <Button color="danger" type="button" onClick={() => setModal(!modal)} >
                                                                            No
                                                                        </Button>
                                                                    </div>
                                                                </Row>
                                                            </ModalBody>
                                                        </Modal> : ''}
                                                </Col>
                                            </Row>
                                        </Fragment>}
                                    </Form>
                                )
                            }}
                        </Formik>
                    }
                </CardBody>
            </Card>
        </Fragment>
    )
}
