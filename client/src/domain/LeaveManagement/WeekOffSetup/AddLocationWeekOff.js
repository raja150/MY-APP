import { notifySaved } from 'components/alert/Toast';
import { RWDropdownList } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import { Form, Formik } from 'formik';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Row } from 'reactstrap';
import * as crypto from 'utils/Crypto';
import * as formUtil from 'utils/form';
import LocationService from 'services/Org/Location'
import * as Yup from 'yup';


export default function AddLocationWeekOff(props) {
    const [state, setState] = useState({ isLoading: true, location: [], formValues: {} })
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    useEffect(() => {
        let location = [], formValues = {}
        const fetchData = async () => {
            await LocationService.getLocationsList().then((res) => {
                location = res.data
            })
            if (props.id) {
                await LocationService.GetLocAllocationById(props.id).then((res) => {
                    formValues = {
                        locationId: res.data.id,
                    }

                })
            }
            else {
                formValues = {
                    locationId: '',
                }
            }
            setState({ ...state, formValues: formValues, location: location, isLoading: false })
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
        await LocationService.UpdateAsync(data).then((res) => {
            notifySaved();
            props.handleAddLoc()
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }
    const handleCancel = () => {
        props.handleAddLoc()
    }

    const validationSchema = Yup.object().shape({
        locationId: Yup.string().required("Location is required")
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
                                                    name: 'locationId', label: 'Location', valueField: 'id', textField: 'name',
                                                    values: state.location, value: values['locationId'],
                                                    error: errors['locationId'], touched: touched['locationId'],
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