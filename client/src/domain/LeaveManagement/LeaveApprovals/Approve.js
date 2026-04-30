import { notifySaved } from 'components/alert/Toast';
import { Radio, TextAreaInput } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import InfoDisplay from 'domain/Approvals/InfoDisplay';
import { Form, Formik } from 'formik';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { Button, Col, Row } from 'reactstrap';
import * as dateUtil from 'utils/date';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import * as crypto from 'utils/Crypto';
import { DETAILS, LEAVE_APPROVAL } from '../../Approvals/navigation'
import * as _ from 'lodash'
import ApplyLeavesService from 'services/Leave/ApplyLeave';
import ApproveOrReject from 'domain/LeaveManagement/Leaves/ApproveOrReject';
import * as compare from 'utils/Compare'
//This is Popup Screen
function LeaveApplication(props) {
    const [state, setState] = useState({
        request: {}, isLoading: true, formValues: {},
        appliedLeaves: [], approvedLeaves: [],
        request: [], weekLeaves: []
    });
    const rid = props.rid ? props.rid : null;
    let history = useHistory();

    useEffect(() => {
        let formValues = {}, request = {}, weekLeaves = [], appliedLeaves = [], approvedLeaves = []
        setState({ ...state, isLoading: true });
        const fetchData = async () => {
            if (rid) {
                await ApplyLeavesService.getApprovalByIdAsync(rid).then((result) => {
                    request = result.data
                })
                await ApplyLeavesService.GetWeekLeaves(request.employeeId, request.fromDate, request.toDate).then((res) => {
                    weekLeaves = res.data
                    weekLeaves.forEach(function (item) {
                        if (compare.isEqual(item.status, '1')) {
                            appliedLeaves.push(item)
                        }
                        else if (compare.isEqual(item.status, '2')) {
                            approvedLeaves.push(item)
                        }
                    })
                })
                setState({ ...state, isLoading: false, request: request, weekLeaves: weekLeaves, appliedLeaves: appliedLeaves, approvedLeaves: approvedLeaves})
            }
        }
        fetchData();
    }, [])
    const handleSubmit = async (data, actions) => {
        await ApplyLeavesService.approverApproveOrReject(data).then((result) => {
            notifySaved();
            props.toggle();
            props.handleSearch(0);
        }).catch((error) => {
            formUtil.displayFormikError(error, actions)
        })
    }

    const MyCalendar = (e) => {
        e.preventDefault();
        const qry = {
            r: (dateUtil.getDate(state.request.fromDate) ? crypto.encrypt(dateUtil.getDate(state.request.fromDate)) : ''),
            s: crypto.encrypt(rid)
        };
        history.push(DETAILS + queryString.stringify(qry));
    };

    const handleReject = async(data)=>{
        await ApplyLeavesService.rejectAfterApprove(data).then((res) => {
            notifySaved();
            props.toggle();
            props.handleSearch(0);
        }).catch((error) => {
            formUtil.displayErrors(error)
        });
    }
    return (
        state.isLoading ? <Loading /> :
            <ApproveOrReject data={state.request}
                applied={state.appliedLeaves} approved={state.approvedLeaves}
                app={state.weekLeaves} request = {state.request}
                handleSubmit={handleSubmit} handleMore={MyCalendar} 
                handleReject={handleReject}
                name="Leave"
                />
    )
}

export default LeaveApplication
