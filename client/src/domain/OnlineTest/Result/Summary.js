import GoBack from 'components/Button/GoBack';
import Loading from 'components/Loader/index';
import InfoDisplay from 'domain/Approvals/InfoDisplay';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Card, CardBody, Col, Row } from 'reactstrap';
import * as crypto from 'utils/Crypto';
import ResultService from '../../../services/OnlineTest/Result';
import Review from './Review';
import { EMP_RESULT, ADMIN_RESULT } from '../Navigation'
import { notifyError } from 'components/alert/Toast'
import { Pie } from 'react-chartjs-2';

export default function Summary(props) {
    const [state, setState] = useState({ pieData: pieDataSet, result: {} })
    const [loading, setLoading] = useState(true)

    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    const fromSelfService = parsed.s ? crypto.decrypt(parsed.s) : null;

    useEffect(() => {
        const fetch = async () => {
            let result = {};
            let data = [];

            await ResultService.GetSummary(rid).then((res) => {
                result = res.data;
                const pieData = state.pieData;
                data.push(result.correct, result.wrong, result.answered, result.unAnswered)
                pieData.labels = ['Correct', 'Wrong', 'Answered', 'UnAnswered'];
                pieData.datasets[0].data = data
                setState({ ...state, pieData: pieData, result: result });
            }).catch((err) => {
                notifyError(err);
            })
            setLoading(false)
        }
        fetch()
    }, [])


    const convertTotime = (milliSeconds) => {
        var min = Math.floor((Math.round(milliSeconds / 1000)) / 60);
        var sec = Math.floor(Math.round(milliSeconds / 1000) % 60)
        return (min + ":" + (sec < 10 ? '0' : '') + sec) + " min"
    }

    return loading ? <Loading /> : state.result && <Fragment>
        <GoBack title={state.result.name + " Summary"} link={fromSelfService ? EMP_RESULT : ADMIN_RESULT} />
        <Card >
            <CardBody>
                <Row>
                    <Col md='6' className='mt-2' >
                        {!fromSelfService && <InfoDisplay label="Employee" info={state.result.empName} />}
                        <p className='mt-2'><InfoDisplay label="Total Questions" info={state.result.totalQuestion} /></p>
                        <p className='mt-1'><InfoDisplay label="Time-Spent (in min)" info={convertTotime(state.result.timeSpent)} /></p>
                        <p className='mt-1'><InfoDisplay label="Total-Time (in min)" info={state.result.totalTime + " min"} /></p>
                        <InfoDisplay label="Total-Percentage" info={state.result.percentage + " %"} />
                    </Col>
                    <Col md='6'>
                        <Pie data={state.pieData} height='3' width='10' />
                    </Col>
                </Row>
            </CardBody>
        </Card>
        <div className='mt-3'>
            <Review
                data={state.result.queAnswerModel}
                empId={state.result.empId}
                paperId={state.result.paperId}
                fromSelfService={fromSelfService}
            />
        </div>
    </Fragment>
}

export const pieDataSet = {
    datasets: [{
        backgroundColor: ['rgba(0, 206, 0,1)', 'rgba(230, 0, 0, 1)', 'rgba(54, 162, 235, 1)', 'rgba(0, 0, 27, 0.5)'],
        borderColor: ['rgba(0, 0, 27, 0.8)', 'rgba(0, 0, 27, 0.8)', 'rgba(0, 0, 27, 0.8)', 'rgba(0, 0, 27, 0.8)'],
        borderWidth: 1,
    },],
};
