import { Button, Card, CardBody, Col, Row, Label, Modal, ModalBody } from 'reactstrap';
import React, { Fragment, useEffect, useState } from 'react';
import queryString from 'query-string';
import * as Yup from 'yup';
import Loading from 'components/Loader';
import { Formik, Form } from 'formik';
import { Input, RWDatePicker, TextAreaInput } from 'components/dynamicform/Controls';
import PageHeader from 'Layout/AppMain/PageHeader';
import * as formUtil from 'utils/form';
import { notifySaved } from 'components/alert/Toast';
import ApplyClientVisitsService from 'services/Leave/ApplyClientVisits'
import * as crypto from 'utils/Crypto';
import { APPLY_CLIENT_VISITS } from '../navigation';

export default function ApplyClientVisits(props) {
    const [state, setState] = useState({ isLoading: true, formValues: {}, })
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    const [modal, setModal] = useState(false);

    useEffect(() => {
        let formValues = {}
        const fetchData = async () => {
            if (rid) {
                await ApplyClientVisitsService.getById(rid).then((res) => {
                    formValues = res.data;
                    formValues.disabled = res.data.status === 1 ? false : true;
                });
            }
            else {
                formValues = {
                    placeOfVisit: '',
                    fromDate: '',
                    toDate: '',
                    purposeOfVisit: '',
                    status: 1,
                    disabled: false,
                }
            }
            setState({ ...state, isLoading: false, formValues: formValues })
        }
        fetchData()
    }, [])

    const validationSchema = Yup.object({
        placeOfVisit: Yup.string().required("Place of Visit is required"),
        fromDate: Yup.string().required("From Date is required"),
        toDate: Yup.string().required("To Date is required"),
        purposeOfVisit: Yup.string().required("Purpose of Visit is required"),
    });

    const handleSubmit = async (values, actions) => {
        const data = values;
        if (rid) {
            data['id'] = rid;
        }
        await ApplyClientVisitsService.UpdateAsync(data).then((res) => {
            notifySaved();
            props.history.push(APPLY_CLIENT_VISITS);
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }
    const handleCancel = async () => {
        setModal(true)
    }
    const handleCancelled = async () => {
        await ApplyClientVisitsService.SelfServiceCancelAsync(rid).then((res) => {
            notifySaved();
            props.history.push(APPLY_CLIENT_VISITS);
        }).catch((err) => {

            formUtil.displayErrors(err)
        })
    }
    const toggle = () => setModal(!modal);

    return (
        <Fragment>
            <PageHeader title="Apply Client Visits" />
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
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'placeOfVisit', label: 'Place of Visit', disabled: values.disabled,
                                                    value: values['placeOfVisit'],
                                                    error: errors['placeOfVisit'], touched: touched['placeOfVisit'],

                                                }} handlevaluechange={handleValueChange} />
                                            </Col>

                                            <Col md='6'>
                                                <RWDatePicker {...{
                                                    name: 'fromDate', label: 'From Date',
                                                    required: true, value: values['fromDate'],
                                                    error: errors['fromDate'], touched: touched['fromDate'],
                                                    showDate: true, showTime: false, disabled: values.disabled,

                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>

                                        <Row>
                                            <Col md='6'>
                                                <RWDatePicker {...{
                                                    name: 'toDate', label: 'To Date',
                                                    required: true, value: values['toDate'],
                                                    error: errors['toDate'], touched: touched['toDate'],
                                                    showDate: true, showTime: false, disabled: values.disabled,

                                                }} handlevaluechange={handleValueChange} />
                                            </Col>

                                            <Col md='6'>
                                                <TextAreaInput {...{
                                                    name: 'purposeOfVisit', label: 'Purpose of Visit',
                                                    value: values['purposeOfVisit'],
                                                    error: errors['purposeOfVisit'], touched: touched['purposeOfVisit'], disabled: values.disabled,
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        {/* {values.status == 1 ?
                                            <div className='d-flex'>
                                                <Button className="mb-2 mr-2 btn-icon btn-success1" key='button' color="success" type="submit" name="save" disabled={isSubmitting}  >
                                                    {isSubmitting ? "Please Wait..." : "Submit"}
                                                </Button>
                                            </div>
                                            : ''} */}
                                        {values.disabled ? '' : <Fragment>
                                            <Row>
                                                <Col md='2.5'>
                                                    <Button style={{ marginTop: '19px', marginLeft:'15px'}} color="success" key='button' type="submit" name="save"
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
                                                                    <Label><h6>Are you sure want to cancel your client visit request?</h6></Label>
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
