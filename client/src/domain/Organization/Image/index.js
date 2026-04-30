import { notifySaved } from 'components/alert/Toast';
import EmployeeSearch from 'domain/EmployeeSearch';
import { Form, Formik } from 'formik';
import queryString from 'query-string';
import React, { Fragment, useState } from 'react';
import { Button, Card, CardBody, CardTitle, Col, Label, Row } from 'reactstrap';
import ImageService from 'services/ImageService';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';

export default function UserProfile(props) {
    const [selEmp, setSelEmp] = useState({})
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;

    const handleSubmit = async (values, actions) => {

        const formData = new FormData();
        formData.append('employeeId', values.employeeId)
        formData.append('imageName', values.no)
        formData.append('file', values.file)

        if (rid) {
            formData.append('id', rid)
            await ImageService.UpdateAsync(formData).then((res) => {
                notifySaved();
                props.history.push()
            }).catch((err) => {
                formUtil.displayFormikError(err, actions);
            })
        } else {
            await ImageService.PostAsync(formData).then((res) => {
                notifySaved();
                props.history.push()
            }).catch((err) => {
                formUtil.displayFormikError(err, actions);
            })
        }
    }
    const validationSchema = Yup.object({
        employeeId: Yup.string().required('Employee Name is required !'),
    })

    return <Fragment>
        <Card>
            <CardBody>
                <CardTitle><Label htmlFor='imageUpload'> Image Upload</Label> </CardTitle>
                <Formik
                    initialValues={{
                        employee: '',
                        file: '',
                        no: ''
                    }}
                    validationSchema={validationSchema}
                    onSubmit={(values, actions) => handleSubmit(values, actions)}
                >
                    {({ values, errors, setFieldValue, touched, isSubmitting }) => {
                        const handleValueChange = async (name, value, { selected }) => {
                            setFieldValue(name, value);
                        }
                        return (<Form>
                            <Row>
                                <Col md='4'>
                                    <Label htmlFor='employee'>Employee Name</Label>
                                    <EmployeeSearch name={'employeeNo'} selEmp={selEmp} setSelEmp={setSelEmp} handleValueChange={handleValueChange} />
                                    <p style={{ color: 'red' }}>{errors.employeeId}</p>
                                </Col>
                            </Row>
                            <Row className='mb-4'>
                                <Col md='4'>
                                    <Label>Select File</Label>
                                    <input id="file" name="file" type="file" enctype='multipart/form-data' onChange={(event) => {
                                        setFieldValue("file", event.currentTarget.files[0]);
                                    }} className="form-control" />
                                </Col>
                            </Row >
                            <Row >
                                <Button className="mb-4  ml-3 btn-icon btn-success1" type='submit' disabled={isSubmitting}>{isSubmitting ? "Please Wait..." : "Submit"} </Button>
                                <Button className="mb-4  ml-3 btn-icon btn-danger" disabled={isSubmitting} type='reset'>Cancel</Button>
                            </Row>
                        </Form>
                        );
                    }}
                </Formik>
            </CardBody>
        </Card>
    </Fragment>
}