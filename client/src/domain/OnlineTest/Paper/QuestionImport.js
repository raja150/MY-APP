import { Saving } from 'components/Loader';
import ErrorTable from 'domain/Setup/DataImport/ErrorTable';
import { FileDownload } from 'domain/Setup/DataImport/file-download';
import React, { Fragment, useState } from 'react';
import { Button, Col, Label, Row } from 'reactstrap';
import APIService from 'services/apiservice';

const QuestionImport = (props) => {
    const { setFieldValue, setImportQuestion, importQuestion,
        values, handleImportQue, columns, data, loading } = props;

    const sampleFile = (e) => {
        e.preventDefault();
        APIService.getBlobAsync(`DataImport/Sample?type=Question`).then(r => {
            FileDownload(r.data, 'Question.xlsx', r.headers['content-type'])
        })
    }

    return <>
        <Row>
            <Col>
                <p>Note: Please import only .xlsx(MS Office 2007 and Above) format.</p>
                <p>Download a <a href="#" onClick={e => sampleFile(e)}>sample</a> excel file and compare it with your import file to ensure that it is ready to import.</p>
            </Col>
        </Row>
        <Row form>
            <Col md='3'>
                <Label>Select File</Label>
                <input id="file" name="file" type="file" encType='multipart/form-data' onChange={(event) => {
                    setFieldValue("file", event.currentTarget.files[0]);
                }} className="form-control" />
            </Col>
            <Col style={{ marginTop: '20px' }}>
                <Button className="mr-2 btn-icon btn-success1" type='button' disabled={values['file'] === '' || loading.isBusy}
                    onClick={() => handleImportQue(values)}>{loading.type === 'Import' ? 'Please wait...' : 'Import'}</Button>
                <Button className="btn-icon btn-danger" type='button' disabled={loading.isBusy}
                    onClick={() => { setImportQuestion(!importQuestion); setFieldValue('file', '') }}>Cancel</Button>
            </Col>
        </Row>
        {columns.length > 0 && <Row className='mt-2'>
            <Col>
                <ErrorTable columns={columns} data={data} />
            </Col>
        </Row>}
    </>
};

export default QuestionImport;