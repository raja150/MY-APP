import { Input, RWDropdownList } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import { Form, Formik } from 'formik';
import React, { Fragment, useState } from 'react';
import { Button, Card, CardBody, Col, Modal, ModalBody, ModalHeader, Row } from 'reactstrap';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import * as _ from 'lodash'
import UpdateProfileService from 'services/Org/UpdateProfileService';

export default function AddContact(props) {
    const [state, setState] = useState({
        formValues: !_.isEmpty(props.rowData) ? props.rowData : {
            personName: '', humanRelation: '', contactNo: ''
        }
    })

    const handleSubmit = async (values, actions) => {
        const data = values;
        if (props.rowData) {
            data['id'] = props.rowData.id;
        }
        await UpdateProfileService.AddContact(data).then((res) => {
            props.fetchdata();
            props.setRowData({})
            props.toggle();
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }

    const validationSchema = Yup.object({
        personName: Yup.string().required("Person Name is required"),
        humanRelation: Yup.string().required("Human Relation is required"),
        contactNo: Yup.string().required("Contact Number is required"),

    });
    const handleCancel = () => {
        props.toggle()
        props.setRowData({})
    }
    return <>
        <Fragment>
            <Card>
                <CardBody>
                    <Modal isOpen={true} >
                        <ModalBody>
                            <ModalHeader>Contact Details</ModalHeader>
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
                                        state.isLoading ? <Loading /> :
                                            <Form>
                                                <Row>
                                                    <Col>
                                                        <Input {...{
                                                            name: 'personName', label: 'Person Name',
                                                            value: values['personName'],
                                                            error: errors['personName'],
                                                            touched: touched['personName'],
                                                        }} handlevaluechange={handleValueChange} />
                                                    </Col>
                                                </Row>
                                                <Row>
                                                    <Col>
                                                        <RWDropdownList {...{
                                                            name: 'humanRelation', label: 'Relationship', valueField: 'value',
                                                            value: values['humanRelation'],
                                                            values: [{ value: 1, text: 'Father' }, { value: 2, text: 'Mother' },
                                                            { value: 3, text: 'Spouse' }, { value: 4, text: 'Child 1' },
                                                            { value: 5, text: 'Child 2' }, { value: 6, text: 'Child 3' }], 
                                                            error: errors['humanRelation'], touched: touched['humanRelation'],
                                                            className: 'form-control'
                                                        }} handlevaluechange={handleValueChange} />
                                                    </Col>
                                                </Row>
                                                <Row>
                                                    <Col>
                                                        <Input {...{
                                                            name: 'contactNo', label: 'Contact No',
                                                            value: values['contactNo'],
                                                             error: errors['contactNo'], touched: touched['contactNo']
                                                        }} handlevaluechange={handleValueChange} />
                                                    </Col>

                                                </Row>
                                                <hr />

                                                <Row className='justify-content-center'>
                                                    <Button className="mb-2 mr-2 btn-icon btn-success" key='Save' color="success" name="save" disabled={isSubmitting}>
                                                        {isSubmitting ? "Please Wait..." : "Save"}
                                                    </Button>
                                                    <Button className="mb-2 mr-2 btn-icon btn-primary" key='Cancel' color="success" onClick={() => handleCancel()} name="Cancel" >
                                                        {"Cancel"}
                                                    </Button>
                                                </Row>

                                            </Form>
                                    )
                                }}
                            </Formik>
                        </ModalBody>

                    </Modal>
                </CardBody>
            </Card>
        </Fragment>
    </>
}