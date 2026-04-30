import React, { Fragment, useEffect, useState } from 'react'
import PageHeader from 'Layout/AppMain/PageHeader'
import { Card, CardBody, Col, Row, Button, Label, FormGroup } from 'reactstrap'
import TestEmployeeService from 'services/OnlineTest/Paper/TestEmployee'
import { notifyError } from 'components/alert/Toast'
import * as dateUtil from 'utils/date'
import * as crypto from 'utils/Crypto'
import queryString from 'query-string';
import Loading from 'components/Loader'
import { EMP_RESULT } from '../navigation'
import TestService from 'services/SelfService/Test'


export default function List(props) {
    const [state, setState] = useState({ tests: [], isLoading: true })

    useEffect(() => {
        window.addEventListener("storage", (e) => {
            if (e.key === "CloseTest") {
                props.history.push(EMP_RESULT)
            }
            if (e.key === "isDataGet") {
                fetch();
            }
        })
        fetch();
    }, [])

    const fetch = async () => {
        let tests = []
        await TestService.GetList().then((res) => {
            tests = res.data
        }).catch((err) => {
            notifyError(err)
        })
        setState({ ...state, tests: tests, isLoading: false })
    }

    const handleRedirect = async (paperId) => {
        const appUrl = window.location.origin;
        const qry = { t: paperId ? crypto.encrypt(paperId) : null }
        window.open(`${appUrl}#/f/test/start?` + queryString.stringify(qry))
    }

    return <Fragment>
        <PageHeader title='Test' />
        {state.isLoading ? <Loading /> : state.tests.length <= 0 ? <h5 className='text-center'>No data found</h5> :
            <Row>
                {state.tests.map((x, i) => {
                    return <Col md='3' className='mt-2'>
                        <Card>
                            <CardBody>
                                <FormGroup row className='m-0 border-bottom'>
                                    <Label className="text-capitalize pl-0" sm={4} >Test </Label>
                                    <p className='mt-2' sm={0}>:</p>
                                    <Label sm={7}>{x.name}</Label>
                                </FormGroup>
                                <FormGroup row className='m-0 border-bottom'>
                                    <Label className="text-capitalize pl-0" sm={4} >Duration </Label>
                                    <p className='mt-2' sm={0}>:</p>
                                    <Label sm={7}>{x.duration} mins</Label>
                                </FormGroup>
                                <FormGroup row className='m-0 border-bottom'>
                                    <Label className="text-capitalize pl-0" sm={4} >Start date </Label>
                                    <p className='mt-2' sm={0}>:</p>
                                    <Label sm={7}>{dateUtil.getDate(x.startAt)}</Label>
                                </FormGroup>
                                <FormGroup row className='m-0 border-bottom'>
                                    <Label className="text-capitalize pl-0" sm={4} >End date </Label>
                                    <p className='mt-2' sm={0}>:</p>
                                    <Label sm={7}>{dateUtil.getDate(x.endAt)}</Label>
                                </FormGroup>
                                {x.isStarted && !x.reTake ? <p className='text-danger text-center mt-2'>Test started or finished</p> :
                                    <div className='mt-2 text-center'>
                                        <Button size='sm' onClick={() => handleRedirect(x.id)} color='primary'
                                            disabled={(!(dateUtil.getDate(x.startAt) <= dateUtil.getDate(dateUtil.getToday())
                                                && dateUtil.getDate(dateUtil.getToday()) <= dateUtil.getDate(x.endAt)))}>Start</Button>
                                    </div>
                                }
                            </CardBody>
                        </Card>
                    </Col>
                })}
            </Row>}
    </Fragment>
}