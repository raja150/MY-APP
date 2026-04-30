import { notifySaved } from 'components/alert/Toast';
import { RWDropdownList } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import { Form, Formik } from 'formik';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Row } from 'reactstrap';
import * as crypto from 'utils/Crypto';
import * as formUtil from 'utils/form';
import DesignationService from 'services/Org/Designation';
import * as Yup from 'yup';


export default function AddDesignationWeekOff(props) {
    const [state, setState] = useState({ isLoading: true, designation: [], formValues: {} })
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    useEffect(() => {
        let designation = [], formValues = {}
        const fetchData = async () => {
            await DesignationService.getDesignationsList().then((res) => {
                designation = res.data
            })
            if (props.id) {
                await DesignationService.GetDesignAllocationById(props.id).then((res) => {
                    formValues = {
                        designationId: res.data.id,
                    }
                })
            }
            else {
                formValues = {
                    designationId: '',
                }
            }
            setState({ ...state, formValues: formValues, designation: designation, isLoading: false })
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

        await DesignationService.UpdateAsync(data).then((res) => {
            notifySaved();
            props.handleAddDesign()
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }
    const handleCancel = () => {
        props.handleAddDesign()
    }

    const validationSchema = Yup.object().shape({
        designationId: Yup.string().required("Designation is required")
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
                                                <RWDropdownList {...{
                                                    name: 'designationId', label: 'Designation', valueField: 'id', textField: 'name',
                                                    values: state.designation, value: values['designationId'],
                                                    error: errors['designationId'], touched: touched['designationId'],
                                                }} handlevaluechange={handleValueChange} />
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