import { notifySaved } from 'components/alert/Toast';
import Loading from 'components/Loader';
import EmployeeSearch from 'domain/EmployeeSearch';
import { Form, Formik } from 'formik';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Label, Row } from 'reactstrap';
import TestEmployeeService from 'services/OnlineTest/Paper/TestEmployee';
import EmployeeService from 'services/Org/Employee';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';


export default function AddEmployee(props) {
    const [selEmp, setSelEmp] = useState({})
    const [state, setState] = useState({ isLoading: true, employees: [], empS: [] })
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        let employees = []
        const fetchData = async () => {
            await EmployeeService.getEmpSList().then((res) => {
                employees = res.data
            })
            setState({ ...state, employees: employees, isLoading: false })
        }
        fetchData();
    }, [])

    const formValues = {
        employeeId: '',
    }

    const handleSubmit = async (values, actions) => {
        setLoading(true)
        const testData = {
            paperId: props.refId || props.rid,
            employeeId: values.employeeId
        }
        await TestEmployeeService.AddAsync(testData).then((res) => {
            notifySaved();
            props.handleAddEmp()
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
        setLoading(false)
    }


    return state.isLoading ? <Loading /> :
        <Fragment>
            <Formik
                initialValues={formValues}
                validationSchema={Yup.object().shape({
                    employeeId: Yup.string().required("Employee is required")
                })}

                onSubmit={(values, actions) => handleSubmit(values, actions)}
            >
                {({ errors, setFieldValue }) => {
                    const handleValueChange = async (name, value) => {
                        setFieldValue(name, value);
                    }
                    return <Form>
                        <Card>
                            <CardBody>
                                <Row>
                                    <Col md='6'>
                                        <Label htmlFor='employee'>Employee Name</Label>
                                        <EmployeeSearch name={'employeeId'} selEmp={selEmp} setSelEmp={setSelEmp}
                                            handleValueChange={handleValueChange} />
                                        <p style={{ color: 'red', fontSize: '10px' }}>{errors.employeeId}</p>
                                    </Col>
                                </Row>
                                <Row>
                                    <Col>
                                        <Button color="success" name="save" handlesubmit={handleSubmit} disabled={loading}>
                                            <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i>
                                            {loading ? "Please wait..." : "Save"}
                                        </Button>
                                        <Button className='ml-2' color="danger" type='button'
                                            onClick={() => props.handleAddEmp()} disabled={loading}>Cancel
                                        </Button>
                                    </Col>
                                </Row>
                            </CardBody>
                        </Card>
                    </Form>
                }}
            </Formik>
        </Fragment>
}