import { notifyError, notifySaved } from 'components/alert/Toast';
import { Input, Radio, RWDropdownList, TextAreaInput } from 'components/dynamicform/Controls';
import { Form, Formik } from 'formik';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Col, Row } from 'reactstrap';
import * as Yup from 'yup';
import WFHService from '../../../services/Approval/WFHService';
import SessionStorageService from 'services/SessionStorage'
function WFH(props) {
    const [state, setState] = useState({ visits: {}, emp: {}, isLoading: true, repotManager: [] });
    const rid = props.rid ? props.rid : null;

    useEffect(() => {
        setState({ ...state, isLoading: true })
        let visits = {}, emp = {}, repotManager = []
        const fetchData = async () => {
            await WFHService.GetAllEmp().then((result) => {
                repotManager = result.data;
            })
            if (rid) {
                await WFHService.getEmp(props.state.employeeId).then((result) => {
                    emp = result.data;
                })
            }
            setState({ ...state, visits: visits, emp: emp, isLoading: false, repotManager: repotManager })
        }
        fetchData()

    }, [])

    let formValues = {
        name: state.emp.name,
        approvedId: state.emp.reportingToId,
        approve: '',
        reason: ''
    }
    const handleSubmit = async (values) => {
        const data = {
            id: rid,
            adminReason: values.reason,
            employeeId: props.state.employeeId,
            approvedById: values.approvedId,
            fromCC: props.state.fromCC,
            toDateC: props.state.toDateC,
            toCC: props.state.toCC,
            shiftId: props.state.shiftId,
            emailIDC: props.state.emailIDC,
            fromDateC: props.state.fromDateC,
            ReasonForWFH: props.state.reasonForWFH,
            status: values.approve,
        }

        await WFHService.updateAsync(data).then((result) => {
            notifySaved();
        }).catch((error) => {
            notifyError(error.message);
        });

        props.toggle()
    }
    const validateYupSchema = Yup.object({
        approvedId: Yup.string().required("Approved By is required"),
        approve: Yup.string().required("Approved  is required"),
      
    })
    return (
        <Fragment>
            {state.isLoading ? '' :
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
                                    <Row>
                                        <Col md='6'>
                                            <Input {...{
                                                name: 'name', label: 'Employee ', disabled: true,
                                                value: values['name'],
                                                values: state.emp,
                                                error: errors['name'], touched: touched['name']
                                            }} handlevaluechange={handleValueChange} />
                                        </Col>
                                        <Col md='6'>
                                            <RWDropdownList {...{
                                                name: 'approvedId', label: 'Approved By', valueField: 'id', textField: 'name',
                                                value: values['approvedId'], values: state.repotManager,
                                                error: errors['approvedId'], touched: touched['approvedId']
                                            }} handlevaluechange={handleValueChange} />
                                        </Col>
                                    </Row>
                                    <Row>
                                        <Col md='6'>
                                            <Radio {...{
                                                name: 'approve', label: 'Do You Want Approve',
                                                value: values['approve'], values: [{ value: 1, text: 'Yes' }, { value: 2, text: 'No' }],
                                                touched: touched['approve'], error: errors['approve']
                                            }} handlevaluechange={handleValueChange} />
                                        </Col>
                                        {values.approve == 2 ?
                                            <Col md='6'>
                                                <TextAreaInput {...{
                                                    name: 'reason', label: 'Reason For Rejection',
                                                    value: values['reason'], values: ['reason']
                                                }} handlevaluechange={handleValueChange} />
                                            </Col> : ''}

                                    </Row>
                                    <Button type='submit' color='success'>Submit</Button>
                                </Fragment>
                            </Form>
                        )
                    }}
                </Formik>
            }
        </Fragment>
    )
}

export default WFH
