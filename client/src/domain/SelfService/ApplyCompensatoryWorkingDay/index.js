import { notifySaved } from 'components/alert/Toast';
import { Input, RWDatePicker, RWDropdownList, TextAreaInput } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import { Form, Formik } from 'formik';
import PageHeader from 'Layout/AppMain/PageHeader';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Row } from 'reactstrap';
import ShiftService from 'services/Leave/Shift';
import CompensatoryWorkingDayService from 'services/Leave/CompensatoryWorkingDayService';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import { APPLY_COMPENSATORY_WORKING_DAY } from '../navigation';
import * as crypto from 'utils/Crypto';


export default function ApplyCompensatoryWorkingDay(props) {
    const [state, setState] = useState({ isLoading: true, formValues: {}, shiftType: [] })
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    useEffect(() => {
        let formValues = {}
        let shiftType = [];
        const fetchData = async () => {
            await ShiftService.getList().then((res) => {
                shiftType = res.data;
            })
            if (rid) {
                await CompensatoryWorkingDayService.getById(rid).then((res) => {
                    formValues = res.data;
                });
            }
            else {
                formValues = {
                    fromDate: '',
                    toDate: '',
                    reasonForApply: '',
                }
            }
            setState({ ...state, isLoading: false, formValues: formValues, shiftType: shiftType })
        }
        fetchData()
    }, [])

    const validationSchema = Yup.object({
        fromDate: Yup.string().required("fromDate is required"),
        toDate: Yup.string().required("toDate is required"),
        reasonForApply: Yup.string().required("ReasonForApply is required")

    });

    const handleSubmit = async (values, actions) => {
        const data = values;
        if (rid) {
            data['id'] = rid;
        }
        await CompensatoryWorkingDayService.UpdateAsync(data).then((res) => {
            notifySaved();
            props.history.push(APPLY_COMPENSATORY_WORKING_DAY);
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }
    return (
        <Fragment>
            <PageHeader title="Apply Compensatory Working  Day " />
            <Card>
                <CardBody>
                    {state.isLoading ? <Loading /> :
                        <Formik
                            initialValues={state.formValues}
                            validationSchema={validationSchema}
                            onSubmit={(values, actions) => handleSubmit(values, actions)}>
                            {({ values, errors, touched, setFieldValue, isSubmitting, setValues }) => {
                                const handleValueChange = async (name, value, { selected }) => {
                                    setFieldValue(name, value);
                                }
                                return (
                                    <Form>
                                        <Row>
                                            <Col md='6'>
                                                <RWDatePicker {...{
                                                    name: 'fromDate', label: 'From Date',
                                                    value: values['fromDate'], error: errors['fromDate'],
                                                    touched: touched['fromDate'], showDate: true,
                                                    showTime: false
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                            <Col md='6'>
                                                <RWDatePicker {...{
                                                    name: 'toDate', label: 'To Date',
                                                    value: values['toDate'], error: errors['toDate'],
                                                    touched: touched['toDate'], showDate: true,
                                                    showTime: false
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col md='6'>
                                                <TextAreaInput {...{
                                                    name: 'reasonForApply', label: 'Reason For Applying Compensatory Working  Day',
                                                    value: values['reasonForApply'], error: errors['reasonForApply'],
                                                    touched: touched['reasonForApply']
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
