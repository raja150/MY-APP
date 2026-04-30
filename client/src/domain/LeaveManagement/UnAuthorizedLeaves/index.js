import { notifySaved } from 'components/alert/Toast';
import { Input, RWDatePicker, RWDropdownList } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import { Form, Formik } from 'formik';
import PageHeader from 'Layout/AppMain/PageHeader';
import moment from 'moment';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Label, Row } from 'reactstrap';
import UnAuthorizedLeavesService from 'services/Org/UnAtuthorizedLeaves';
import * as crypto from 'utils/Crypto';
import * as formUtil from 'utils/form';
import { UNAUTHORIZED_LEAVES } from '../navigation';
import * as Yup from 'yup';
import EmployeeTeamSearch from 'domain/EmployeeSearch/EmployeeTeamSearch';
import EmployeeSearch from 'domain/EmployeeSearch';



export default function UnAuthorizedLeavesForm(props) {
    const [state, setState] = useState({ isLoading: true, formValues: {} })
    const [selEmp, setSelEmp] = useState({})

    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    const todayDate = moment().format('YYYY-MM-D')
    console.log(rid)
    useEffect(() => {
        let formValues = {}
        const fetchData = async () => {
            if (rid) {
                await UnAuthorizedLeavesService.getUnAuthorizedLeavesById(rid).then((res) => {
                    formValues = res.data
                    setSelEmp(formValues)
                });
            }
            else {
                formValues = {
                    date: todayDate,
                    leaveStatus: 1,
                    departmentId: '',
                    designationId: '',
                }
            }
            setState({ ...state, formValues: formValues, isLoading: false })
        }
        fetchData();
    }, [])

    const handleSubmit = async (values, actions) => {
        const data = values;
        console.log(data)
        if (rid) {
            data['unAuthorizedLeavesId'] = rid;
        }
            await UnAuthorizedLeavesService.AddUnAuthorizedLeaves(data).then((res) => {
                notifySaved();
                props.history.push(UNAUTHORIZED_LEAVES);
            }).catch((err) => {
                formUtil.displayFormikError(err, actions)
            })
    }
    const handleCancel = () => {
        props.history.push(UNAUTHORIZED_LEAVES);
    }
    const validationSchema = Yup.object({
        employeeId: Yup.string().required("Employee Name is required"),
        leaveStatus: Yup.string().required('Leave Status is required'),
    });

    return <>
        {state.isLoading ? <Loading /> :
            <Fragment>
                {rid == null ? <PageHeader title="Un-Authorized Leaves" /> : ""}
                <Card>
                    <CardBody>
                        <Formik
                            initialValues={state.formValues}
                            validationSchema={validationSchema}
                            onSubmit={(values, actions) => handleSubmit(values, actions)}
                        >
                            {({ values, errors, touched, setFieldValue, isSubmitting, setValues }) => {
                                console.log(values)
                                const handleValueChange = async (name, value, { selected }) => {
                                    setFieldValue(name, value);
                                }
                                return (

                                    <Form>
                                        <Row>
                                            <Col md='4'>
                                                <Label htmlFor='employee'>Employee Name</Label>
                                                <EmployeeTeamSearch disabled={false} name={'employeeNo'} selEmp={selEmp} setSelEmp={setSelEmp} handleValueChange={handleValueChange} />
                                                <p style={{ color: 'red' }}>{errors.employeeId}</p>
                                            </Col>

                                            <Col md='4'>
                                                <RWDatePicker {...{
                                                    name: 'date', label: 'Select Date', showDate: true,
                                                    value: values['date'], showTime: false,
                                                    error: errors['date'],

                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            {rid != null &&
                                                <Col xs="4">
                                                    <RWDropdownList {...{
                                                        name: 'leaveStatus', label: 'Leave Status',
                                                        value: values['leaveStatus'], values: [{ value: 1, text: 'Un-Authorized' }, { value: 2, text: 'Authorized' }],
                                                        error: errors['leaveStatus'], touched: touched['leaveStatus']
                                                    }} handlevaluechange={handleValueChange} />
                                                </Col>
                                            }

                                        </Row>
                                        <Row>
                                            <Col xs="4">
                                                <Input {...{
                                                    name: 'departmentId', label: 'Department', disabled: selEmp.department ? true : false,
                                                    value: selEmp.department,
                                                    error: errors['departmentId'], touched: touched['departmentId']
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col xs="4">
                                                <Input {...{
                                                    name: 'designationId', label: 'Designation', disabled: selEmp.designation ? true : false,
                                                    value: selEmp.designation,
                                                    error: errors['designationId'], touched: touched['designationId']
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <Row className='justify-content-center'>

                                            <Button style={{ marginTop: '19px' }} className="mb-2 mr-2 btn-icon btn-success" key='Save' color="success" type="submit" name="Submit" disabled={isSubmitting}>
                                                {isSubmitting ? "Please Wait..." : "Submit"}
                                            </Button>
                                            <Button style={{ marginTop: '19px' }} className="mb-2 mr-2 btn-icon btn-primary" key='Cancel' color="success" type="button" onClick={() => handleCancel()} name="Cancel" >
                                                {"Cancel"}
                                            </Button>
                                        </Row>
                                    </Form>
                                )
                            }}
                        </Formik>
                    </CardBody>
                </Card>
            </Fragment>
        }
    </>
}