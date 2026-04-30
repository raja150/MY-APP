import camelcase from 'camelcase'
import { notifyError, notifySaved } from 'components/alert/Toast'
import { RWDatePicker, RWDropdownList } from 'components/dynamicform/Controls'
import { FileDownload } from 'domain/Setup/DataImport/file-download'
import { Form, Formik } from 'formik'
import React, { Fragment, useState } from 'react'
import { useTable } from 'react-table'
import { Button, Card, CardBody, CardTitle, Col, Label, Row } from 'reactstrap'
import APIService from 'services/apiservice'
import * as Yup from 'yup'
import * as dateUtil from "../../../utils/date"
import * as formUtil from 'utils/form';
import './style.css'
import moment from 'moment'
import ErrorTable from './ErrorTable'


function DataImport(props) {
    const { IMPORT_TYPES } = props
    const [state, setState] = useState({ columns: [], data: [] });
    const validationSchema = Yup.object({
        uploadType: Yup.string().required('Upload file is required !'),
        date: Yup.string().when('haveDate', {
            is: (val) => val == true,
            then: Yup.string().required('Date is required !')
        })
    })
    let formValues = {
        uploadType: '',
        file: '',
        haveDate: false,
        date: ''
    }
    const sampleFile = async (e, type) => {
        e.preventDefault();
        APIService.getBlobAsync(`DataImport/Sample?type=${type}`).then(r => {
            FileDownload(r.data, `${type}.xlsx`, r.headers['content-type']);
        });
    }
    const handleSubmit = async (values, actions) => {
        var formData = new FormData();
        //Find type form list
        const type = IMPORT_TYPES.find(x => x.type == values.uploadType);
        actions.setSubmitting(true);
        if (type) {
            formData.append('fileName', values.uploadType)
            formData.append('formFile', values.file)
            formData.append('date', values.date)

            await APIService.postFormDataAsync(`DataImport/${type.postURL}`, formData)
                .then((result) => {
                    if (result.data.hasNoError && result.data.headers && result.data.headers.length > 0) {
                        const columns = result.data.headers.map(f => {
                            return { Header: f.name, accessor: f.propertyName }
                        })
                        setState({ columns: columns, data: result.data.returnValue });
                    }
                    if (result.data.returnValue === null && result.status === 200) {
                        notifySaved("Data imported Successfully")
                        setTimeout(function () {
                            window.location.reload();
                        }, 2500);
                        actions.setFieldValue('uploadType', '')
                        actions.setFieldValue('date', '')
                        let input = document.querySelector('input[type="file"]');
                        input.outerHTML = input.outerHTML;
                        setState({ columns: [], data: [] });
                    }
                    if (result.data.hasError) {
                        if (result.data && result.data.messages && result.data.messages.length > 0) {
                            notifyError(result.data.messages[0].description);
                        }
                        const columns = result.data.headers.map(f => {
                            return { Header: f.name, accessor: f.propertyName }
                        })
                        setState({ columns: columns, data: result.data.returnValue });
                    }


                })
                .catch((error) => {
                    formUtil.displayErrors(error);
                });
        }
        actions.setSubmitting(false);
    }
    // const {
    //     getTableProps,
    //     getTableBodyProps,
    //     headerGroups,
    //     rows,
    //     prepareRow,
    // } = useTable({ columns: state.columns, data: state.data })
    return (

        <Fragment>
            <Card>
                <CardBody>
                    <Formik
                        initialValues={formValues}
                        validationSchema={validationSchema}
                        onSubmit={(values, actions) => handleSubmit(values, actions)}
                    >
                        {({ values, errors, touched, setFieldValue, isSubmitting }) => {
                            const handleValueChange = async (name, value) => {
                                //Update type is component state
                                if (name === 'uploadType') {
                                    const type = IMPORT_TYPES.find(x => x.type == value);
                                    if (type) {
                                        if (type.haveDate) {
                                            setFieldValue('haveDate', true)
                                        } else {
                                            setFieldValue('haveDate', false)
                                        }
                                    }
                                }
                                setFieldValue(name, value)
                            }
                            return (
                                <Form>
                                    <CardTitle><Label htmlFor='dataImport'> Data Import</Label> </CardTitle>
                                    <Row className='mb-4'>
                                        <Col md='4'>
                                            <RWDropdownList {...{
                                                name: 'uploadType', label: 'Upload File Selection',
                                                error: errors['uploadType'], touched: touched['uploadType'],
                                                valueField: 'type', value: values['uploadType'], values: IMPORT_TYPES
                                            }} handlevaluechange={handleValueChange} />
                                        </Col>
                                        <Col md='4'>
                                            {values.haveDate ?
                                                <RWDatePicker {...{
                                                    name: 'date', label: 'Date', error: errors['date'], format: "MM/YY",
                                                    editFormat: "MM/YY", views: ["year", "decade", "century"],
                                                    // min: dateUtil.getToday(),
                                                    showDate: true, showTime: false,
                                                    value: values['date'], touched: touched['date'],
                                                }} handlevaluechange={handleValueChange}
                                                />
                                                : ''}
                                        </Col>
                                    </Row>
                                    {values.uploadType ? <>
                                        <p>Note: Please import only .xlsx(MS Office 2007 and Above) format.</p>
                                        <p>Download a <a href="#" onClick={x => sampleFile(x, values.uploadType)}>sample</a> excel file and compare it with your import file to ensure that it is ready to import.</p>
                                    </> : <div />}
                                    <Row>

                                    </Row>
                                    <Row className='mb-4'>
                                        <Col md='4'>
                                            <Label>Select File</Label>
                                            <input id="file" name="file" type="file" encType='multipart/form-data' onChange={(event) => {
                                                setFieldValue("file", event.currentTarget.files[0]);
                                            }} className="form-control" />
                                            {/* <Dropzone
                                                onDrop={handleDrop}
                                                accept="xlsx/*"
                                            >
                                                {({
                                                    getRootProps,
                                                    getInputProps,
                                                    isDragActive,
                                                    isDragAccept,
                                                    isDragReject
                                                }) => {
                                                    const additionalClass = isDragAccept
                                                        ? "accept"
                                                        : isDragReject
                                                            ? "reject"
                                                            : "";

                                                    return (
                                                        <div
                                                            {...getRootProps({
                                                                className: `dropzone ${additionalClass}`
                                                            })}
                                                        >
                                                            <input {...getInputProps()} />
                                                            <span>{isDragActive ? <FontAwesomeIcon icon={faUpload} size='10x' />
                                                                : <FontAwesomeIcon icon={faUpload} />}</span>
                                                            <p>Drop Files Herae, or click to Upload files</p>
                                                        </div>
                                                    );
                                                }}
                                            </Dropzone> */}

                                        </Col>
                                    </Row>
                                    <Button className="mb-2 mr-2 btn-icon btn-success1" type='submit' disabled={isSubmitting}>{isSubmitting ? "Please Wait..." : "Submit"}</Button>
                                    <Button className="mb-2 mr-2 btn-icon btn-danger" type='reset'>Cancel</Button>
                                </Form>
                            )
                        }}
                    </Formik>
                </CardBody>
            </Card>
            <ErrorTable
                columns={state.columns}
                data={state.data}
            />
        </Fragment>
    )
}
export default DataImport;