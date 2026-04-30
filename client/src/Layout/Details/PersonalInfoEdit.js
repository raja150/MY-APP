import { Form, Formik } from 'formik';
import { Input, Radio, RWDatePicker } from 'components/dynamicform/Controls';
import { Button, Card, CardBody, Col, Modal, ModalBody, ModalHeader, Row } from 'reactstrap';
import React, { useState } from 'react';
import Loading from 'components/Loader';
import * as _ from 'lodash';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import UpdateProfileService from 'services/Org/UpdateProfileService';

function PersonalInfoEdit(props) {
    const [state, setState] = useState({
        isLoading: false, formValues: {
            ...props.data
        }
    })

    const validationSchema = Yup.object().shape({
        dateOfBirth: Yup.string().required("Date Of Birth is required"),
        mobileNumber: Yup.string().required("Mobile Number is required"),
        fatherName: Yup.string().required("Father Name is required"),
        personalEmail: Yup.string().required("Personal Email is required"),

    });
    const handleSave = async (values, actions) => {
        const data = {
            personalEmail: values.personalEmail,
            mobileNumber: values.mobileNumber,
            no: values.no,
            aadhaarNumber: values.aadhaarNumber,
            gender: values.gender,
            dateOfBirth: values.dateOfBirth,
            maritalStatus: values.maritalStatus,
            fatherName: values.fatherName
        }
        if (props.data) {
            data['id'] = props.data.id;
        }
        await UpdateProfileService.PersonalInfoEdit(data).then((response) => {
            props.setPersonalInfoResult('empDetails', response.data);
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
                <Modal isOpen={true} >
                    <Formik
                        initialValues={state.formValues}
                        validationSchema={validationSchema}
                        onSubmit={(values, actions) => handleSave(values, actions)}
                    >
                        {({ values, errors, touched, setFieldValue, isSubmitting }) => {
                            const handleValueChange = async (name, value) => {
                                setFieldValue(name, value)
                            };
                            return (
                                state.isLoading ? <Loading /> :
                                    <Form>
                                        <ModalHeader>Personal Information</ModalHeader>
                                        <ModalBody>
                                            <Row>
                                                <Col>
                                                    <RWDatePicker {...{
                                                        name: 'dateOfBirth', label: 'Date Of Birth', showDate: true, showTime: false,
                                                        value: values['dateOfBirth']

                                                    }} handlevaluechange={handleValueChange} />
                                                </Col>
                                                <Col>
                                                    <Input
                                                        {...{
                                                            name: 'mobileNumber',
                                                            label: 'Mobile Number',
                                                            value: values['mobileNumber'],
                                                            error: errors['mobileNumber'], touched: touched['mobileNumber']
                                                        }}
                                                        handlevaluechange={handleValueChange}
                                                    />
                                                </Col>
                                            </Row>
                                            <Row>
                                                <Col>
                                                    <Radio {...{
                                                        name: 'gender', label: 'Gender',
                                                        value: values['gender'],
                                                        values: [{ value: 1, text: 'Male' }, { value: 2, text: 'Female' }],
                                                    }} handlevaluechange={handleValueChange}
                                                    />
                                                </Col>
                                                <Col>
                                                    <Radio {...{
                                                        name: 'maritalStatus', label: 'Marital Status',
                                                        value: values['maritalStatus'],
                                                        values: [{ value: 1, text: 'Married' }, { value: 2, text: 'Single' }, { value: 3, text: 'Separated' }],
                                                    }} handlevaluechange={handleValueChange}
                                                    />
                                                </Col>

                                            </Row>
                                            <Row>

                                                <Col>
                                                    <Input
                                                        {...{
                                                            name: 'fatherName',
                                                            label: 'Father Name',
                                                            value: values['fatherName'],
                                                            error: errors['fatherName'], touched: touched['fatherName']
                                                        }}
                                                        handlevaluechange={handleValueChange}
                                                    />
                                                </Col>
                                                <Col>
                                                    <Input
                                                        {...{
                                                            name: 'personalEmail',
                                                            label: 'Personal Email',
                                                            value: values['personalEmail'],
                                                            error: errors['personalEmail'], touched: touched['personalEmail']
                                                        }}
                                                        handlevaluechange={handleValueChange}
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

export default PersonalInfoEdit
