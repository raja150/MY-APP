import { Button, Card, CardBody, Col, Row,Label } from 'reactstrap';
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
import EmployeeSearch from 'domain/EmployeeSearch';

export default function ApplyClientVisits(props) {
    const [state, setState] = useState({ isLoading: true, formValues: {}, })
    const [selEmp, setSelEmp] = useState({})
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;

    useEffect(() => {
        let formValues = {}
        const fetchData = async () => {
            if (rid) {
                await ApplyClientVisitsService.getById(rid).then((res) => {
                    formValues = res.data;
                    setSelEmp(formValues)
                });
            }
            else {
                formValues = {
                    employeeNo: '',
                    placeOfVisit: '',
                    fromDate: '',
                    toDate: '',
                    purposeOfVisit: '',
                    status: 0,
                }
            }
            setState({ ...state, isLoading: false, formValues: formValues })
        }
        fetchData()
    }, [])

    const validationSchema = Yup.object({
        employeeId: Yup.string().required("Employee Name is required"),
        placeOfVisit: Yup.string().required("Place of Visit is required"),
        fromDate: Yup.string().required("From Date is required"),
        toDate: Yup.string().required("To Date is required"),
        purposeOfVisit: Yup.string().required("Purpose of Visit is required"),
    });

    const handleSubmit = async (values, actions) => {
        const data = values;
        if(rid == null){
            data['employeeId'] = selEmp.id
        }
        if (rid) {
            data['employeeId'] = values.employeeId;
            data['id'] = rid;
        }
        await ApplyClientVisitsService.UpdateLMAsync(data).then((res) => {
            notifySaved();
            props.history.push(APPLY_CLIENT_VISITS);
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }
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
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'placeOfVisit', label: 'Place of Visit', disabled: values.status != 0 ? true : false,
                                                    value: values['placeOfVisit'],
                                                    error: errors['placeOfVisit'], touched: touched['placeOfVisit'],

                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>

                                        <Row>
                                            <Col md='6'>
                                                <RWDatePicker {...{
                                                    name: 'fromDate', label: 'From Date',
                                                    required: true, value: values['fromDate'],
                                                    error: errors['fromDate'], touched: touched['fromDate'],
                                                    showDate: true, showTime: false, disabled: values.status != 0 ? true : false,

                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='6'>
                                                <RWDatePicker {...{
                                                    name: 'toDate', label: 'To Date',
                                                    required: true, value: values['toDate'],
                                                    error: errors['toDate'], touched: touched['toDate'],
                                                    showDate: true, showTime: false, disabled: values.status != 0 ? true : false,

                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col md='6'>
                                                <TextAreaInput {...{
                                                    name: 'purposeOfVisit', label: 'Purpose of Visit',
                                                    value: values['purposeOfVisit'],
                                                    error: errors['purposeOfVisit'], touched: touched['purposeOfVisit'], disabled: values.status != 0 ? true : false,
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        {values.status == 0 ?
                                            <div className='d-flex'>
                                                <Button className="mb-2 mr-2 btn-icon btn-success1" key='button' color="success" type="submit" name="save" disabled={isSubmitting}  >
                                                    {isSubmitting ? "Please Wait..." : "Submit"}
                                                </Button>
                                            </div>
                                            : ''}
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
