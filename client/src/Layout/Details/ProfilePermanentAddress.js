import { Form, Formik } from 'formik';
import { Input, Radio } from 'components/dynamicform/Controls';
import { Button, Card, CardBody, Col, Modal, ModalBody, ModalHeader, Row } from 'reactstrap';
import React, { useState } from 'react';
import Loading from 'components/Loader';
import * as compare from 'utils/Compare'
import * as formUtil from 'utils/form';
import * as _ from 'lodash';
import * as Yup from 'yup';
import UpdateProfileService from 'services/Org/UpdateProfileService';

function ProfilePermanentAddress(props) {
    const [state, setState] = useState({
        isLoading: false, formValues: {
            disable: compare.isEqual(props.data.sameAsPresent, "1") ? true : false
            , sameAsPresent: compare.isEqual(props.data.sameAsPresent, 1) ? 1 : 0, ...props.data,
        }, presentAddress: { ...props.presentAddress }
    })

    const validationSchema = Yup.object({
        addressLineOne: Yup.string().when('sameAsPresent', {
            is: (val) => val === 0,
            then: Yup.string().required("Address Line One is required"),
            otherwise: Yup.string().notRequired()
        }),
        addressLineTwo: Yup.string().when('sameAsPresent', {
            is: (val) => val === 0,
            then: Yup.string().required("Address Line Two is required"),
            otherwise: Yup.string().notRequired()
        }),
        cityOrTown: Yup.string().when('sameAsPresent', {
            is: (val) => val === 0,
            then: Yup.string().required("City Or Town is required"),
            otherwise: Yup.string().notRequired('')
        }),
        state: Yup.string().when('sameAsPresent', {
            is: (val) => val === 0,
            then: Yup.string().required("State is required"),
            otherwise: Yup.string().notRequired()
        }),
        country: Yup.string().when('sameAsPresent', {
            is: (val) => val === 0,
            then: Yup.string().required("Country is required"),
            otherwise: Yup.string().notRequired()
        })
    })

    const handleSave = async (values, actions) => {
        const data = {
            addressLineOne: values.addressLineOne,
            addressLineTwo: values.addressLineTwo,
            cityOrTown: values.cityOrTown,
            state: values.state,
            country: values.country,
            SameAsPresent: values.sameAsPresent
        }
        if (props.data) {
            data['id'] = props.data.id;
        }

        await UpdateProfileService.UpdatePermanentAddress(data).then((response) => {
            props.setPermanentResult('permanentAddress', response.data);
            props.toggle();
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }

    const handleCancel = () => {
        props.toggle();
    }
    return (
        <Card className="mb-3">
            <CardBody>
                <Modal isOpen={true}>
                    <Formik
                        initialValues={state.formValues}
                        validationSchema={validationSchema}
                        enableReinitialize={true}
                        onSubmit={(values, actions) => handleSave(values, actions)}
                    >
                        {({ values, setFieldValue, errors, touched, isSubmitting }) => {
                            const handleValueChange = async (name, value) => {


                                if (compare.isEqual(name, 'sameAsPresent')) {
                                    if (compare.isEqual(value, "1")) {
                                        setFieldValue('addressLineTwo', state.presentAddress.addressLineTwo, false)
                                        setFieldValue('cityOrTown', state.presentAddress.cityOrTown, false)
                                        setFieldValue('state', state.presentAddress.state, false)
                                        setFieldValue('country', state.presentAddress.country, false)
                                        setFieldValue('addressLineOne', state.presentAddress.addressLineOne, false)
                                        setFieldValue('disable', true, false)
                                        setFieldValue('sameAsPresent', 1, true)
                                    }
                                    else {
                                        setFieldValue('addressLineTwo', '', false)
                                        setFieldValue('cityOrTown', '', false)
                                        setFieldValue('state', '', false)
                                        setFieldValue('country', '', false)
                                        setFieldValue('addressLineOne', '', false)
                                        setFieldValue('disable', false, false)
                                        setFieldValue('sameAsPresent', 0, true)
                                    }
                                }
                                else {
                                    setFieldValue(name, value)
                                }
                            };

                            return (
                                state.isLoading ? <Loading /> :
                                    <Form>
                                        <ModalHeader>Permanent Address</ModalHeader>
                                        <ModalBody>
                                            <Row>
                                                <Col xs='6'>
                                                    <Input
                                                        {...{
                                                            name: 'addressLineOne',
                                                            label: 'Address Line 1',
                                                            value: values['addressLineOne'],
                                                            error: errors['addressLineOne'],
                                                            touched: touched['addressLineOne'],
                                                            disabled: values.disable
                                                        }}
                                                        handlevaluechange={handleValueChange}
                                                    />
                                                </Col >
                                                <Col xs='6'>
                                                    <Input
                                                        {...{
                                                            name: "addressLineTwo",
                                                            label: 'Address Line 2',
                                                            value: values["addressLineTwo"],
                                                            error: errors["addressLineTwo"], touched: touched["addressLineTwo"],
                                                            disabled: values.disable
                                                        }}
                                                        handlevaluechange={handleValueChange}
                                                    />
                                                </Col >
                                            </Row>
                                            <Row>
                                                <Col xs='6'>
                                                    <Input
                                                        {...{
                                                            name: 'cityOrTown',
                                                            label: 'City/Town',
                                                            value: values['cityOrTown'],
                                                            error: errors['cityOrTown'], touched: touched['cityOrTown'],
                                                            disabled: values.disable
                                                        }}
                                                        handlevaluechange={handleValueChange}
                                                    />
                                                </Col >
                                                <Col>
                                                    <Radio {...{
                                                        name: 'sameAsPresent', label: 'Same As Present',
                                                        value: values['sameAsPresent'],
                                                        values: [{ value: 1, text: 'Yes' }, { value: 0, text: 'No' }],
                                                    }} handlevaluechange={handleValueChange}
                                                    />
                                                </Col>
                                            </Row>

                                            <Row>
                                                <Col>
                                                    <Input {...{
                                                        name: 'state', label: 'State',
                                                        value: values['state'],
                                                        error: errors['state'], touched: touched['state'],
                                                        disabled: values.disable
                                                    }} handlevaluechange={handleValueChange}
                                                    />
                                                </Col>
                                                <Col>
                                                    <Input {...{
                                                        name: 'country',
                                                        label: 'Country',
                                                        value: values['country'],
                                                        error: errors['country'], touched: touched['country'],
                                                        disabled: values.disable
                                                    }} handlevaluechange={handleValueChange}
                                                    />
                                                </Col>
                                            </Row>
                                            <Row className='justify-content-center'>
                                                <Button className="mb-2 mr-2 btn-icon btn-success" key='Save' color="success" type="submit" name="save" disabled={isSubmitting}>
                                                    {isSubmitting ? "Please Wait..." : "Save"}
                                                </Button>
                                                <Button className="mb-2 mr-2 btn-icon btn-primary" key='Cancel' color="success" type="button" onClick={() => handleCancel()} name="Cancel" >
                                                    {"Cancel"}
                                                </Button>
                                            </Row>
                                        </ModalBody>
                                    </Form>
                            )
                        }}
                    </Formik>
                </Modal>
            </CardBody>
        </Card>
    )
}

export default ProfilePermanentAddress
