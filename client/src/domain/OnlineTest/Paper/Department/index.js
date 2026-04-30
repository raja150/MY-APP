import { notifySaved } from 'components/alert/Toast';
import { RWDropdownList } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import { Form, Formik } from 'formik';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Row } from 'reactstrap';
import TestDepartmentService from 'services/OnlineTest/Paper/TestDepartment';
import DepartmentService from 'services/Org/Department';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';


export default function AddDepartment(props) {
    const [state, setState] = useState({ isLoading: true, department: [] })
    const [loading, setLoading] = useState(false)

    useEffect(() => {
        let department = []
        const fetchData = async () => {
            await DepartmentService.getDepSList().then((res) => {
                department = res.data
            })
            setState({ ...state, department: department, isLoading: false })
        }
        fetchData();
    }, [])

    const formValues = {
        departmentId: '',
    }

    const handleSubmit = async (values, actions) => {
        setLoading(true)
        const testData = {
            paperId: props.refId || props.rid,
            departmentId: values.departmentId,
        }

        await TestDepartmentService.AddAsync(testData).then((res) => {
            notifySaved();
            props.handleAddDept()
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
                    departmentId: Yup.string().required("Department is required")
                })}
                onSubmit={(values, actions) => handleSubmit(values, actions)}
            >
                {({ values, errors, touched, setFieldValue }) => {
                    const handleValueChange = async (name, value) => {
                        setFieldValue(name, value);
                    }
                    return <Form>
                        <Card>
                            <CardBody>
                                <Row>
                                    <Col md='6'>
                                        <RWDropdownList {...{
                                            name: 'departmentId', label: 'Department', valueField: 'id', textField: 'name',
                                            values: state.department, value: values['departmentId'],
                                            error: errors['departmentId'], touched: touched['departmentId'],
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                </Row>
                                <Row>
                                    <Col className='mt-3'>
                                        <Button color="success" name="save" handlesubmit={handleSubmit}
                                            disabled={loading}>
                                            <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i>
                                            {loading ? "Please wait..." : "Save"}
                                        </Button>
                                        <Button className='ml-2' color="danger" type='button' disabled={loading} onClick={() => props.handleAddDept()}> Cancel</Button>
                                    </Col>
                                </Row>
                            </CardBody>
                        </Card>
                    </Form>
                }}
            </Formik>
        </Fragment>
}