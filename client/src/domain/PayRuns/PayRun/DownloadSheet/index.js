import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faDownload } from '@fortawesome/free-solid-svg-icons';
import { RWDropdownList } from 'components/dynamicform/Controls';
import { Formik, Form } from 'formik';
import React, { Fragment } from 'react';
import { Card, CardBody, CardTitle, Col, Row } from 'reactstrap';
import { notifySaved } from 'components/alert/Toast';
import * as formUtil from 'utils/form'
import APIService from 'services/apiservice'
import { FileDownload } from 'domain/Setup/DataImport/file-download'

const DownloadFiles = [
    { value: 1, text: 'Register Of Leaves', type: 'Register_Of_Leaves' },
    { value: 2, text: 'Weekly Holidays', type: 'Weekly_Holidays' },
    { value: 3, text: 'Register for Payment of Wages', type: 'Payment_Of_Wages' },
    { value: 4, text: 'Register of Employment', type: 'Register_Of_Employment' },
    { value: 5, text: 'Register For Equal Remuneration', type: 'Register_For_Equal_Remuneration' },
    { value: 6, text: 'PF Upload Format', type: 'Pf_upload' },
    { value: 7, text: 'ESI Template', type: 'esi' },
    { value: 8, text: 'Bank Sheet', type: 'Bank_Sheet' },
]
export default function AppendToExcel(props) {
    let formValues = {
        downloadFile: '',
        fileName: ''
    }
    const handleSubmit = (values, actions) => {

    }
    const DownloadExcelSheet = async (values) => {
        APIService.getBlobAsync(`PayRoll/AppendToExcel/Download/${values.downloadFile}/${props.rid}`).then(r => {
            FileDownload(r.data, `${values.downloadFile}.xlsx`, r.headers['content-type']);
            notifySaved("File Downloaded Successfully")
        }).catch((err)=>{
            formUtil.displayErrors(err)
        });
    }
    return (
        <Fragment>
            <Card>
                <CardBody>
                    <Formik
                        initialValues={formValues}
                        onSubmit={(values, actions) => handleSubmit(values, actions)}
                    >
                        {({ values, errors, touched, setFieldValue, isSubmitting }) => {
                            const handleValueChange = async (name, value) => {
                                let fileName = DownloadFiles.find(x => x.type == value)
                                if (fileName) {
                                    setFieldValue('fileName', fileName.text)
                                }
                                setFieldValue(name, value)
                            }
                            return (
                                <Form>
                                    <CardTitle>DownLoad Sheet</CardTitle>
                                    <Row>
                                        <Col md='4'>
                                            <RWDropdownList {...{
                                                name: 'downloadFile', label: 'Select File Name', valueField: 'type',
                                                error: errors['downloadFile'], touched: touched['downloadFile'],
                                                value: values['downloadFile'], values: DownloadFiles
                                            }} handlevaluechange={handleValueChange}
                                            />
                                        </Col>
                                    </Row>
                                    {values.downloadFile ? <>
                                        <p>Please click on the download button to download the <b>{values.fileName}</b> file
                                            <FontAwesomeIcon icon={faDownload} size='2x' style={{marginLeft:'15px'}} onClick={() => DownloadExcelSheet(values)} /> </p>
                                    </> : <div />}
                                </Form>
                            )
                        }}
                    </Formik>
                </CardBody>
            </Card>
        </Fragment>
    )
}