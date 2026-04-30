import { RWDatePicker } from 'components/dynamicform/Controls'
import { Form, Formik } from 'formik'
import React, { Fragment, useState } from 'react'
import { Button, Card, CardBody, Col, Label, Modal, ModalBody, Row } from 'reactstrap'
import * as  dateUtil from 'utils/date'
import * as Yup from 'yup'
import AttendanceService from 'services/LMAttendance/Attendance'
import { notifySaved } from 'components/alert/Toast'
import * as formUtil from 'utils/form'
import { FileDownload } from 'domain/Setup/DataImport/file-download'
import { actions } from 'react-table'

export default function DownloadAttendance(props) {
    const [loading, setLoading] = useState(false)
    const [finalized, setFinalized] = useState(false)
    const [modal, setModal] = useState(false);
    const handleDownload = async (values, actions) => {
        setLoading(true)
        await AttendanceService.DownloadAttendance(values.date).then(r => {
            FileDownload(r.data, `${'Attendance'}.xlsx`, r.headers['content-type']);
            notifySaved("Downloaded Successfully")
        }).catch(er => {
            formUtil.displayFormikError(er, actions)
        })
        setLoading(false)
    }

    const handleFinalized = () => {
        setModal(!modal)
    }

    const handleConfirm = async (values) => {
        setFinalized(true)
        setModal(!modal)
        await AttendanceService.Finalized(values.date).then(r => {
            notifySaved("Finalized Successfully")
        }).catch(er => {
            formUtil.displayErrors(er)
        })
        setFinalized(false)
    }
    return <Fragment>
        <Card>
            <CardBody>
                <Formik
                    initialValues={{
                        date: ''
                    }}
                    validationSchema={
                        Yup.object().shape({
                            date: Yup.string().required('Attendance month is required!')
                        })
                    }
                    onSubmit={(values, actions) => handleDownload(values, actions)}
                >
                    {({ values, errors, setFieldValue, touched }) => {
                        const handleValueChange = async (name, value) => {
                            setFieldValue(name, value);
                        }
                        return <Form>
                            <Row>
                                <Col md='3'>
                                    <RWDatePicker {...{
                                        name: 'date', label: 'Attendance Month', showDate: true, showTime: false,
                                        error: errors['date'], touched: touched['date'],
                                        value: values['date'], format: 'MM/YY',
                                        editFormat: "MM/YY", views: ["year", "decade", "century"],
                                        max: dateUtil.getToday(),
                                    }} handlevaluechange={handleValueChange} />
                                </Col>
                                <Col md='4'>
                                    <Row>
                                        <Col md='4'>
                                            <Button style={{ marginTop: '16px', padding: 6 }} color='success' disabled={loading ? true : false}>{loading ? 'Please wait' : 'Download'}</Button>
                                        </Col>
                                        <Col md='4'>
                                            <Button style={{ marginTop: '16px', padding: 6 }} color='danger' type='button'
                                                disabled={loading ? true : false} onClick={() => handleFinalized()}>{finalized ? 'Please wait' : 'Finalized'}</Button>
                                        </Col>
                                    </Row>
                                </Col>
                            </Row>
                            <Modal isOpen={modal}>
                                <ModalBody>
                                    <Col ms='4'>
                                        <Label>Are you sure! you want to Finalise ?</Label>
                                    </Col>
                                    <Row>
                                        <div style={{ paddingLeft: '30px' }} >
                                            <Button color='success' type='button'
                                                disabled={loading ? true : false} onClick={() => handleConfirm(values, actions)}>Yes</Button>
                                        </div>

                                        <div style={{ paddingLeft: '10px' }} >
                                            <Button color="warning" type="button" onClick={() => setModal(!modal)} >
                                                No
                                            </Button>
                                        </div>
                                    </Row>
                                </ModalBody>
                            </Modal>
                        </Form>
                    }}
                </Formik>
            </CardBody>
        </Card>
    </Fragment>
}