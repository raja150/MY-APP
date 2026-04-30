import { notifySaved } from 'components/alert/Toast';
import { RWDropdownList } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import { FieldError } from 'domain/Error/ErrorMessage'
import { Form, Formik } from 'formik';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Label, Row } from 'reactstrap'
import EmployeeService from 'services/Org/Employee';
import EmployeeAllocationService from 'services/Org/EmployeeAllocationService';
import * as crypto from 'utils/Crypto';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import EmployeeSearch from 'domain/EmployeeSearch';


export default function AddEmployeeWeekOff(props) {
    const [onEdit, setOnEdit] = useState(false)
    const [selEmp, setSelEmp] = useState({})
    const [state, setState] = useState({ isLoading: true, employees: [], empS: [], formValues: {} })
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    useEffect(() => {
        let employees = [], formValues = {}
        const fetchData = async () => {
            await EmployeeService.getEmpSList().then((res) => {
                employees = res.data
            })
            if (props.id) {
                await EmployeeAllocationService.GetEmpAllocationById(props.id).then((res) => {
                    formValues = res.data
                    setSelEmp(res.data)
                })
            }
            else {
                formValues = {
                    employeeId: '',
                }
            }
            setState({ ...state, formValues: formValues, employees: employees, isLoading: false })
        }
        fetchData();
    }, [])
    const handleSubmit = async (values, actions) => {
        const data = values;
        if (rid) {
            data['weekOffSetupId'] = rid;
        }
        if (props.id) {
            data['id'] = props.id
        }
        await EmployeeAllocationService.UpdateAsync(data).then((res) => {
            notifySaved();
            props.handleAddEmp()
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }
    const handleCancel = () => {
        props.handleAddEmp()
    }

    const validationSchema = Yup.object().shape({
        employeeId: Yup.string().required("Employee is required")
    });

    return <>
        {state.isLoading ? <Loading /> :
            <Fragment>
                <Card>
                    <CardBody>
                        <Formik
                            initialValues={state.formValues}
                            validationSchema={validationSchema}

                            onSubmit={(values, actions) => handleSubmit(values, actions)}
                        >
                            {({ values, errors, touched, setFieldValue, isSubmitting, setValues }) => {
                                const handleValueChange = async (name, value, { selected }) => {
                                    setFieldValue(name, value);
                                }
                                return (
                                    <Form>
                                        <Row>
                                            <Col md='6'>
                                                <Label htmlFor='employee'>Employee Name</Label>
                                                <EmployeeSearch name={'employeeId'} selEmp={selEmp} setSelEmp={setSelEmp}
                                                    handleValueChange={handleValueChange} />
                                                <p style={{ color: 'red' }}>{errors.employeeNo}</p>
                                            </Col>
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
                            }}
                        </Formik>
                    </CardBody>
                </Card>
            </Fragment>
        }
    </>
}