import { notifySaved } from 'components/alert/Toast';
import { Radio, RWDatePicker, RWDropdownList, RWMultiSelect } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Label, Row } from 'reactstrap';
import * as crypto from 'utils/Crypto';
import * as formUtil from 'utils/form';
import { Form, Formik } from 'formik';
import * as Yup from 'yup';
import WeekOffDaysService1 from '../../../services/Leave/WeekOffDays'
import * as Compare from 'utils/Compare'
export default function AddWeekOffDays(props) {
    const [state, setState] = useState({ isLoading: true, weekOffDays: [], formValues: {} })
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    useEffect(() => {
        let formValues = {}
        const fetchData = async () => {
            if (props.id) {
                await WeekOffDaysService1.GetWeekOffDaysById(props.id).then((res) => {
                    formValues = {
                        type: res.data.type,
                        weekDay: res.data.weekDay,
                        weekOffDaysId: res.data.id,
                        weekInYear: res.data.weekInYear,
                        weekNoInMonth: res.data.weekNoInMonth,
                        weekDate: res.data.weekDate,
                        status: res.data.status
                    }
                })
            }
            else {
                formValues = {
                    type: '',
                    weekDay: '',
                    weekInYear: '0',
                    weekNoInMonth: [],
                    weekDate: '',
                    status: '0'
                }
            }
            setState({ ...state, formValues: formValues, isLoading: false })
        }
        fetchData();
    }, [])
    const validationSchema = Yup.object({
        type: Yup.number().required("Type  is required"),
        weekDay: Yup.string().when('type', {
            is: (val) => val == 1 || val == 2,
            then: Yup.string().required('Week day is required !'),
            otherwise: Yup.string().notRequired()
        }),
        weekNoInMonth: Yup.string().when('type', {
            is: (val) => val == 1,
            then: Yup.string().required('Week day is required !'),
            otherwise: Yup.string().notRequired()
        }),
    });
    const handleSubmit = async (values, actions) => {
        
        const data = values
        if (props.id) {
            data['id'] = props.id
        }

        if (props.refId) {
            data['WeekNoInMonth'] = values.weekNoInMonth.length != 0 ? values.weekNoInMonth : "";
            data['weekOffSetupId'] = props.refId;
        }
        if (rid) {
            data['WeekNoInMonth'] = values.weekNoInMonth.length != 0 ? values.weekNoInMonth : "";
            data['weekOffSetupId'] = rid;
        }

        await WeekOffDaysService1.UpdateAsync(data).then((res) => {
            notifySaved();
            props.handleAddWeekOffDays()
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }
    const handleCancel = () => {
        props.handleAddWeekOffDays()
    }

    return <>
        {state.isLoading ? <Loading /> :
            <Fragment>
                <Card>
                    <CardBody>
                        <Formik
                            initialValues={state.formValues}
                            validationSchema={validationSchema}
                            onSubmit={(values, actions) => handleSubmit(values, actions)}
                        >{({ values, errors, touched, setFieldValue, isSubmitting, setValues }) => {
                            const handleValueChange = async (name, value, { selected }) => {
                                setFieldValue(name, value);
                                if (Compare.isEqual(name, 'type')) {
                                    if (Compare.isEqual(value, "1")) {
                                        setFieldValue('weekInYear', '')
                                        setFieldValue('weekDate', '')
                                        setFieldValue('status', '')
                                    } if (Compare.isEqual(value, "2")) {
                                        setFieldValue('weekNoInMonth', '')
                                        setFieldValue('weekDate', '')
                                        setFieldValue('status', '')
                                    } if (Compare.isEqual(value, "3")) {
                                        setFieldValue('weekNoInMonth', '')
                                        setFieldValue('weekDay', '')
                                        setFieldValue('weekInYear', '')
                                    }
                                }
                            }
                            return (

                                <Form>
                                    <Row>
                                        <Col md='6'>
                                            <Radio {...{
                                                name: 'type', label: 'Type', disabled: false,
                                                value: values['type'],
                                                values: [{ value: 1, text: 'Weeks in a month' }, { value: 2, text: 'Weeks in a year' }, { value: 3, text: 'Week Date' }],
                                                touched: touched['type'],
                                                error: errors['type']
                                            }} handlevaluechange={handleValueChange}
                                            />
                                        </Col>
                                        {values.type != 3 ? <Col md='5'>
                                            <RWDropdownList {...{
                                                name: 'weekDay', label: 'Week Day', valueField: 'value', textField: 'text',
                                                value: values['weekDay'], touched: touched['weekDay'], error: errors['weekDay'],
                                                values: [
                                                    { value: 1, text: 'Monday' }, { value: 2, text: 'Tuesday' },
                                                    { value: 3, text: 'Wednesday' }, { value: 4, text: 'Thursday' },
                                                    { value: 5, text: 'Friday' }, { value: 6, text: 'Saturday' },
                                                    { value: 7, text: 'Sunday' }]
                                            }} handlevaluechange={handleValueChange}
                                            />
                                        </Col>
                                            : ''}
                                    </Row>
                                    <Row>
                                        {values.type == 1 ? <Col md='6'>
                                            <RWMultiSelect {...{
                                                name: 'weekNoInMonth', label: 'Weeks In Month', value: values['weekNoInMonth'],
                                                values: [
                                                    { value: 1, text: '1st' }, { value: 2, text: '2nd' },
                                                    { value: 3, text: '3rd' }, { value: 4, text: '4th' },
                                                    { value: 5, text: '5th' }],
                                                valueField: 'value', textField: 'text',
                                                touched: touched['weekNoInMonth'], error: errors['weekNoInMonth']
                                            }} handlevaluechange={handleValueChange}
                                            />
                                        </Col> :
                                            values.type == 2 ? <Col md='6'>
                                                <Radio {...{
                                                    name: 'weekInYear', label: 'Week In Year', disabled: false,
                                                    value: values['weekInYear'],
                                                    values: [{ value: 0, text: 'All' }, { value: 1, text: 'On even weeks' }, { value: 2, text: 'On odd weeks' }],
                                                    touched: touched['weekInYear'],
                                                    error: errors['weekInYear']
                                                }} handlevaluechange={handleValueChange}
                                                />
                                            </Col> :
                                                values.type == 3 ?
                                                    <>
                                                        <Col md='3'>
                                                            <RWDatePicker {...{
                                                                name: 'weekDate', label: 'Week Date',
                                                                value: values['weekDate'], showDate: true, showTime: false,
                                                                error: errors['weekDate'], touched: touched['weekDate']
                                                            }} handlevaluechange={handleValueChange} />
                                                        </Col>
                                                        <Col md='6'>
                                                            <Radio {...{
                                                                name: 'status', label: 'Status', value: values['status'],
                                                                values: [{ value: 0, text: 'Off' }, { value: 1, text: 'Present' }],
                                                                touched: touched['status'],
                                                                error: errors['status']
                                                            }} handlevaluechange={handleValueChange}
                                                            />
                                                        </Col>
                                                    </>
                                                    : ''}
                                    </Row>
                                    <Row>
                                        <Col md='2.5'>
                                            <Button style={{ marginTop: '19px' }} color="primary" type="button" name="save" onClick={handleCancel}>
                                                <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i> {"Cancel"}
                                            </Button>
                                        </Col>
                                        <Col>
                                            <Button style={{ marginTop: '19px' }} key='button' color="success" type="submit" name="save" handlesubmit={handleSubmit}
                                                disabled={isSubmitting}>
                                                <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i> {"Save"}
                                            </Button>
                                        </Col>

                                    </Row>

                                </Form>
                            )
                        }
                            }
                        </Formik>
                    </CardBody>
                </Card>
            </Fragment>
        }
    </>
}