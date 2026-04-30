
import Loading from 'components/Loader';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Row } from 'reactstrap';
import * as formUtil from 'utils/form';
import ApproveOrReject from '../Leaves/ApproveOrReject';
import WorkFromHomeService from 'services/Leave/WorkFromHome';
import { notifySaved } from 'components/alert/Toast';
import { APPROVAL_WFH } from '../navigation';
import { useHistory } from 'react-router-dom';
import * as compare from 'utils/Compare'

//This Is popUp Screen
export default function LMApproveWFH(props) {
    const [state, setState] = useState({ isLoading: true, formValues: {}, Employees: [], weekWFH: [], request: [], appliedWFH: [], approvedWFH: [] })
    const rid = props.rid ? props.rid : null;
    let history = useHistory();
    useEffect(() => {
        let request = {}, weekWFH = [], appliedWFH = [], approvedWFH = []
        const fetchData = async () => {
            if (rid) {
                await WorkFromHomeService.getById(rid).then((result) => {
                    request = result.data
                });
                await WorkFromHomeService.getPastFutureWFH(request.employeeId, request.fromDate, request.toDate).then((res) => {
                    weekWFH = res.data
                    weekWFH.forEach(function (item) {
                        if (compare.isEqual(item.status, '1')) {
                            appliedWFH.push(item)
                        }
                        else if (compare.isEqual(item.status, '2')) {
                            approvedWFH.push(item)
                        }
                    })
                })
                setState({ ...state, isLoading: false, request: request, weekWFH: weekWFH, appliedWFH: appliedWFH, approvedWFH: approvedWFH })
            }
        }
        fetchData()
    }, [])

    const handleSubmit = async (data, actions) => {

        await WorkFromHomeService.approveOrReject(data).then((res) => {
            notifySaved();
            props.toggle();
            props.handleSearch(0);
        }).catch((error) => {
            formUtil.displayFormikError(error, actions)
        });
    }
    const handleReject = async(data)=>{
        await WorkFromHomeService.rejectAfterApprove(data).then((res)=>{
            notifySaved();
            props.toggle();
            props.handleSearch(0);
        }).catch((error) => {
            formUtil.displayErrors(error)
        });
    }
    return (
        <Fragment>
            <Card>
                <CardBody>
                    {state.isLoading ? <Loading /> :
                        <ApproveOrReject data={state.request}
                            //applied={null} 
                            request={state.request}
                            applied={state.appliedWFH}
                            approved={state.approvedWFH}
                            app={state.weekWFH}
                            handleSubmit={handleSubmit}
                            handleReject={handleReject}
                            name ="WFH"
                        />
                    }
                </CardBody>
            </Card>
        </Fragment>
    )
}
