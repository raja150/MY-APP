import { notifySaved } from 'components/alert/Toast';
import { RWDatePicker, TextAreaInput, CheckBox } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import EmployeeSearch from 'domain/EmployeeSearch';
import { Form, Formik } from 'formik';
import PageHeader from 'Layout/AppMain/PageHeader';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Label, Row } from 'reactstrap';
import ShiftService from 'services/Leave/Shift';
import WorkFromHomeService from 'services/Leave/WorkFromHome';
import * as crypto from 'utils/Crypto';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';

export default function ApplyWFH(props) {
    const [state, setState] = useState({ isLoading: true, formValues: {}, shiftType: [] })
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    const [selEmp, setSelEmp] = useState({})
    const [isSearching, setIsSearching] = useState(false);
    useEffect(() => {
        let formValues = {}
        let shiftType = [];
        const fetchData = async () => {
            await ShiftService.getList().then((res) => {
                shiftType = res.data;
            })
            if (rid) {
                await WorkFromHomeService.getById(rid).then((res) => {
                    formValues = res.data;
                    setSelEmp(formValues)
                });
            }
            else {
                formValues = {
                    fromDateC: '',
                    toDateC: '',
                    reasonForWFH: '',
                    status: '',
                    adminReason: '',
                    employeeNo: '',
                    fromHalf: '',
                    toHalf: ''
                }
            }
            setState({ ...state, isLoading: false, formValues: formValues, shiftType: shiftType })
        }
        fetchData()
    }, [])


    const validationSchema = Yup.object({
        fromDateC: Yup.string().required("From Date is required"),
        toDateC: Yup.string().required("To Date is required"),
        reasonForWFH: Yup.string().required("Reason For work from Home is required"),
        employeeId: Yup.string().required("Employee Name is required"),
    });


    const handleSubmit = async (values, actions) => {

        const data = values;
        if (rid == null) {
            data['employeeId'] = selEmp.id
        }

        if (rid) {
            data['employeeId'] = values.employeeId;
            data['id'] = rid;
        }
        await WorkFromHomeService.UpdateLMAsync(data).then((res) => {
            notifySaved();
            props.history.push('/m/Leave/WFH');
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }
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
                                const handleValueChange = async (name, value) => {
                                    if (name == 'employeeNo') {
                                        setFieldValue('employeeId', value.id);
                                    }
                                    setFieldValue(name, value);
                                }
                                return (
                                    <Form>
                                        <Row>
                                            <Col md='6'>
                                                <Label htmlFor='employee'>Employee Name</Label>
                                                <EmployeeSearch disabled={false} name={'employeeNo'} selEmp={selEmp} setSelEmp={setSelEmp} handleValueChange={handleValueChange} />

                                                <p style={{ color: 'red' }}>{errors.employeeNo}</p>
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col md='5'>
                                                <RWDatePicker {...{
                                                    name: 'fromDateC', label: 'From Date', showDate: true, showTime: false,
                                                    required: true, value: values['fromDateC'],
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
                                                    required: true, value: values['toDateC'],
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
                                                    value: values['reasonForWFH'],
                                                    error: errors['reasonForWFH'], touched: touched['reasonForWFH'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <div className='d-flex'>
                                            <Button className="mb-2 mr-2 btn-icon btn-success1" key='button' color="success" type="submit" name="save" disabled={isSubmitting}>
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
