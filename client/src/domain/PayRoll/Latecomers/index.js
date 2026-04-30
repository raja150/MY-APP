import { notifySaved } from 'components/alert/Toast';
import { Input, RWDropdownList } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import EmployeeSearch from 'domain/EmployeeSearch';
import { Form, Formik } from 'formik';
import PageHeader from 'Layout/AppMain/PageHeader';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Label, Row } from 'reactstrap';
import LatecomersService from 'services/PayRoll/Latecomers';
import PayMonthService from 'services/PayRoll/PayMonth';
import * as crypto from 'utils/Crypto';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import { LATECOMER } from '../navigation';


export default function LateComing(props) {
    const [state, setState] = useState({ isLoading: true, formValues: {}, PayMonth: [], Employees: [] })
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;

    const [selEmp, setSelEmp] = useState({})
    const [isSearching, setIsSearching] = useState(false);
    useEffect(() => {
        let formValues = {}
        const fetchData = async () => {
            let PayMonth = [];
            await LatecomersService.PayMonths().then((response) => {
                PayMonth = response.data
            });
            if (rid) {
                await LatecomersService.getById(rid).then((res) => {
                    formValues = res.data;
                    setSelEmp(formValues)
                });
            }
            else {
                formValues = {
                    employeeId: '',
                    payMonthId: '',
                    numberOfDays: '',
                }
            }
            setState({ ...state, isLoading: false, formValues: formValues, PayMonth: PayMonth })
        }
        fetchData()
    }, [])

    const validationSchema = Yup.object({
        employeeId: Yup.string().required("Employee Name is required"),
    });

    const handleSubmit = async (values, actions) => {
        const data = values;
        if (rid) {
            data['id'] = rid;
        }
        await LatecomersService.UpdateAsync(data).then((res) => {
            notifySaved();
            props.history.push(LATECOMER);
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }
    return (
        <Fragment>
            <PageHeader title="Latecomers" />
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
                                                        values: state.PayMonth, value: values['payMonthId'],
                                                        error: errors['payMonthId'], touched: touched['payMonthId'],
                                                    }} handlevaluechange={handleValueChange} />
                                                }
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col md='6'>
                                                <Input {...{
                                                    name: 'numberOfDays', label: 'Number Of Days', type: 'string',
                                                    value: values['numberOfDays'],
                                                    error: errors['numberOfDays'], touched: touched['numberOfDays'],

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
