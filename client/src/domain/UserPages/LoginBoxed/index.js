import { Form, Formik } from 'formik';
import _ from 'lodash';
import React, { useState } from 'react';
import { Button, Card, CardBody, CardTitle, FormGroup, Input, Label } from 'reactstrap';
import * as Yup from 'yup';
import SettingsService from '../../../services/Settings';
import SessionStorageService from 'services/SessionStorage';


function LoginBoxed(props) {
    const [state, setState] = useState({
        isInvalid: false,
        isLoading: true
    })

    const backgroundStyle = {
        background: '#061f3a',
        height: '100vh',

    }
    const SignupSchema = Yup.object().shape({
        password: Yup.string().required('Required'),
        username: Yup.string().required('Required'),
    });

    const handleSubmit = async (values) => {
       
        if (values.username !== '' && values.password !== '') {
            setState({ submitted: true });
            setState({ invalid: false });

            try {
                await SettingsService.postLoginBox(values.username, values.password).then((response) => {
                    if (response.data) {
                        SessionStorageService.setUserLogin(response.data.jwtToken, response.data.refreshToken, response.data.name,response.data.empCode, response.data.userId, response.data.roleId)
                        props.history.push('/Dashboard/User')

                    }
                });
            }
            catch (error) {
                setState({ invalid: true });
            }
        }
    }

    return (
        <div className="h-100 w-100 p-0" style={{ position: 'fixed' }}>
            <Formik
                initialValues={{
                    username: '',
                    password: ''
                }}
                validationSchema={SignupSchema}
                onSubmit={(values) => handleSubmit(values)}
            >
                {({ values, setFieldValue, errors }) => {
                    const handleChange = (e) => {
                        const { name, value } = e.target;
                        setFieldValue(name, value)
                    }

                    return (
                        <Form>
                            <div className='d-flex align-items-center justify-content-center' style={backgroundStyle}>
                                <Card style={{ minWidth: '350px', maxWidth: '450px', width: '100%' }}>
                                    <CardBody>

                                        <CardTitle><h5>Sign In</h5></CardTitle>
                                        <FormGroup>
                                            <Label for="email">Employee Code</Label>
                                            <Input type="text" class={`form-control ${(!_.isEmpty(errors) && errors.username) ? 'is-invalid' : ''}`} name='username' placeholder="Your Email or Username"
                                                onChange={e => handleChange(e)} value={values.username} />
                                        </FormGroup>
                                        <FormGroup>
                                            <Label for="password">Password</Label>
                                            <Input type="password" class={`form-control ${(!_.isEmpty(errors) && errors.password) ? 'is-invalid' : ''}`} name='password' placeholder="Your Password "
                                                onChange={e => handleChange(e)} value={values.password} />
                                        </FormGroup>
                                        <FormGroup>
                                            <Button color='success' type='submit' name='submit'>Login</Button>
                                        </FormGroup>
                                        <div class="form-group">
                                            <a href="#" class="ForgetPwd">Forgot Password?</a>
                                        </div>
                                        {
                                            state.invalid ? <div className='text-danger'>Invalid username and password</div> : ''
                                        }

                                    </CardBody>
                                </Card>
                            </div>
                        </Form>
                    )
                }}
            </Formik>


        </div>
    )
}

export default LoginBoxed
