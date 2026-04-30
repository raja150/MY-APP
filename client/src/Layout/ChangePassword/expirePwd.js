import { notifySaved } from 'components/alert/Toast';
import { Field, Form, Formik } from 'formik';
import PageHeader from 'Layout/AppMain/PageHeader';
import React, { Fragment, useState } from 'react';
import PasswordChecklist from "react-password-checklist";
import { Button, Card, CardBody, CardSubtitle, Col, FormGroup, Label, Row } from 'reactstrap';
import * as Yup from "yup";
import APIService from '../../services/apiservice';
import * as formUtil from 'utils/form'
import { Base64 } from 'js-base64';
import SessionStorageService from 'services/SessionStorage';
import queryString from 'query-string'
import * as crypto from 'utils/Crypto'

function ExpirePassword(props) {

    const [loading, setLoad] = useState(false);

    const user = SessionStorageService.getPasswordExp()
    const userName = SessionStorageService.getUserName()

    const token = queryString.parse(props.location.search);
    const ValidationSchema = Yup.object().shape({
        newPassword: Yup.string().required("This field is required"),
        retypePassword: Yup.string().when("newPassword", {
            is: val => (val && val.length > 0 ? true : false),
            then: Yup.string().oneOf(
                [Yup.ref("newPassword")],
                "Both password should  be the same"
            ).required("Re-enter password")
        })
    });

    const handleSubmit = async (values, actions) => {
        setLoad(true);
        const data = {
            newPassword: Base64.encode(values.newPassword),
            token: user ? user.token : token['r'],
        };
        await APIService.putAsync('Auth/ChangePwd', data).then((response) => {
            notifySaved('Password updated successfully');
            SessionStorageService.removeExpireUserInfo()
            props.history.push('/Login');
        }).catch((err) => {
            formUtil.displayAPIError(err)
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
                                validPassword: true
                            }}
                            validationSchema={ValidationSchema}
                            onSubmit={async (values, actions) => { handleSubmit(values, actions) }}
                        >
                            {({ formik, touched, errors, actions, isSubmitting, values, setFieldValue }) => {
                                const handleValueChange = async (name, value) => {
                                    setFieldValue(name, value)
                                }
                                return (
                                    <Form>
                                        <PageHeader title="Update Your Password" />
                                        <CardSubtitle>
                                            You need to update your password because this is the first time you are signing in,<br />
                                            or because your password has expired.
                                        </CardSubtitle>
                                        <CardBody>
                                            <Row className='mb-4'>
                                                <Col md='6'>
                                                    <Row >
                                                        <CardSubtitle>Please enter your new password below.</CardSubtitle>
                                                        <FormGroup>
                                                            <Label for="newPassword">New Password :</Label>
                                                            <Field type="password" name="newPassword" className={`form-control ${touched.newPassword && errors.newPassword ? "is-invalid" : ""}`} placeholder="Enter New Password" />
                                                        </FormGroup>
                                                    </Row>
                                                    <Row >
                                                        <FormGroup>
                                                            <Label for="retypePassword">Confirm New Password :</Label>
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
                                            <Button className='mt-2 btn btn-success font-bold' disabled={!values.validPassword || loading} type="submit" name="save">{loading ? "Please Wait..." : "Change my password"}</Button>
                                            <Button className='mt-2 ml-2 btn btn-danger font-bold' type='button' onClick={handleClick}>Cancel</Button>
                                        </CardBody>
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
export default ExpirePassword
