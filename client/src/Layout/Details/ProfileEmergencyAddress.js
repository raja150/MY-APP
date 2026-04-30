import { Form, Formik } from 'formik';
import { Input } from 'components/dynamicform/Controls';
import { Button, Card, CardBody, Col, Modal, ModalBody, ModalHeader, Row } from 'reactstrap';
import React, { useState } from 'react';
import Loading from 'components/Loader';
import * as _ from 'lodash';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import UpdateProfileService from 'services/Org/UpdateProfileService';


function ProfileEmergencyAddress(props) {

    const [state, setState] = useState({
        isLoading: false, formValues: {
            ...props.data
        }
    })


    const handleSave = async (values, actions) => {
        const data = {
            addressLineOne: values.addressLineOne,
            addressLineTwo: values.addressLineTwo,
            cityOrTown: values.cityOrTown,
            state: values.state,
            country: values.country,
            EmergencyConNo: values.emergencyConNo

        }
        if (props.data) {
            data['id'] = props.data.id;
        }
        await UpdateProfileService.UpdateEmergencyAddress(data).then((response) => {
            props.setEmergencyResult('emergencyAddress', response.data);
            props.toggle();

        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }

    const validationSchema = Yup.object().shape({
        addressLineOne: Yup.string().required("Address Line First is required"),
        addressLineTwo: Yup.string().required("Address Line Second is required"),
        cityOrTown: Yup.string().required("City/Town is required"),
        state: Yup.string().required("State is required"),
        country: Yup.string().required("Country is required"),
        emergencyConNo: Yup.string().required("Contact Number is required")

    });
    const handleCancel = () => {
        props.toggle();
    }
    return (
        <Card className="mb-3">
            <CardBody>
                <Modal isOpen={true} >
                    <Formik
                        initialValues={state.formValues}
                        validationSchema={validationSchema}
                        enableReinitialize={true}
                        onSubmit={(values, actions) => handleSave(values, actions)}
                    >
                        {({ values, errors,touched, setFieldValue, isSubmitting }) => {
                            const handleValueChange = async (name, value) => {
                                setFieldValue(name, value)
                            };
                            return (
                                state.isLoading ? <Loading /> :
                                    <Form>
                                        <ModalHeader>Emergency Address</ModalHeader>
                                        <ModalBody>
                                            <Row>
                                                <Col xs='6'>
                                                    <Input
                                                        {...{
                                                            name: 'addressLineOne',
                                                            label: 'Address Line 1',
                                                            value: values['addressLineOne'],
                                                            error: errors['addressLineOne'], touched: touched['addressLineOne']
                                                        }}
                                                        handlevaluechange={handleValueChange}
                                                    />
                                                </Col >
                                                <Col xs='6'>
                                                    <Input
                                                        {...{
                                                            name: 'addressLineTwo',
                                                            label: 'Address Line 2',
                                                            value: values['addressLineTwo'],
                                                            error: errors['addressLineTwo'], touched: touched['addressLineTwo']
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
                                                             error: errors['cityOrTown'], touched: touched['cityOrTown']
                                                        }}
                                                        handlevaluechange={handleValueChange}
                                                    />
                                                </Col >
                                                <Col>
                                                    <Input
                                                        {...{
                                                            name: 'emergencyConNo',
                                                            label: 'Emergency Contact Number',
                                                            value: values['emergencyConNo'],
                                                             error: errors['emergencyConNo'], touched: touched['emergencyConNo']
                                                        }}
                                                        handlevaluechange={handleValueChange}
                                                    />
                                                </Col>
                                            </Row>

                                            <Row>
                                                <Col>
                                                    <Input {...{
                                                        name: 'state', label: 'State',
                                                        value: values['state'],
                                                         error: errors['state'], touched: touched['state']
                                                    }} handlevaluechange={handleValueChange}
                                                    />
                                                </Col>
                                                <Col>
                                                    <Input {...{
                                                        name: 'country',
                                                        label: 'Country',
                                                        value: values['country'],
                                                         error: errors['country'], touched: touched['country']
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

export default ProfileEmergencyAddress
