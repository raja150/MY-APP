import { notifySaved } from 'components/alert/Toast';
import { Input, RWDropdownList } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import EmployeeSearch from 'domain/EmployeeSearch';
import { Form, Formik } from 'formik';
import PageHeader from 'Layout/AppMain/PageHeader';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Label, Row } from 'reactstrap';
import IncentivesPayCutService from 'services/PayRoll/IncentivesPayCut';
import PayMonthService from 'services/PayRoll/PayMonth';
import * as crypto from 'utils/Crypto';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import { INCENTIVES_PAY_CUT } from '../navigation';


export default function IncentivesPayCut(props) {
    const [state, setState] = useState({ isLoading: true, formValues: {}, payMonth: [], Employees: [] })
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;

    const [selEmp, setSelEmp] = useState({})
    useEffect(() => {
        let formValues = {}
        const fetchData = async () => {
            let payMonth = [];
            await IncentivesPayCutService.PayMonths().then((response) => {
                payMonth = response.data
            });
            if (rid) {
                await IncentivesPayCutService.getById(rid).then((res) => {
                    formValues = res.data;
                    setSelEmp(formValues)
                });
            }
            else {
                formValues = {
                    employeeId: '',
                    payMonthId: '',
                    faxFilesAndArrears: '',
                    sundayInc: '',
                    productionInc: '',
                    spotInc: '',
                    punctualityInc: '',
                    centumClub: '',
                    otherInc: '',
                    firstMinuteInc: '',
                    nightShift: '',
                    weeklyStarInc: '',
                    tTeamInc: '',
                    doublePay: '',
                    internalQualityFeedbackDed: '',
                    externalQualityFeedbackDed: '',
                    unauthorizedLeaveDed: '',
                    lateComingDed: '',
                    otherDed: '',
                }
            }
            setState({ ...state, isLoading: false, formValues: formValues, payMonth: payMonth })
        }
        fetchData()
    }, [])

    const validationSchema = Yup.object({
        employeeId: Yup.string().required("Employee name is required!"),
        faxFilesAndArrears: Yup.number().required("Fax files and arrears is required!"),
        sundayInc: Yup.number().required("Sunday incentive is required!"),
        productionInc: Yup.number().required("Production incentive is required!"),
        spotInc: Yup.number().required("Spot Incentive is required!"),
        punctualityInc: Yup.number().required("Punctuality incentive is required!"),
        centumClub: Yup.number().required("Centum club is required!"),
        firstMinuteInc: Yup.number().required("First minute incentive is required!"),
        otherInc: Yup.number().required("Other incentives is required!"),
        nightShift: Yup.number().required("Night shift is required!"),
        weeklyStarInc: Yup.number().required("Weekly star incentive is required!"),
        tTeamInc: Yup.number().required("Transition team incentive is required!"),
        doublePay: Yup.number().required("Double pay is required!"),
        internalQualityFeedbackDed: Yup.number().required("Internal quality feedback is required!"),
        externalQualityFeedbackDed: Yup.number().required("External quality feedback is required!"),
        unauthorizedLeaveDed: Yup.number().required(" Unauthorized leave deduction is required!"),
        lateComingDed: Yup.number().required("Late coming deduction is required!"),
        otherDed: Yup.number().required("Other deductions is required!"),

    });

    const handleSubmit = async (values, actions) => {
        const data = values;
        if (rid) {
            data['id'] = rid;
        }
        await IncentivesPayCutService.UpdateAsync(data).then((res) => {
            notifySaved();
            props.history.push(INCENTIVES_PAY_CUT);
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }
    return (
        <Fragment>
            <PageHeader title="Incentives Pay Cut" />
            <Card>
                <CardBody>
                    {state.isLoading ? <Loading /> :
                        <Formik
                            initialValues={state.formValues}
                            validationSchema={validationSchema}
                            onSubmit={(values, actions) => handleSubmit(values, actions)}>
                            {({ values, errors, touched, setFieldValue, isSubmitting }) => {
                                const handleValueChange = async (name, value) => {
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
                                                {rid ? ''
                                                    : <RWDropdownList {...{
                                                        name: 'payMonthId', label: 'Pay Month', valueField: 'id', textField: 'name',
                                                        values: state.payMonth, value: values['payMonthId'],
                                                        error: errors['payMonthId'], touched: touched['payMonthId'],
                                                    }} handlevaluechange={handleValueChange} />
                                                }
                                            </Col>
                                        </Row>
                                        <p className='font-weight-bold h6'>Incentives</p>
                                        <Row>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'faxFilesAndArrears', label: 'Fax Files And Arrears', type: 'number',
                                                    value: values['faxFilesAndArrears'],
                                                    error: errors['faxFilesAndArrears'], touched: touched['faxFilesAndArrears'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'sundayInc', label: 'Sunday Incentives', type: 'number',
                                                    value: values['sundayInc'],
                                                    error: errors['sundayInc'], touched: touched['sundayInc'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'productionInc', label: 'Production Incentives', type: 'number',
                                                    value: values['productionInc'],
                                                    error: errors['productionInc'], touched: touched['productionInc'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'spotInc', label: 'Spot Incentives', type: 'number',
                                                    value: values['spotInc'],
                                                    error: errors['spotInc'], touched: touched['spotInc'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'punctualityInc', label: 'Punctuality Incentives', type: 'number',
                                                    value: values['punctualityInc'],
                                                    error: errors['punctualityInc'], touched: touched['punctualityInc'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'centumClub', label: 'Centum Club', type: 'number',
                                                    value: values['centumClub'],
                                                    error: errors['centumClub'], touched: touched['centumClub'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'firstMinuteInc', label: 'First Minute Incentives', type: 'number',
                                                    value: values['firstMinuteInc'],
                                                    error: errors['firstMinuteInc'], touched: touched['firstMinuteInc'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'doublePay', label: 'Double Pay', type: 'number',
                                                    value: values['doublePay'],
                                                    error: errors['doublePay'], touched: touched['doublePay'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'nightShift', label: 'Night Shift', type: 'number',
                                                    value: values['nightShift'],
                                                    error: errors['nightShift'], touched: touched['nightShift'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'weeklyStarInc', label: 'Weekly Star Incentives', type: 'number',
                                                    value: values['weeklyStarInc'],
                                                    error: errors['weeklyStarInc'], touched: touched['weeklyStarInc'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'tTeamInc', label: 'Transition Team Incentives', type: 'number',
                                                    value: values['tTeamInc'],
                                                    error: errors['tTeamInc'], touched: touched['tTeamInc'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'otherInc', label: 'Other Incentives', type: 'number',
                                                    value: values['otherInc'],
                                                    error: errors['otherInc'], touched: touched['otherInc'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <p className='font-weight-bold h6 mt-3'>Deductions</p>
                                        <Row>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'internalQualityFeedbackDed', label: 'Internal Quality Feedback', type: 'number',
                                                    value: values['internalQualityFeedbackDed'],
                                                    error: errors['internalQualityFeedbackDed'], touched: touched['internalQualityFeedbackDed'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'externalQualityFeedbackDed', label: 'External Quality Feedback', type: 'number',
                                                    value: values['externalQualityFeedbackDed'],
                                                    error: errors['externalQualityFeedbackDed'], touched: touched['externalQualityFeedbackDed'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'unauthorizedLeaveDed', label: 'Unauthorized Leave Deduction', type: 'number',
                                                    value: values['unauthorizedLeaveDed'],
                                                    error: errors['unauthorizedLeaveDed'], touched: touched['unauthorizedLeaveDed'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'lateComingDed', label: 'Late Coming Deduction', type: 'number',
                                                    value: values['lateComingDed'],
                                                    error: errors['lateComingDed'], touched: touched['lateComingDed'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'otherDed', label: 'Other Deductions', type: 'number',
                                                    value: values['otherDed'],
                                                    error: errors['otherDed'], touched: touched['otherDed'],
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <div className='d-flex'>
                                            <Button className="mb-2 mr-2 btn-icon btn-success1" key='button' color="success" type="submit" name="save" disabled={isSubmitting} handlesubmit={handleSubmit}>
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
