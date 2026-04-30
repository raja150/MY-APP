
import { notifySaved } from 'components/alert/Toast';
import Loading from 'components/Loader';
import React, { Fragment, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { Card, CardBody } from 'reactstrap';
import ApplyClientVisitsService from 'services/Leave/ApplyClientVisits';
import * as compare from 'utils/Compare';
import * as formUtil from 'utils/form';
import ApproveOrReject from '../Leaves/ApproveOrReject';


//This Is popUp Screen
export default function LMClientVisit(props) {
    const [state, setState] = useState({ isLoading: true, formValues: {}, Employees: [], weekWFH: [], request: [], appliedWFH: [], approvedWFH: [] })
    const rid = props.rid ? props.rid : null;
    let history = useHistory();
    useEffect(() => {
        let request = {}, weekVisits = [], appliedVisits = [], approvedVisits = []
        const fetchData = async () => {
            if (rid) {
                await ApplyClientVisitsService.getById(rid).then((result) => {
                    request = result.data
                });
                await ApplyClientVisitsService.getPastFutureVisits(request.employeeId, request.fromDate, request.toDate).then((res) => {
                    weekVisits = res.data 
                    weekVisits.forEach(function (item) {
                        if (compare.isEqual(item.status, '1')) {
                            appliedVisits.push(item)
                        }
                        else if (compare.isEqual(item.status, '2')) {
                            approvedVisits.push(item)
                        }
                    })
                })
                setState({ ...state, isLoading: false, request: request, weekVisits: weekVisits, appliedVisits: appliedVisits, approvedVisits: approvedVisits })
            }
        }
        fetchData()
    }, [])

    const handleSubmit = async (data, actions) => {
        await ApplyClientVisitsService.approveOrReject(data).then((res) => {
            notifySaved();
            props.toggle();
            props.handleSearch(0);
        }).catch((error) => {
            formUtil.displayFormikError(error, actions)
        });
    }
    const handleReject = async (data) => {
        await ApplyClientVisitsService.approveOrReject(data).then((res) => {
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
                            request={state.request}
                            applied={state.appliedVisits}
                            approved={state.approvedVisits}
                            app={state.weekVisits}
                            handleSubmit={handleSubmit}
                            handleReject={handleReject}
                            name="Client Visit"
                        />
                    }
                </CardBody>
            </Card>
        </Fragment>
    )
}
