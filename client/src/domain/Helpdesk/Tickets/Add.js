import PageHeader from 'Layout/AppMain/PageHeader';
import Loading from 'components/Loader';
import { notifySaved } from 'components/alert/Toast';
import TimeLine from 'domain/TimeLine';
import { Form, Formik } from 'formik';
import queryString from 'query-string';
import React,{ Fragment, useEffect, useState } from 'react';
import { Card, CardBody, Row } from 'reactstrap';
import DepartmentService from 'services/HelpDesk/Department';
import HelpTopicService from 'services/HelpDesk/HelpTopic';
import SubTopicService from 'services/HelpDesk/SubTopic';
import TicketService from 'services/HelpDesk/TicketLog';
import * as crypto from 'utils/Crypto';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import TicketLogService from '../../../services/HelpDesk/TicketLog';
import TicketInfo from '../../SelfService/Ticket/info';
import { HELP_DESK_TICKETS } from '../navigation';
import NewTicket from '../../SelfService/Ticket/New';
import UserResponse from '../../SelfService/Ticket/Response';


export default function Ticket(props) {
    const [state, setState] = useState({
        isLoading: true,
        departments: [], entityData: {},
        topics: [], subtopics: [], comments: []
    })

    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;

    useEffect(() => {
        const fetchData = async () => {
            let departments = [], entityData = {}, topics = [], subtopics = [], response = [];
            await DepartmentService.getDepList().then((res) => {
                departments = res.data;
            });
            if (rid) {
                await TicketService.getById(rid).then((res) => {
                    entityData = res.data;
                });
                await HelpTopicService.HelpTopicsByDept(entityData.departmentId).then((result) => {
                    topics = result.data
                });
                await SubTopicService.SubTopicByHelp(entityData.helpTopicId).then((result) => {
                    subtopics = result.data
                });
                await TicketLogService.getTicketLogResponse(entityData.id).then(res => {
                    response = res.data;
                });
            }
            setState({ ...state, isLoading: false, departments: departments, entityData: entityData, topics: topics, subtopics: subtopics, comments: response })
        }
        fetchData()
    }, [])

    const validationSchema = Yup.object({
        departmentId: Yup.string().required("Department is required"),
        helpTopicId: Yup.string().required('HelpTopic  is required!'),
        subTopicId: Yup.string().required('SubTopic is required !'),
        subject: Yup.string().required('Summary is required !'),
        message: Yup.string().required('Details is required !'),
    });

    const handleSubmit = async (values, actions) => {
        const formData = new FormData();
       
        formData.append('departmentId', values.departmentId)
        formData.append('helpTopicId', values.helpTopicId)
        formData.append('subTopicId', values.subTopicId)
        formData.append('subject', values.subject)
        formData.append('message', values.message)
        formData.append('file', values.file)

        if (rid) {
            formData.append('id', rid)
            await TicketService.PutAsync(formData).then((res) => {
                notifySaved();
                props.history.push(HELP_DESK_TICKETS);
            }).catch((err) => {
                formUtil.displayFormikError(err, actions)
            })
        } else {
            await TicketService.PostAsync(formData).then((res) => {
                notifySaved();
                props.history.push(HELP_DESK_TICKETS);
            }).catch((err) => {
                formUtil.displayFormikError(err, actions)
            })
        }
    }
    let formValues = {
        departmentId: '',
        helpTopicId: '',
        subTopicId: '',
        subject: '',
        message: '',
        file: '',
        response: ''
    }
    if (!state.isLoading && rid) {
        formValues = { ...state.entityData }
    }
    const messageReceive = (e) => {
        if (e.key === "response") {
            window.removeEventListener("storage", messageReceive)
            window.location.reload()
        }
    }
    return (
        <Fragment>
            <PageHeader title="Open a New Ticket" />
            <Card>
                <CardBody>
                    {state.isLoading ? <Loading /> :
                        <Formik
                            initialValues={formValues}
                            validationSchema={validationSchema}
                            onSubmit={(values, actions) => handleSubmit(values, actions)}>
                            {({ values, errors, touched, setFieldValue, isSubmitting, setValues }) => {

                                const handleValueChange = async (name, value) => {
                                    setFieldValue(name, value);
                                    if (name == "departmentId") {
                                        if (value) {
                                            await HelpTopicService.HelpTopicsByDept(value).then((result) => {
                                                setState({ ...state, topics: result.data, subtopics: [] })
                                            })
                                            setFieldValue('helpTopicId', '')
                                            setFieldValue('subTopicId', '')
                                        } else {
                                            setState({ ...state, topics: [], subtopics: [] })
                                            setFieldValue('helpTopicId', '')
                                            setFieldValue('subTopicId', '')
                                        }
                                    }
                                    if (name == "helpTopicId")
                                        if (value) {
                                            await SubTopicService.SubTopicByHelp(value).then((result) => {
                                                setState({ ...state, subtopics: result.data })
                                            })
                                            setFieldValue('subTopicId', '')
                                        } else {
                                            setState({ ...state, subtopics: [] })
                                            setFieldValue('subTopicId', '')
                                        }
                                }
                                return <>
                                    {!state.entityData.status &&
                                        <Form>
                                            <NewTicket
                                                values={values}
                                                errors={errors}
                                                touched={touched}
                                                handleValueChange={handleValueChange}
                                                topics={state.topics}
                                                subTopics={state.subtopics}
                                                departments={state.departments}
                                                rid={rid}
                                                isSubmitting={isSubmitting}
                                                file={state.entityData.file}
                                            />
                                        </Form>
                                    }
                                    <>
                                        <TicketInfo entityData={state.entityData} />
                                        {rid &&
                                            <div>
                                                {!state.entityData.status
                                                    && state.comments.length > 0 &&
                                                    <UserResponse rid={rid}{...props} />
                                                }
                                                {state.comments.length > 0 &&
                                                    <Row className='ml-4'>
                                                        <TimeLine comments={state.comments}
                                                            isEditable={!state.entityData.status}
                                                            messageReceive={messageReceive} />
                                                    </Row>
                                                }
                                            </div>
                                        }

                                    </>
                                </>
                            }}
                        </Formik >
                    }
                </CardBody>
            </Card>
        </Fragment>
    )
}
