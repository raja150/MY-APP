import { notifySaved } from 'components/alert/Toast';
import { RWDropdownList, TextAreaInput } from 'components/dynamicform/Controls';
import { Form, Formik } from 'formik';
import React, { useEffect, useState } from 'react';
import { Button, Col, Row } from 'reactstrap';
import DeskDepartmentService from 'services/HelpDesk/Department';
import TicketLogService from 'services/HelpDesk/TicketLog';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import { EDIT_RESPONSE, HELP_DESK_TICKETS } from '../navigation';
import TimeLine from 'domain/TimeLine';
import Loading from 'components/Loader';

export default function DepartmentTransfer(props) {
    const [state, setState] = useState({ departments: [], isLoading: true })
    useEffect(() => {
        const fetchData = async () => {
            let departments = []
            await DeskDepartmentService.getDepList().then((res) => {
                departments = res.data.filter(x => x.id != props.departmentId)
            })
            setState({ ...state, departments: departments, isLoading: false })
        }
        fetchData();
    }, [])
    const handleSubmit = async (values, actions) => {
        const data = {
            departmentId: values.departmentId,
            message: values.message,
            id: props.rid
        }
        await TicketLogService.DeptTransfer(data).then(res => {
            notifySaved();
            props.history.push(HELP_DESK_TICKETS);
        }).catch(err => {
            formUtil.displayFormikError(err, actions)
        })
    }
    const messageReceive = (e) => {
        if (e.key === "response") {
            window.removeEventListener("storage", messageReceive)
            window.location.reload()
        }
    }
    return state.isLoading ? <Loading /> :
        <Formik
            initialValues={{
                departmentId: '',
                message: ''
            }}
            validationSchema={
                Yup.object().shape({
                    departmentId: Yup.string().required('Department is required !'),
                    message: Yup.string().required('Reason to transfer is required !')
                })
            }
            onSubmit={(values, actions) => handleSubmit(values, actions)}
        >
            {({ values, errors, setFieldValue, touched, isSubmitting }) => {
                const handleValueChange = async (name, value) => {
                    setFieldValue(name, value);
                }
                return <Form>
                    <Row>
                        <Col md='4'>
                            <RWDropdownList {...{
                                name: 'departmentId', values: state.departments, value: values['departmentId'],
                                label: 'Department',
                                error: errors['departmentId'],
                                touched: touched['departmentId'],
                                valueField: 'id', textField: 'department'
                            }} handlevaluechange={handleValueChange} />
                        </Col>
                    </Row>
                    <Row>
                        <Col md='12'>
                            <TextAreaInput {...{
                                name: 'message', label: 'Message',
                                value: values['message'],
                                error: errors['message'], touched: touched['message'],

                            }} handlevaluechange={handleValueChange} />
                        </Col>
                    </Row>
                    <div className='d-flex'>
                        <Button disabled={isSubmitting} className="mb-2 mr-2 btn-icon btn-success1" key='button' color="success" type="submit" name="save">
                            {isSubmitting ? 'Please wait' : 'Transfer'}
                        </Button>
                    </div>
                    {props.comments.length > 0 && <Row className='ml-0 mt-2'>
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