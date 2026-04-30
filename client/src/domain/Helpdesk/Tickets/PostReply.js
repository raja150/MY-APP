import Loading from 'components/Loader';
import { notifySaved } from 'components/alert/Toast';
import { Input, RWDropdownList } from 'components/dynamicform/Controls';
import RichTextArea from 'components/dynamicform/Controls/RichTextArea';
import { FileDownload } from 'domain/Setup/DataImport/file-download';
import TimeLine from 'domain/TimeLine';
import { FieldArray, Form, Formik } from 'formik';
import React, { useEffect, useState } from 'react';
import { Button, Col, Row } from 'reactstrap';
import TicketLogService from 'services/HelpDesk/TicketLog';
import TicketStsService from 'services/HelpDesk/TicketSts';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import TicketService from '../../../services/SelfService/Ticket';
import { EDIT_RESPONSE, HELP_DESK_TICKETS } from '../navigation';
import { Details } from './ConstValues';
import MailsList from './Mails';
import * as _ from 'lodash'
export function PostReply(props) {
    const [state, setState] = useState({ ticketSts: [], isLoading: true, })
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [loading, setLoad] = useState(false);

    useEffect(() => {
        fetchData();
    }, [])

    const fetchData = async () => {
        let ticketSts = []
        await TicketStsService.getList().then((res) => {
            ticketSts = _.orderBy(res.data, ['orderNo'], ['asc']);
        })
        setState({ ...state, ticketSts: ticketSts, isLoading: false })
    }

    const handleSubmit = async (values) => {
        setIsSubmitting(true)
        let items = []
        values.mails.length > 0 && values.mails.map((emp, i) => {
            if (emp.id) {
                items.push({
                    EmployeeId: emp.id,
                    workMail: emp.workMail
                })
            }
        })
        const data = {
            ticketId: props.details.id,
            response: values.response,
            ticketStatusId: values.ticketStsId,
            raisedByEmpMail: props.details.email,
            recipients: items,
            ticketNo: props.details.no
        }
        await TicketLogService.postAsync(data).then(res => {
            notifySaved();
            props.history.push(HELP_DESK_TICKETS);
        }).catch(err => {
            formUtil.displayAPIError(err)
        })
        setIsSubmitting(false)
    }
    const handleDownload = async () => {
        setLoad(true)
        await TicketService.Download(props.details.id, props.details.file).then((res) => {
            FileDownload(res.data, `${props.details.file}`, res.headers['content-type']);
        })
        setLoad(false)
    }
    const messageReceive = (e) => {
        if (e.key === "response") {
            window.removeEventListener("storage", messageReceive)
            window.location.reload()
        }
    }
    return <>
        {
            state.isLoading ? <Loading /> :
                <Formik
                    initialValues={{
                        to: '',
                        response: '',
                        userMails: state.mailsList,
                        mails: [Details()],
                        raiseById: props.details.raiseById,
                        ticketStsId: ''
                    }}
                    validationSchema={
                        Yup.object().shape({
                            ticketStsId: Yup.string().required('Status of ticket is required !'),
                            response: Yup.string().required('this field id required !')
                        })
                    }
                    onSubmit={(values, actions) => handleSubmit(values, actions)}
                >
                    {({ values, errors, setFieldValue, touched }) => {
                        const handleValueChange = async (name, value) => {
                            setFieldValue(name, value);
                        }
                        return <Form>
                            <Row>
                                <Col md='4'>
                                    <Input {...{
                                        name: 'to', label: 'To',
                                        error: false, touched: false,
                                        value: props.details.email, disabled: true
                                    }} />
                                </Col>
                            </Row>
                            <Row>
                                <Col md='5'>
                                    <table className="table table-bordered " >
                                        <thead>
                                            <tr>
                                                <th> Add Recipients</th>
                                                <th> Action </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <FieldArray
                                                name="mails"
                                                component={MailsList}
                                            />
                                        </tbody>
                                    </table>
                                </Col>
                            </Row>
                            <Row className='mb-2' >
                                <Col md='4'>
                                    <RWDropdownList {...{
                                        name: 'ticketStsId', label: 'Ticket Status', valueField: 'id', textField: 'name',
                                        value: values['ticketStsId'], values: state.ticketSts,
                                        error: errors['ticketStsId'], touched: touched['ticketStsId'],
                                    }} handlevaluechange={handleValueChange} />
                                </Col>
                                {props.details.file && <Col className='mt-3'>
                                    <Button type='button' className="mb-2 mr-2 btn-icon btn-primary" key='button' color="success" name="save"
                                        onClick={() => handleDownload()} disabled={loading} >
                                        {loading ? "Please wait..." : "Open File"}
                                    </Button>
                                </Col>}
                            </Row>
                            <Row className='mb-2'>
                                <Col>
                                    <RichTextArea {...{
                                        name: 'response', label: 'Response',
                                        value: values['response'],
                                        height: '200px',
                                        error: errors['response'], touched: touched['response'],
                                    }} handlevaluechange={handleValueChange} />
                                </Col>
                            </Row>
                            <div>
                                <Button className="mb-2 mr-2 btn-icon btn-success1" key='button' color="success" type="submit" name="save" disabled={isSubmitting ? true : false}>
                                    {isSubmitting ? "Please Wait..." : "Send"}
                                </Button>
                            </div>
                            {props.comments.length > 0 && <Row className='ml-0 mt-1'>
                                <TimeLine
                                    comments={props.comments}
                                    messageReceive={messageReceive}
                                    isEditable={true}
                                    path={EDIT_RESPONSE}
                                />
                            </Row>}
                        </Form>
                    }}
                </Formik>
        }
    </>
}