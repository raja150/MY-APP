import { notifySaved } from 'components/alert/Toast'
import { Form, Formik } from 'formik'
import React, { Fragment, useState } from 'react'
import { AsyncTypeahead } from 'react-bootstrap-typeahead'
import { Button, Col, Label, Row } from 'reactstrap'
import TicketLogService from 'services/HelpDesk/TicketLog'
import SearchService from 'services/Search'
import * as formUtil from 'utils/form'
import * as Yup from 'yup'
import { EDIT_RESPONSE, HELP_DESK_TICKETS } from '../navigation'
import TimeLine from 'domain/TimeLine'

export default function ReAssignTicket(props) {
    const [groupEmp, setGroupEmp] = useState([])
    const [selEmp, setSelEmp] = useState([])
    const [isSearching, setIsSearching] = useState(false)

    const handleSubmit = async (values, actions) => {
        const data = {
            ticketId: props.rid,
            assignedToId: values.employeeId,
            ticketNo: props.ticketNo,
            toWorkMail: values.workMail
        }
        await TicketLogService.ReAssign(data).then((res) => {
            notifySaved("Assigned successfully")
            props.history.push(HELP_DESK_TICKETS);

        }).catch((e) => [
            formUtil.displayFormikError(e, actions)
        ])
    }
    const messageReceive = (e) => {
        if (e.key === "response") {
            window.removeEventListener("storage", messageReceive)
            window.location.reload()
        }
    }
    return <Fragment>
        <Formik
            initialValues={{
                employeeId: ''
            }}
            validationSchema={
                Yup.object().shape({
                    employeeId: Yup.string().required('Employee is required !')
                })
            }
            onSubmit={(values, actions) => handleSubmit(values, actions)}
        >
            {({ values, errors, setFieldValue, isSubmitting, setFieldError }) => {

                return <Form>
                    <Row className='mb-2'>
                        <Col md='4'>
                            <Label>Employee</Label>
                            <AsyncTypeahead
                                name={`id`}
                                placeholder='Search with Employee Name'
                                onChange={async (e) => {
                                    if (e.length > 0) {
                                        let emp = e[0];
                                        setSelEmp(emp)
                                        setFieldValue(`workMail`, emp.workMail)
                                        setFieldValue(`employeeId`, emp.id)
                                        setFieldError('employeeId', '')
                                    }
                                    else {
                                        setFieldValue(`workMail`, '')
                                        setFieldValue(`employeeId`, '')
                                        setSelEmp({})
                                    }
                                }}

                                onSearch={async (query) => {
                                    setIsSearching(true);
                                    await SearchService.DeskGroupEmps(props.deptId, query).then((result) => {
                                        setGroupEmp(result.data)
                                    })
                                    setIsSearching(false);
                                }}

                                renderMenuItemChildren={(option) => {
                                    return [
                                        <div>
                                            <div><strong>Name : {option.name}</strong></div>
                                            <div>No : {option.no}</div>
                                            <div>Mail : {option.workMail}</div>
                                            <div>Designation : {option.designation}</div>
                                        </div>
                                    ]
                                }}
                                isLoading={isSearching}
                                options={groupEmp}
                                labelKey={option => `${option.name}`}
                                selected={selEmp && selEmp.id ? [selEmp] : []}
                                id='id'
                                clearButton
                            />
                            <p style={{ color: 'red' }}>{errors.employeeId}</p>
                        </Col>
                    </Row>
                    <Button disabled={isSubmitting}>{isSubmitting ? 'Please wait' : 'Re-assign'}</Button>
                    {props.comments.length > 0 && <Row className='ml-0 mt-3'>
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
    </Fragment >
}