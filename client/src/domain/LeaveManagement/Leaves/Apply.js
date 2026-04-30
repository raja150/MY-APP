import { notifySaved } from 'components/alert/Toast';
import { CheckBox, Input, RWDatePicker, RWDropdownList, TextAreaInput } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import EmployeeSearch from 'domain/EmployeeSearch';
import { Form, Formik } from 'formik';
import PageHeader from 'Layout/AppMain/PageHeader';
import React, { Fragment, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { Button, Card, CardBody, Col, Label, Row } from 'reactstrap';
import ApplyLeavesService from 'services/Leave/ApplyLeave';
import LeaveTypeService from 'services/Leave/LeaveType';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import { APPROVAL_LEAVES } from '../navigation';

export default function ApplyLeave(props) {

    const [state, setState] = useState({ isLoading: true, formValues: {}, LeaveType: [], Employees: [], request: [] })
    const rid = props.rid ? props.rid : null;
    const [selEmp, setSelEmp] = useState({})
    let history = useHistory();

    useEffect(() => {
        let formValues = {}, request = {};
        const fetchData = async () => {
            let LeaveType = [];
            await LeaveTypeService.getLeaveType().then((response) => {

                LeaveType = response.data
            });
            if (rid) {
                await ApplyLeavesService.getLeaveById(rid).then((res) => {
                    formValues = res.data;
                    formValues.fromHalfDisplay = formValues.fromHalf ? '1' : '';
                    formValues.toHalfDisplay = formValues.toHalf ? '1' : '';
                    setSelEmp(formValues)
                });
            }
            else {
                formValues = {
                    leaveTypeId: '',
                    emergencyContNo: '',
                    fromDate: '',
                    toDate: '',
                    fromHalfDisplay: '',
                    toHalfDisplay: '',
                    // duration: '',
                    // durationFrame: '',
                    reason: '',
                    adminReason: '',
                    status: '',
                    employeeId: '',
                    diffDuration: '',
                    diffDurationFrame: '',
                }
            }
            setState({ ...state, isLoading: false, formValues: formValues, LeaveType: LeaveType, request: request })
        }
        fetchData()

    }, [])

    const validationSchema = Yup.object({
        leaveTypeId: Yup.string().required("Leave Type is required"),
        fromDate: Yup.string().required("From Date is required"),
        toDate: Yup.string().required("To Date is required"),
        employeeId: Yup.string().required("Employee Name is required"),
    });

    const handleSubmit = async (values, actions) => {
        const data = values;
        if (rid) {
            data['id'] = rid;
        }
        data.fromHalf = data.fromHalfDisplay === '1' ? true : false;
        data.toHalf = data.toHalfDisplay === '1' ? true : false;
        await ApplyLeavesService.LeavePostAsync(data).then((res) => {
            notifySaved();
            history.push(APPROVAL_LEAVES)
            window.location.reload()
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }

    return (
        <Fragment>
            {rid == null ? <PageHeader title="Apply Leave" /> : ""}
            <Card>
                <CardBody>
                    {state.isLoading ? <Loading /> :
                        <Formik
                            initialValues={state.formValues}
                            validationSchema={validationSchema}
                            onSubmit={(values, actions) => handleSubmit(values, actions)}>
                            {({ values, errors, touched, setFieldValue, isSubmitting, setValues }) => {
                                const handleValueChange = async (name, value) => {
                                    setFieldValue(name, value);
                                    if (name == 'duration') {
                                        if (value == 1) {
                                            setFieldValue('durationFrame', '')
                                        }
                                    }
                                }
                                return (
                                    <Form>
                                        <Row>
                                            <Col md='6'>
                                                <Label htmlFor='employee'>Employee Name</Label>
                                                <EmployeeSearch disabled={false} name={'employeeNo'} selEmp={selEmp} setSelEmp={setSelEmp} handleValueChange={handleValueChange} />
                                                <p style={{ color: 'red' }}>{errors.employeeId}</p>
                                            </Col>

                                            <Col md='6'>
                                                <RWDropdownList {...{
                                                    name: 'leaveTypeId', label: 'Leave Type', valueField: 'id', textField: 'code',
                                                    values: state.LeaveType, value: values['leaveTypeId'],
                                                    error: errors['leaveTypeId'], touched: touched['leaveTypeId'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col md='5'>
                                                <RWDatePicker {...{
                                                    name: 'fromDate', label: 'From Date', showDate: true,
                                                    value: values['fromDate'], showTime: false,
                                                    error: errors['fromDate'], touched: touched['fromDate'],

                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='1'>
                                                <CheckBox {...{
                                                    name: 'fromHalfDisplay', label: 'Half day', values: [{ value: 1, text: '' }],
                                                    value: values['fromHalfDisplay'], disabled: values.disabled,
                                                    error: errors['fromHalfDisplay'], touched: touched['fromHalfDisplay'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='5'>
                                                <RWDatePicker {...{
                                                    name: 'toDate', label: 'To Date',
                                                    value: values['toDate'], showTime: false,
                                                    error: errors['toDate'], touched: touched['toDate'],
                                                    showDate: true

                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='1'>
                                                <CheckBox {...{
                                                    name: 'toHalfDisplay', label: 'Half day', values: [{ value: 1, text: '' }],
                                                    value: values['toHalfDisplay'], disabled: values.disabled,
                                                    error: errors['toHalfDisplay'], touched: touched['toHalfDisplay'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'emergencyContNo', label: 'Emergency Contact No', type: 'string',
                                                    value: values['emergencyContNo'],
                                                    error: errors['emergencyContNo'], touched: touched['emergencyContNo'],

                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='6'>
                                                <TextAreaInput {...{
                                                    name: 'reason', label: 'Reason For Leave',
                                                    required: true, value: values['reason'],
                                                    error: errors['reason'], touched: touched['reason'],

                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <div className='d-flex'>
                                            <Button className="mb-2 mr-2 btn-icon btn-success1" key='button'
                                                color="success" type="submit" name="save" disabled={isSubmitting}>
                                                {isSubmitting ? "Please Wait..." : "Submit"}
                                            </Button>
                                        </div>
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
