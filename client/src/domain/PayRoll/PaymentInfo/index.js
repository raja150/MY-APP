import { notifySaved } from 'components/alert/Toast';
import { Input, Radio, RWDropdownList } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import EmployeeSearch from 'domain/EmployeeSearch';
import { Form, Formik } from 'formik';
import PageHeader from 'Layout/AppMain/PageHeader';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Label, Row } from 'reactstrap';
import BankService from 'services/PayRoll/BankService';
import PaymentInfoService from 'services/PayRoll/PaymentInfoService';
import * as crypto from 'utils/Crypto';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import { PAYMENT_INFO } from '../navigation';
import * as compare from 'utils/Compare'


export default function PaymentInfo(props) {
    const [state, setState] = useState({ isLoading: true, formValues: {}, banks: [], Employees: [] })
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;

    const [selEmp, setSelEmp] = useState({})
    useEffect(() => {
        let formValues = {}
        const fetchData = async () => {
            let banks = [];
            await BankService.getList().then((response) => {
                banks = response.data
            });
            if (rid) {
                await PaymentInfoService.getById(rid).then((res) => {
                    formValues = res.data;
                    setSelEmp(formValues)
                });
            }
            else {
                formValues = {
                    employeeId: '',
                    name: '',
                    no: '',
                    bankId: '',
                    payMode: '',
                    bankName: '',
                    ifscCode: '',
                    accountNoVerify: '',
                    accountNo: ''
                }
            }
            setState({ ...state, isLoading: false, formValues: formValues, banks: banks })
        }
        fetchData()
    }, [])

    const validationSchema = Yup.object({
        employeeId: Yup.string().required('Employee Name is required!'),
        payMode: Yup.string().required('How Would You Like To Pay is required!'),
        bankId: Yup.string().nullable().required('Bank is required!'),
        bankName: Yup.string().nullable().when('payMode', { is: val => (val == 2), then: Yup.string().required('Bank Name is required!') }),
        ifscCode: Yup.string().nullable().when('payMode', { is: val => (val == 2), then: Yup.string().required('IFSC Code is required!') }),
        accountNo: Yup.string().nullable().required("Account number is required!"),
        accountNoVerify: Yup.string().nullable().when('accountNo', {
            is: (accountNo) => {
                if (accountNo && accountNo.length > 0) {
                    return true
                }
                return false
            },
            then: Yup.string().oneOf([Yup.ref("accountNo")], "Both account numbers need to be the same!").required("Re-enter account number is required!")
        }),
    });

    const handleSubmit = async (values, actions) => {
        const data = values;
        if (rid) {
            data['id'] = rid;
        }
        await PaymentInfoService.UpdateAsync(data).then((res) => {
            notifySaved();
            props.history.push(PAYMENT_INFO);
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }
    return (
        <Fragment>
            <PageHeader title="Payment Info" />
            <Card>
                <CardBody>
                    {state.isLoading ? <Loading /> :
                        <Formik
                            initialValues={state.formValues}
                            validationSchema={validationSchema}
                            onSubmit={(values, actions) => handleSubmit(values, actions)}>
                            {({ values, actions, errors, touched, setFieldValue, isSubmitting }) => {
                                const handleValueChange = async (name, value) => {
                                    if (compare.isEqual(name, 'payMode')) {
                                        setFieldValue('bankName', '');
                                        setFieldValue('ifscCode', '');
                                        setFieldValue('accountNo', '');
                                        setFieldValue('accountNoVerify', '');
                                    }
                                    setFieldValue(name, value);
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
                                                <Input {...{
                                                    name: 'no', label: 'Employee Code', type: 'string',
                                                    value: selEmp.no, disabled: true,
                                                    error: errors['no'], touched: touched['no'],

                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col xs='6'>
                                                <Radio {...{
                                                    name: 'payMode', label: 'How Would You Like To Pay', disabled: false,
                                                    value: values['payMode'],
                                                    values: [{ value: 1, text: 'Bank Transfer' }, { value: 2, text: 'Online' }],
                                                    touched: touched['payMode'],
                                                    error: errors['payMode']
                                                }} handlevaluechange={handleValueChange}
                                                />
                                            </Col>

                                            <Col md='6'>
                                                <RWDropdownList {...{
                                                    name: 'bankId', label: 'Employer Bank', valueField: 'id', textField: 'name',
                                                    values: state.banks, value: values['bankId'],
                                                    error: errors['bankId'], touched: touched['bankId'],
                                                }} handlevaluechange={handleValueChange} />

                                            </Col>
                                        </Row>
                                        {values.payMode == 2 ?
                                            <Row>
                                                <Col md='6'>
                                                    <Input {...{
                                                        name: 'bankName', label: 'Bank Name', type: 'string',
                                                        value: values['bankName'], disabled: false,
                                                        error: errors['bankName'], touched: touched['bankName'],

                                                    }} handlevaluechange={handleValueChange} />
                                                </Col>
                                                <Col md='6'>
                                                    <Input {...{
                                                        name: 'ifscCode', label: 'IFSC Code', type: 'string',
                                                        value: values['ifscCode'], disabled: false,
                                                        error: errors['ifscCode'], touched: touched['ifscCode'],

                                                    }} handlevaluechange={handleValueChange} />
                                                </Col>
                                            </Row> : ''
                                        }
                                        <Row>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'accountNo', label: 'Account Number', type: 'string',
                                                    value: values['accountNo'], disabled: false,
                                                    error: errors['accountNo'], touched: touched['accountNo'],

                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'accountNoVerify', label: 'Re-Enter Account Number', type: 'string',
                                                    value: values['accountNoVerify'], disabled: false,
                                                    error: errors['accountNoVerify'], touched: touched['accountNoVerify'],

                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <div className='d-flex'>
                                            <Button className="mb-2 mr-2 btn-icon btn-success1" key='button' color="success" type="submit" name="save" disabled={isSubmitting} handlesubmit={() => handleSubmit(values, actions)}>
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
