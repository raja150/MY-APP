import { notifySaved } from 'components/alert/Toast';
import { Radio, TextAreaInput } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import { Form, Formik } from 'formik';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Col, Row } from 'reactstrap';
import ClientVisitService from 'services/Approval/ClientVisitService'
import * as dateUtil from 'utils/date';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import InfoDisplay from '../InfoDisplay';


function ClientVisit(props) {
    const [state, setState] = useState({ visits: {}, isLoading: true });
    const [loading, setLoading] = useState(false);
    const rid = props.rid ? props.rid : null;
    useEffect(() => {
        setState({ ...state, isLoading: true })
        let visits = {}

        const fetchData = async () => {
            await ClientVisitService.getAsync(rid).then((result) => {
                visits = result.data;
            })
            setState({ ...state, visits: visits, isLoading: false })
        }
        fetchData()

    }, [])


    const validateYupSchema = Yup.object({
        status: Yup.string().required("Approved is required"),
        reason: Yup.string().when('status', {
            is: (val) => val == 2,
            then: Yup.string().required('Reason is required!'),
            otherwise: Yup.string().notRequired()
        })
    })

    let formValues = {
        name: props.state.employeeName,
        status: state.visits.status == 0 ? 1 : state.visits.status,
        reason: state.visits.rejectReason == null ? '' : state.visits.rejectReason
    }

    const handleSubmit = async (values, actions) => {
        setLoading(true)
        const data = {
            id: rid,
            isApproved: values.status == 1 ? true : false,
            rejectReason: values.reason,
        }
        await ClientVisitService.updateAsync(data).then((result) => {
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
                validationSchema={validateYupSchema}
                onSubmit={(values) => handleSubmit(values)}
            >
                {({ values, errors, touched, setFieldValue }) => {
                    const handleValueChange = (name, value) => {
                        setFieldValue(name, value)
                    }
                    return (
                        <Form>
                            <Fragment>
                                <InfoDisplay label="Employee Code" info={state.visits.employeeNo} />
                                <InfoDisplay label="Employee Name" info={state.visits.employeeName} />
                                <InfoDisplay label="Designation" info={state.visits.designation} />
                                <InfoDisplay label="Place of visit" info={state.visits.placeOfVisit} />
                                <InfoDisplay label="Purpose of visit" info={state.visits.purposeOfVisit} />
                                <InfoDisplay label="Visit From-To" info={dateUtil.getDate(state.visits.fromDate).concat(' To ', dateUtil.getDate(state.visits.toDate))} />

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
                                {state.visits.status == 0 || values.status == 2 ?
                                    <Button type='submit' disabled={loading || state.visits.status == 2} color='success'>{loading ? 'Please wait' : 'Submit'}</Button> : ''}
                            </Fragment>
                        </Form>
                    )
                }}

            </Formik>
    )
}

export default ClientVisit
