import { notifyError } from 'components/alert/Toast';
import Loading from 'components/Loader';
import InfoDisplay from 'domain/Approvals/InfoDisplay';
import TimeLine from 'domain/TimeLine';
import queryString from 'query-string';
import Tabs, { TabPane } from 'rc-tabs';
import ScrollableInkTabBar from 'rc-tabs/lib/ScrollableInkTabBar';
import TabContent from 'rc-tabs/lib/TabContent';
import React, { useEffect, useState } from 'react';
import { Card, CardBody, Col, Row } from 'reactstrap';
import TicketService from 'services/HelpDesk/Ticket';
import { GENDER_M, PRIORITY } from 'Site_constants';
import * as compare from 'utils/Compare';
import * as crypto from 'utils/Crypto';
import * as dateUtil from 'utils/date';
import * as formUtil from 'utils/form';
import TicketLogService from '../../../services/HelpDesk/TicketLog';
import { TicketTabs } from './ConstValues';
import { PostReply } from './PostReply';
import ReAssignTicket from './Re-assign';
import DepartmentTransfer from './Transfer';


export default function Tickets(props) {
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    const [state, setState] = useState({ details: {}, comments: [], isLoading: true })
    useEffect(() => {
        const fetchData = async () => {
            let details = {}, response = [];
            await TicketService.getById(rid).then(res => {
                details = res.data;
            }).catch(err => {
                notifyError(err.message)
            })
            await TicketLogService.getTicketLogResponse(details.id).then(res => {
                response = res.data;
            });
            setState({ ...state, details: details, comments: response, isLoading: false })
        }
        fetchData();
    }, [])

    const messageReceive = (e) => {
        if (e.key === "response") {
            window.removeEventListener("storage", messageReceive)
            window.location.reload()
        }
    }
    return <>
        {state.isLoading ? <Loading /> :
            <Card>
                <CardBody>
                    <h4>{`Ticket No :${state.details.no}`} </h4>
                    <Row className='mb-4'>
                        <Col md='6'>
                            <InfoDisplay label="Status" info={state.details.ticketSts} />
                            <InfoDisplay label="Priority" info={formUtil.GetTextFromArray(PRIORITY, state.details.priority)} />
                            <InfoDisplay label="Department" info={state.details.department} />
                            <InfoDisplay label="Create Date" info={dateUtil.DisplayDateTime(state.details.raisedOn)} />
                            <InfoDisplay label="Assigned To" info={state.details.assignedTo == null ? '-' : state.details.assignedTo} />
                            <InfoDisplay label="SLA Plan" info={state.details.slaPlan} />
                            <InfoDisplay label="Issue Details" info={state.details.message} />
                        </Col>
                        <Col md='6'>
                            <InfoDisplay label="User" info={formUtil.GetTextFromArray(GENDER_M, state.details.gender) + '. ' + state.details.user} />
                            <InfoDisplay label="Email" info={state.details.email} />
                            <InfoDisplay label="Phone" info={state.details.phone} />
                            <InfoDisplay label="Source" info={'-'} />
                            <InfoDisplay label="Help Topic" info={state.details.helpTopic} />
                            <InfoDisplay label="Due Date" info={dateUtil.DisplayDateTime(state.details.dueDate)} />
                        </Col>
                    </Row>
                    <hr />
                    <Row>
                        <Col md='6'>
                            <InfoDisplay label={<b>Ticket RaisedBy </b>} info={<b>{state.details.user}</b>} />
                        </Col>
                    </Row>
                    {!state.details.status && <Tabs defaultActiveKey="0"
                        renderTabBar={() => <ScrollableInkTabBar />}
                        renderTabContent={() => <TabContent />}
                    >
                        {
                            TicketTabs.length > 0 && TicketTabs.map((tab, key) => {
                                return (
                                    <TabPane tab={tab.text} key={key}>
                                        {
                                            compare.isEqual(key, 0) &&
                                            <PostReply
                                                {...props}
                                                details={state.details}
                                                comments={state.comments} />
                                        }
                                        {
                                            compare.isEqual(key, 1) &&
                                            <DepartmentTransfer {...props}
                                                rid={rid}
                                                comments={state.comments}
                                                departmentId={state.details.departmentId} />
                                        }
                                        {
                                            compare.isEqual(key, 2) &&
                                            <ReAssignTicket {...props}
                                                rid={rid}
                                                deptId={state.details.departmentId}
                                                ticketNo={state.details.no}
                                                comments={state.comments} />
                                        }
                                    </TabPane>
                                )
                            })
                        }
                    </Tabs>}
                    {rid && state.details.status && state.comments.length > 0 &&
                        <Row className='ml-0'>
                            <TimeLine comments={state.comments} messageReceive={messageReceive} isEditable={false} />
                        </Row>}

                </CardBody>
            </Card >
        }
    </>
}