import GoBack from 'components/Button/GoBack';
import moment from 'moment';
import React, { Fragment, useEffect, useState } from 'react';
import { Card, CardBody, Col, Row } from 'reactstrap';
// import MyCalendar from '../../Approvals/Leave/Calendar';
import { LEAVE_APPROVAL } from '../../Approvals/navigation';
import * as crypto from 'utils/Crypto';
import queryString from 'query-string';
import Loading from 'components/Loader';
import ApplyLeaveService from 'services/Leave/ApplyLeave'
import * as _ from 'lodash'
import { useHistory } from 'react-router-dom';
import MyCalendar from 'domain/LeaveManagement/Leaves/Calendar';
import ApproveOrReject from 'domain/LeaveManagement/Leaves/ApproveOrReject';
import { notifySaved } from 'components/alert/Toast';
import ApplyLeavesService from 'services/Leave/ApplyLeave';

import * as formUtil from 'utils/form';
function ApprovalDetails(props) {
    const parsed = queryString.parse(props.location.search);
    const date = parsed.r ? crypto.decrypt(parsed.r) : null;
    const rid = parsed.s ? crypto.decrypt(parsed.s) : null;
    const [state, setState] = useState({ isLoading: true, data: [], approveData: {} });
    let history = useHistory()

    useEffect(() => {
        const fetchData = async () => {
            let data = [], approveData = {}
            if (rid != null) {
                await ApplyLeaveService.GetLeavesByMonth(moment(date).format('MM')).then((response) => {
                    data = response.data
                });
                await ApplyLeaveService.getApprovalByIdAsync(rid).then((res) => {
                    approveData = res.data
                })
                setState({ isLoading: false, data: data, approveData: approveData })
            }
        }
        fetchData();
    }, []); 

    const handleSubmit = async (data, actions) => {
        await ApplyLeavesService.approverApproveOrReject(data).then((result) => {
            notifySaved();
            history.push(LEAVE_APPROVAL)
        }).catch((error) => {
            formUtil.displayFormikError(error, actions)
        })
    }
    return (
        <Fragment>
            <Card>
                {state.isLoading ? <Loading /> :
                    <CardBody>
                        <GoBack title={'Leave application'} link={LEAVE_APPROVAL} /> 
                        <ApproveOrReject data={state.approveData}
                            handleSubmit={handleSubmit} />
                        <MyCalendar data={state.data} month={moment(date).format('MMMM')} />
                    </CardBody>
                }
            </Card>
        </Fragment>
    )
}
export default ApprovalDetails