import axios from 'axios';
import { notifyError } from 'components/alert/Toast';
import { Form, Formik } from 'formik';
import { Base64 } from 'js-base64';
import _ from 'lodash';
import React, { useEffect, useState } from 'react';
import { Button, CardTitle, FormGroup, Input, Label } from 'reactstrap';
import SessionStorageService from 'services/SessionStorage';
import * as appUtil from 'utils/app';
import * as Yup from 'yup';
import bg5 from '../../../assets/utils/images/originals/bg-01.jpg';
import SettingsService from '../../../services/Settings';
import './style.css';

function Index1(props) {
    const [state, setState] = useState({
        invalid: false,
        geoLoc: {},
        isLoading: true
    });

    const SignupSchema = Yup.object().shape({
        password: Yup.string().required('Required'),
        username: Yup.string().required('Required'),
    });
    useEffect(() => {
        const fetchData = async () => {
            await axios.get(process.env.REACT_APP_GEO_LOCATION).then((res) => {
                setState({ ...state, geoLoc: res.data, isLoading: false })
            })
        }
        fetchData();
        document.title = appUtil.pageTitle("IHMS - Avontix");
    }, [])

    const handleSubmit = async (values) => {

        const data = {
            username: values.username,
            password: Base64.encode(values.password),
            geoLocation: JSON.stringify(state.geoLoc),
            state: state.geoLoc.state,
            ipAddress: state.geoLoc.IPv4,
            country: state.geoLoc.country_name,
            city: state.geoLoc.city
        }
        if (values.username !== '' && values.password !== '') {
            try {
                await SettingsService.postLoginBox(data).then((response) => {

                    if (response.data) {
                        if (response.data.errorCode == 5) {
                            return notifyError('Your account is locked.')
                        }
                        if (response.data.roleId && response.data.jwtToken) {
                            SessionStorageService.setUserLogin(response.data.jwtToken, response.data.refreshToken, response.data.name, response.data.empCode, response.data.userId, response.data.roleId)
                            props.history.push('/Dashboard/User')
                        } else {
                            SessionStorageService.setPwdExpire(response.data.jwtToken, values.username)
                            props.history.push('/ExpirePassword')
                        }
                    }
                });
            }
            catch (error) {
                setState({ ...state, invalid: true });
            }
        }
    }

    return <Formik
        initialValues={{
            username: '',
            password: ''
        }}
        validationSchema={SignupSchema}
        onSubmit={(values) => handleSubmit(values)}
    >
        {({ values, setFieldValue, errors, isSubmitting }) => {
            const handleChange = (name, value) => {
                setFieldValue(name, value)
            }
            return (
                <Form>
                    <div className="w-100 mx-0 my-auto" style={{ position: 'fixed' }}>
                        <div className="container-login100">
                            <div className="wrap-login100 align-items-center">
                                <div className='text-center'><h2 color='#3ac47d'>Avontix Integrated Human Resource Management System(IHMS)</h2></div>
                                <div className="login100-pic flex-grow-1" data-tilt>
                                    <img src={bg5} alt="IMG" />
                                </div>

                                <div className="flex-grow-1">
                                    <CardTitle className='text-center mb-3'> </CardTitle>
                                    <FormGroup>
                                        <Label for="username">Username</Label>
                                        <Input  autoComplete="nope" className={`form-control ${(!_.isEmpty(errors) && errors.username) ? 'is-invalid' : ''}`} type="text" name="username" id="username" value={values.username} onChange={(e) => handleChange(e.target.name, e.target.value)} placeholder="Enter Username" />
                                    </FormGroup>
                                    <FormGroup>
                                        <Label for="exampleEmail">Password</Label>
                                        <Input  autoComplete="nope" className={`form-control ${(!_.isEmpty(errors) && errors.password) ? 'is-invalid' : ''}`} type="password" name="password" id="password" value={values.password} onChange={(e) => handleChange(e.target.name, e.target.value)} placeholder="Enter Password" />
                                    </FormGroup>
                                    {
                                        state.invalid ? <div className='text-danger'>Invalid Credentials</div> : ''
                                    }
                                    <div className="mt-3">
                                        <Button color='success' className={`w-100`} disabled={isSubmitting}>
                                            {isSubmitting ? 'Please wait...' : "Login"}
                                        </Button>
                                    </div>
                                    <div className="mt-3">
                                        {`${process.env.REACT_APP_API_ENV} | Ver ${process.env.REACT_APP_VERSION_NO} Copyright © Avontix Global 2023`}
                                    </div>
                                    
                                    {/* <div className="text-left mt-3">
                                        <a href="#/forgot" target='_blank'>
                                            Forgot Password ?
                                        </a>
                                    </div> */}
                                </div>
                            </div>
                        </div>
                    </div>
                </Form>
            )
        }}
    </Formik>
}

export default Index1