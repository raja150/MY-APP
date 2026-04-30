import { notifySaved } from 'components/alert/Toast';
import { RWDropdownList } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import { Form, Formik } from 'formik';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Row } from 'reactstrap';
import DesignationService from 'services/Org/Designation';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import TestDesignationService from 'services/OnlineTest/Paper/TestDesignation'


export default function AddDesignation(props) {
    const [state, setState] = useState({ isLoading: true, designation: [] })
    const [loading, setLoading] = useState(false)


    useEffect(() => {
        let designation = []
        const fetchData = async () => {
            await DesignationService.getDesignationsList().then((res) => {
                designation = res.data
            })
            setState({ ...state, designation: designation, isLoading: false })
        }
        fetchData();
    }, [])

    const formValues = {
        designationId: '',
    }

    const handleSubmit = async (values, actions) => {
        setLoading(true)
        const testData = {
            paperId: props.refId || props.rid,
            designationId: values.designationId,
        }

        await TestDesignationService.AddAsync(testData).then((res) => {
            notifySaved();
            props.handleAddDesign()
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
                    designationId: Yup.string().required("Designation is required")
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
                                            name: 'designationId', label: 'Designation', valueField: 'id', textField: 'name',
                                            values: state.designation, value: values['designationId'],
                                            error: errors['designationId'], touched: touched['designationId'],
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                </Row>
                                <Row>
                                    <Col className='mt-3'>
                                        <Button color="success" name="save" handlesubmit={handleSubmit} disabled={loading}>
                                            <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i>
                                            {loading ? "Please wait... " : "Save"}
                                        </Button>
                                        <Button className='ml-2' color="danger" type='button'
                                            onClick={() => props.handleAddDesign()} disabled={loading}>
                                            Cancel</Button>
                                    </Col>
                                </Row>
                            </CardBody>
                        </Card>
                    </Form>
                }}
            </Formik>
        </Fragment>
}