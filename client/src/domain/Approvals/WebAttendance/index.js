import { notifySaved } from 'components/alert/Toast';
import { Radio, TextAreaInput } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import { Form, Formik } from 'formik';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Col, Row } from 'reactstrap';
import * as dateUtil from 'utils/date';
import * as formUtil from 'utils/form';
import InfoDisplay from '../InfoDisplay';
import WebAttendanceService from 'services/SelfService/WebAttendance'


function WebAttendance(props) {
    const [state, setState] = useState({ webAttendance: {}, isLoading: true });
    const [loading, setLoading] = useState(false);
    const rid = props.rid ? props.rid : null;

    useEffect(() => {
        setState({ ...state, isLoading: true })
        let webAttendance = {}

        const fetchData = async () => {
            await WebAttendanceService.gerById(rid).then((result) => {
                webAttendance = result.data;
            })
            setState({ ...state, webAttendance: webAttendance, isLoading: false })
        }
        fetchData()

    }, [])
    let formValues = {
        name: props.state.employeeName,
        status: state.webAttendance.status == 2 ? 1
            : state.webAttendance.status == 3 ? 2 : state.webAttendance.status,
        reason: state.webAttendance.rejectReason == null ? '' : state.webAttendance.rejectReason
    }

    const handleSubmit = async (values, actions) => {
        setLoading(true)
        const data = {
            id: rid,
            employeeId: state.webAttendance.employeeId,
            isApproved: values.status == 1 ? true : false,
            rejectReason: values.reason,
        }
        if (rid) {
            data['attendanceDate'] = state.webAttendance.attendanceDate
        }
        await WebAttendanceService.Approve(data).then((result) => {
            notifySaved();
            props.toggle();
            props.handleSearch(0)
        }).catch((error) => {
            formUtil.displayErrors(error)
        })
        setLoading(false)
    }
    return (
        state.isLoading ? <Loading /> :
            <Formik
                initialValues={formValues}
                onSubmit={(values) => handleSubmit(values)}
            >
                {({ values, errors, touched, setFieldValue }) => {
                    const handleValueChange = (name, value) => {
                        setFieldValue(name, value)
                    }
                    return (
                        <Form>
                            <Fragment>
                                <InfoDisplay label="Employee Code" info={state.webAttendance.employeeNo} />
                                <InfoDisplay label="Employee Name" info={state.webAttendance.employeeName} />
                                <InfoDisplay label="Designation" info={state.webAttendance.designation} />
                                <InfoDisplay label="Attendance Date" info={dateUtil.getDate(state.webAttendance.attendanceDate)} />
                                <InfoDisplay label="InTime" info={dateUtil.getTime(state.webAttendance.inTime)} />
                                <InfoDisplay label="OutTime" info={dateUtil.getTime(state.webAttendance.outTime)} />

                                <Row>
                                    <Col md='6'>
                                        <Radio {...{
                                            name: 'status', label: 'Do You Want Approve',
                                            value: values['status'], values: [{ value: 1, text: 'Yes' }, { value: 2, text: 'No' }],
                                            touched: touched['status'], error: errors['status']
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                </Row>
                                <Row>
                                    {values.status == 2 ?
                                        <Col md='12'>
                                            <TextAreaInput {...{
                                                name: 'reason', label: 'Reason For Rejection',
                                                value: values['reason'], values: ['reason'], touched: touched['reason'], error: errors['reason']
                                            }} handlevaluechange={handleValueChange} />
                                        </Col> : ''}
                                </Row>
                                {state.webAttendance.status == 1 || values.status == 2 ?
                                    <Button type='submit' disabled={loading || state.webAttendance.status == 3} color='success'>{loading ? 'Please wait' : values.status == 2 ? "Reject" : "Approve"}</Button> : ''}
                            </Fragment>
                        </Form>
                    )
                }}

            </Formik>
    )
}

export default WebAttendance
