import { Radio, TextAreaInput } from 'components/dynamicform/Controls';
import InfoDisplay from 'domain/Approvals/InfoDisplay';
import { Form, Formik } from 'formik';
import React, { Fragment, useState } from 'react';
import { Button, Col, Row, Label, Modal, ModalBody, } from 'reactstrap';
import * as dateUtil from 'utils/date';
import * as Yup from 'yup';
import * as _ from 'lodash'
import { actions } from 'react-table';
import { getCamelCaseObject } from 'utils/form'

//This is Popup Screen
function ApproveOrReject(props) {
    const [modal, setModal] = useState(false);
    const formValues = {
        name: props.data.name,
        status: props.data.status == 1 ? 2 : props.data.status,
        reason: props.data.rejectReason == null ? '' : props.data.rejectReason,
        canApprove: props.data.status == 1,
    }
    const validateYupSchema = Yup.object({
        status: Yup.string().required("Approved is required"),
        reason: Yup.string().when('status', {
            is: (val) => val == 3,
            then: Yup.string()
                .required('Reason is required!'),
            otherwise: Yup.string().notRequired()

        })
    })
    var leaveTypes = [];
    if (props.data.leaveTypes) {
        leaveTypes = JSON.parse(props.data.leaveTypes)
    }
    const handleSubmit = async (values, actions) => {
        actions.setSubmitting(true);
        if (values.status == 2) {
            setModal(true)
        }
        else {
            const data = {
                id: props.data.id,
                isApproved: false,
                rejectReason: values.reason,
                adminReason: values.reason,
            }
            await props.handleSubmit(data, actions);

            actions.setSubmitting(false);
        }

    }
    const handleApprove = async (values, actions) => {
        const data = {
            id: props.data.id,
            isApproved: true,
            adminReason: values.reason,
        }
        await props.handleSubmit(data, actions);
        setModal(false)
    }
    const toggle = () => setModal(!modal);

    const handleReject = async (values) => {
        const data = {
            id: props.data.id,
            isApproved: false,
            rejectReason: values.reason,
            adminReason: values.reason,
            employeeId: props.data.employeeId,
        }
        await props.handleReject(data);
        setModal(false)
    }
    return (
        <Formik
            initialValues={formValues}
            validationSchema={validateYupSchema}
            onSubmit={(values, actions) => handleSubmit(values, actions)}
        >
            {({ values, errors, touched, setFieldValue, isSubmitting }) => {
                const handleValueChange = (name, value) => {
                    setFieldValue(name, value)
                }
                return (
                    <Form>
                        <Fragment>
                            <InfoDisplay label="Employee Code" info={props.data.employeeNo} />
                            <InfoDisplay label="Employee Name" info={props.data.employeeName} />
                            <InfoDisplay label="Designation" info={props.data.designation} />
                            {/* {props.applied != null ? <InfoDisplay label="Leave Type" info={props.data.leaveType} /> : ''} */}
                            {props.data.leaveTypes && <Row>
                                <Col md='5'>
                                    <h6>{'Leave Type - Day(s)'}</h6>
                                </Col>
                                <Col md='1'>
                                    <h6>:</h6>
                                </Col>
                                <Col md='6'>
                                    {leaveTypes.map((leaveType, index) => {
                                        leaveType = getCamelCaseObject(leaveType)
                                        return (
                                            <div style={{ fontSize: '12px' }}>
                                                <span>{leaveType.name} : {leaveType.days} Day(s)</span><br />
                                            </div>)
                                    })}
                                </Col>
                            </Row>}
                            {props.data.noOfDays &&
                                <Row>
                                    <Col md='5'>
                                        <h6>{'Day(s)'}</h6>
                                    </Col>
                                    <Col md='1'>
                                        <h6>:</h6>
                                    </Col>
                                    <Col md='6'>
                                        {props.data.noOfDays}
                                    </Col>
                                </Row>
                            }
                            <InfoDisplay label="Reason" info={props.data.reason} />
                            {/* {props.applied != null && props.applied.length > 0 && <h5 style={{ color: 'InfoText' }}>Applied</h5>}
                            {props.data.leaveTypes == null && props.applied != null ? <InfoDisplay label={props.name + " From-To"} info={dateUtil.getDate(props.data.fromDate).concat(' To ', dateUtil.getDate(props.data.toDate))} />
                                : props.applied != null && props.applied.map((val, index) => {
                                    return (
                                        <InfoDisplay label="Leave From-To" info={dateUtil.getDate(val.fromDate).concat(' To ', dateUtil.getDate(val.toDate))} />
                                    )
                                })} */}
                            <h5 style={{ color: 'InfoText' }}>Applied</h5>
                            <InfoDisplay label={props.name + " From-To"} info={dateUtil.getDate(props.data.fromDate).concat(' To ', dateUtil.getDate(props.data.toDate))} />
                            <Row>
                                <Col md='6'>
                                    <Radio {...{
                                        name: 'status', label: 'Do you want approve',
                                        value: values['status'], values: [{ value: 2, text: 'Yes' }, { value: 3, text: 'No' }],
                                        touched: touched['status'], error: errors['status'], disabled: !values.canApprove && props.applied == null
                                    }} handlevaluechange={handleValueChange} />
                                </Col>
                            </Row>
                            <Row>
                                <Col md='12'>
                                    {values.status == 3 ? <TextAreaInput {...{
                                        name: 'reason', label: 'Reason For Rejection', error: errors['reason'], touched: touched['reason'],
                                        value: values['reason'], //disabled: !values.canApprove
                                    }} handlevaluechange={handleValueChange} /> : ''}
                                </Col>
                            </Row>
                            {values.canApprove ?
                                <Row >
                                    <Col md='3'>
                                        <Button type='submit' disabled={isSubmitting} color='success'>
                                            {isSubmitting ? 'Please wait' : values.status == 3 ? "Reject" : "Approve"}
                                        </Button>
                                    </Col>
                                    {props.handleMore ?
                                        <Col style={{ marginTop: '10px' }}>
                                            <a href='#' onClick={(e) => props.handleMore(e)}>Advanced view</a>
                                        </Col> : ''}
                                </Row>
                                : values.status == 3 && props.applied != null ?
                                    <Button type='button' disabled={isSubmitting || props.request.status == 3} onClick={() => handleReject(values)} color='success'>
                                        {isSubmitting ? 'Please wait' : values.status == 3 ? "Reject" : ""}
                                    </Button>
                                    : ''}
                            {props.applied != null && props.approved.length > 0 && <h5 style={{ color: 'green', marginTop: '10px', marginBottom: '10px' }}>Approved</h5>}
                            {props.data.leaveTypes == null ? props.approved.map((val, index) => {
                                return (
                                    <InfoDisplay label={props.name + " From-To"} info={dateUtil.getDate(val.fromDate).concat(' To ', dateUtil.getDate(val.toDate))} />
                                )
                            }) : props.applied != null && props.approved.map((val, index) => {
                                return (
                                    <InfoDisplay label={props.name + " From-To"} info={dateUtil.getDate(val.fromDate).concat(' To ', dateUtil.getDate(val.toDate))} />
                                )
                            })}
                            {values.status != 3 ?
                                <Modal isOpen={modal} toggle={toggle}>
                                    <ModalBody>
                                        <Col ms="4">
                                            <Label><h6>Are you sure want to Approve?</h6></Label>
                                        </Col>
                                        <Row>
                                            <div style={{ paddingLeft: '30px' }} >
                                                <Button type="submit" color="success" onClick={() => handleApprove(values, actions)}>
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
                        </Fragment>
                    </Form>
                )
            }}

        </Formik>
    )
}

export default ApproveOrReject
