import { Form, Formik } from 'formik';
import { Input } from 'components/dynamicform/Controls';
import { Button, Card, CardBody, Modal, ModalBody, ModalHeader, Row } from 'reactstrap';
import React, { useState } from 'react';
import Loading from 'components/Loader';
import * as _ from 'lodash';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import UpdateProfileService from 'services/Org/UpdateProfileService';


function ProfileEdit(props) {

    const [state, setState] = useState({
        isLoading: false, formValues: {
            ...props.data
        },
    })
    const validationSchema = Yup.object().shape({
        name: Yup.string().required("Employee Name is required"),

    });
    const handleSave = async (values, actions) => {
        const data = {
            name: values.name
        }
        if (props.data) {
            data['id'] = props.data.id;
        }
        await UpdateProfileService.PersonalInfoEdit(data).then((response) => {
            props.setProfileEditResult('empDetails', response.data)
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
                                        <ModalHeader>Profile</ModalHeader>
                                        <ModalBody>
                                            <Row style={{ marginLeft: '10px' }}>
                                                <Input
                                                    {...{
                                                        name: 'name',
                                                        label: 'Employee Name',
                                                        value: values['name'],
                                                        error: errors['name'], touched: touched['name']
                                                    }}
                                                    handlevaluechange={handleValueChange}
                                                />
                                            </Row>

                                            <Row className='justify-content-center'>
                                                <Button className="mb-2 mr-2 btn-icon btn-success" key='Save' color="success" name="save" disabled={isSubmitting}>
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
        </Card >
    )
}

export default ProfileEdit
