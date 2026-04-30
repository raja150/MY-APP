import { Input, RWDropdownList, TextAreaInput, Upload } from "components/dynamicform/Controls";
import { Button, Col, Row } from "reactstrap";
import React, { useState } from 'react'
import TicketService from "services/SelfService/Ticket";
import { FileDownload } from "domain/Setup/DataImport/file-download";

export default function NewTicket(props) {
    const { values, isSubmitting, errors, touched, handleValueChange,
        rid, topics, departments, subTopics, file, selfService = false } = props

    const [loading, setLoad] = useState(false)

    const handleDownload = async () => {
        setLoad(true)
        await TicketService.Download(rid, file).then((res) => {
            FileDownload(res.data, `${file}`, res.headers['content-type']);
        })
        setLoad(false)
    }
    return <>
        <Row>
            <Col md='6'>
                <RWDropdownList {...{
                    name: 'departmentId', label: 'Department',
                    value: values['departmentId'],
                    error: errors['departmentId'], touched: touched['departmentId'],
                    textField: 'department', valueField: 'id',
                    values: departments, className: 'form-control',

                }} handlevaluechange={handleValueChange} />
            </Col>
            <Col md='6'>
                <RWDropdownList {...{
                    name: 'helpTopicId', label: 'Help Topic', textField: 'name', valueField: 'id',
                    value: values['helpTopicId'], values: topics,
                    error: errors['helpTopicId'], touched: touched['helpTopicId'],
                    className: 'form-control',

                }} handlevaluechange={handleValueChange} />
            </Col>
        </Row>
        <Row>
            <Col md='6'>
                <RWDropdownList {...{
                    name: 'subTopicId', label: 'Sub Topic', values: subTopics,
                    required: true, value: values['subTopicId'],
                    error: errors['subTopicId'], touched: touched['subTopicId'],
                    textField: 'subTopic', valueField: 'id',
                    className: 'form-control',

                }} handlevaluechange={handleValueChange} />
            </Col>
            <Col md='6'>
                <Input {...{
                    name: 'subject', label: 'Issue Summary ',
                    value: values['subject'], error: errors['subject'],
                    touched: touched['subject']
                }} handlevaluechange={handleValueChange} />
            </Col>
        </Row>
        <Row>
            <Col md='12'>
                <TextAreaInput {...{
                    name: 'message', label: 'Issue Details',
                    required: true, value: values['message'],
                    error: errors['message'], touched: touched['message'],
                    className: 'form-control',

                }} handlevaluechange={handleValueChange} />
            </Col>
        </Row>
        <Row>
            <Col md='1'>
                <Button className="mb-3 mr-2 btn-icon btn-success1" key='button' color="success" type="submit" name="save" disabled={isSubmitting}>
                    {isSubmitting ? "Please Wait..." : "Submit"}
                </Button>
            </Col>
            <Col md='3'>
                {rid && file ?
                    <div className='d-flex'>
                        <Button type='button' className="mb-2 mr-2 btn-icon btn-primary" key='button' color="success" name="save"
                            onClick={() => handleDownload()} disabled={loading} >
                            {loading ? "Please wait..." : "Open File"}
                        </Button>
                    </div>
                    : !rid ?
                        <Upload {...{
                            name: 'file', label: 'Upload Attachment',
                            value: values['file'], error: errors['file'],
                            touched: touched['file']
                        }} handlevaluechange={handleValueChange} />
                        : ''
                }
            </Col>
        </Row>
    </>
}