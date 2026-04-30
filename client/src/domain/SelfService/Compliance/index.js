import PageHeader from "Layout/AppMain/PageHeader";
import { notifySaved } from "components/alert/Toast";
import { Upload } from "components/dynamicform/Controls";
import { Form, Formik } from "formik";
import React, { Fragment, useEffect, useState } from "react";
import { Button, Card, CardBody, Col, Row } from "reactstrap";
import * as Yup from 'yup'
import ComplianceService from "services/SelfService/Compliances";
import * as formUtil from 'utils/form'
import Loading from "components/Loader";
import PdfReader from "domain/PdfView";

export default function Compliances() {
    const [state, setState] = useState({ isLoading: true, isUploaded: false });
    const [id, setId] = useState(null);
    const [data, setData] = useState(null);

    const fetchData = async () => { 
        let isUploaded = false
        await ComplianceService.isUploaded().then((res) => {
            if (res.data && res.status !== 204) {
                setData(res.data)
                isUploaded = true
            } 
        })
        setState({ ...state, isLoading: false, isUploaded: isUploaded })
    }

    useEffect(() => {
        fetchData();
    }, [id])

    const validationSchema = Yup.object({
        file: Yup.string().required("Attestation file is required"),
    })

    const handleSubmit = async (values) => {
        const data = new FormData()
        data.append('file', values.file)
        await ComplianceService.post(data).then((res) => {
            notifySaved('File uploaded successfully');  
            window.location.reload();
        }).catch((er) => {
            formUtil.displayAPIError(er)
        })
    }

    return <Fragment>
        <Card>
            <CardBody>
                {state.isLoading ? <Loading /> :
                    <div>
                        <PageHeader title={'Nimble Employee Attestation'} />
                        <Formik
                            initialValues={{
                                file: ''
                            }}
                            validationSchema={validationSchema}
                            onSubmit={(values) => handleSubmit(values)}
                        >
                            {({ values, errors, setFieldValue, touched, isSubmitting }) => {
                                const handleValueChange = async (name, value) => {
                                    setFieldValue(name, value)
                                }
                                return <Form>
                                    {state.isUploaded ? <Row className='text-center'>
                                        <Col md='12'  >
                                            <h6 style={{ color: 'blue' }}>Your attestation file has been received. <br />
                                                You can re-upload if you find anything wrong with the uploaded file. Thank you.</h6>
                                        </Col>
                                    </Row> : ''}
                                    <Row className="mb-2">
                                        <Col md='4'>
                                            <Upload {...{
                                                name: 'file', label: "Upload Your Attestation File",
                                                error: errors['file'], touched: touched['file'],
                                                values: values['file']
                                            }} handlevaluechange={handleValueChange} />
                                        </Col>
                                    </Row>
                                    <Row className="mb-2">
                                        <Col md='4'>
                                            <div>Only PDF format is acceptable.</div>
                                        </Col>
                                    </Row>
                                    <Button type="submit" disabled={isSubmitting}
                                        color="success">{isSubmitting ? "Please wait" : "Upload"}</Button>

                                    {state.isUploaded ? <PdfReader blobData={data} label="Your Attestation File" /> : ''}
                                </Form>
                            }}
                        </Formik>
                    </div>
                }
            </CardBody>
        </Card>
    </Fragment>
}