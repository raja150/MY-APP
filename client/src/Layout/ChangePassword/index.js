import { notifySaved } from 'components/alert/Toast';
import { Field, Form, Formik } from 'formik';
import { Base64 } from 'js-base64';
import React, { Fragment, useState } from 'react';
import PasswordChecklist from "react-password-checklist";
import { Button, Card, CardBody, CardSubtitle, CardTitle, Col, FormGroup, Label, Row } from 'reactstrap';
import * as formUtil from 'utils/form';
import * as Yup from "yup";
import APIService from '../../services/apiservice';
import SessionStorageService from 'services/SessionStorage';

function ChangePassword(props) {

    const [loading, setLoad] = useState(false);
    const user = JSON.parse(SessionStorageService.getUser())
    const ValidationSchema = Yup.object().shape({
        oldPassword: Yup.string().required("This field is required"),
        newPassword: Yup.string().required("This field is required"),
        retypePassword: Yup.string().when("newPassword", {
            is: val => (val && val.length > 0 ? true : false),
            then: Yup.string().oneOf(
                [Yup.ref("newPassword")],
                "Both password need to be the same"
            ).required("Re-enter password")
        })
    });

    const handleSubmit = async (values, actions) => {
        setLoad(true);
        const data = {
            name: user.name,
            oldPassword: Base64.encode(values.oldPassword),
            newPassword: Base64.encode(values.newPassword),
            token: user.token
        };
        await APIService.putAsync('Auth', data).then((response) => {
            notifySaved();
            props.history.push('/Login');
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
        setLoad(false);
    }
    const handleClick = async () => {
        props.history.push('/Dashboard/User');
    };

    return (
        <Fragment>
            <div className='d-flex align-items-center justify-content-center p-4 h-100 bg-dark'>
                <Card className='animate__animated animate__zoomIn'>
                    <CardBody>
                        <Formik
                            initialValues={{
                                oldPassword: '',
                                newPassword: '',
                                retypePassword: '',
                                validPassword: false
                            }}
                            validationSchema={ValidationSchema}
                            onSubmit={async (values, actions) => { handleSubmit(values, actions) }}
                        >
                            {({ formik, touched, errors, values, setFieldValue }) => {
                                const handleValueChange = async (name, value) => {

                                    setFieldValue(name, value)
                                }
                                return (
                                    <Form>
                                        <CardTitle>Change Password</CardTitle>
                                        <CardSubtitle>
                                            Password must contains: Capital letters, Numbers or Symbols <br />
                                        </CardSubtitle>
                                        <Row className='mb-4'>
                                            <Col md='6'>
                                                <Row >
                                                    <FormGroup>
                                                        <Label htmlFor="oldPassword">Current Password</Label>
                                                        <Field type="password" name='oldPassword' className={`form-control ${touched.oldPassword && errors.oldPassword ? "is-invalid" : ""}`} placeholder="Enter Current Password" />
                                                    </FormGroup>
                                                </Row>
                                                <Row >
                                                    <FormGroup>
                                                        <Label htmlFor="newPassword">New Password</Label>
                                                        <Field type="password" name="newPassword" className={`form-control ${touched.newPassword && errors.newPassword ? "is-invalid" : ""}`} placeholder="Enter New Password" />
                                                    </FormGroup>
                                                </Row>
                                                <Row >
                                                    <FormGroup>
                                                        <Label htmlFor="retypePassword">Confirm Password</Label>
                                                        <Field type="password" name="retypePassword" className={`form-control ${touched.retypePassword && errors.retypePassword ? "is-invalid" : ""}`} placeholder="Enter New Password" />
                                                    </FormGroup>
                                                </Row>
                                            </Col>
                                            <Col md={6}>
                                                <PasswordChecklist
                                                    rules={["length", "specialChar", "number", "capital", "match"]}
                                                    minLength={5}
                                                    value={values.newPassword}
                                                    valueAgain={values.retypePassword}
                                                    onChange={(isValid) => {
                                                        handleValueChange('validPassword', isValid);
                                                    }}
                                                />
                                            </Col>
                                        </Row>


                                        <div class="error text-danger">
                                            {errors.retypePassword}
                                        </div>
                                        <Button className='mt-2 btn btn-success font-bold' disabled={!values.validPassword || loading} type='submit' name='save'>{loading ? "Please Wait..." : "Change"}</Button>
                                        <Button className='mt-2 ml-2 btn btn-danger font-bold' type='button' onClick={handleClick}>Cancel</Button>
                                    </Form>
                                )
                            }}

                        </Formik>

                    </CardBody>
                </Card>
            </div>

        </Fragment>
    )
}

export default ChangePassword
