import { notifySaved } from 'components/alert/Toast';
import { Input } from 'components/dynamicform/Controls';
import { Form, Formik } from 'formik';
import React, { Fragment, useState } from 'react';
import { Button, Card, CardBody, CardSubtitle, CardTitle, Col, Row } from 'reactstrap';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import APIService from '../../../services/apiservice';
import SessionStorageService from 'services/SessionStorage';

function CheckUser(props) {

    const [loading, setLoading] = useState(false)
    const appUrl = window.location.origin;
    const handleSubmit = async (values, actions) => {
        setLoading(true)
        const data = {
            userName: values.userName,
            appUrl: appUrl
        }
        await APIService.putAsync(`Auth/SendMail`, data).then((response) => {
            notifySaved("Please go and check your mail");
            SessionStorageService.removeEmpInfo();
            setTimeout(function(){
                window.close();
             }, 3000);
        }).catch(e => {
            formUtil.displayFormikError(e, actions)
        })
        setLoading(false)
    }

    const handleClick =  () => {
        window.close();
    };
    return (
        <Fragment>
            <div className='d-flex align-items-center justify-content-center p-4 h-100 bg-dark'>

                <Card className='animate__animated animate__zoomIn'>
                    <CardBody>
                        <Formik
                            initialValues={{
                                userName: ''
                            }}
                            validationSchema={Yup.object().shape({
                                userName: Yup.string().required("User name is required !")
                            })}
                            onSubmit={(values, actions) => { handleSubmit(values, actions) }}
                        >
                            {({ values, errors, setFieldValue, touched, setFieldError }) => {
                                const handleValueChange = async (name, value) => {
                                    setFieldValue(name, value);
                                }

                                return (
                                    <Form>
                                        <CardTitle>Forgot Password?</CardTitle>
                                        <CardSubtitle>Enter your User Name we will send you a link to reset your password.</CardSubtitle>
                                        <Row>
                                            <Col md=''>
                                                <Input {...{
                                                    name: 'userName', label: 'User Name', type: 'string',
                                                    value: values['userName'], error: errors['userName'],
                                                    touched: touched['userName']
                                                }} handlevaluechange={handleValueChange} />

                                            </Col>
                                        </Row>
                                        <Button className='mt-2 btn btn-success font-bold' type='submit' color='success' disabled={loading}>{loading ? 'Please wait' : 'Submit'}</Button>
                                        <Button className='mt-2 ml-2 btn btn-success font-bold' type='button' color='danger' onClick={()=>handleClick()}>Cancel</Button>

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
export default CheckUser