import React, { Fragment, useState, useEffect } from 'react'
import PageHeader from 'Layout/AppMain/PageHeader'
import CorrectionService from 'services/OnlineTest/Correction'
import Loading from 'components/Loader/index'
import { Card, CardBody, FormGroup, Label, Button, Row, Col } from 'reactstrap'
import * as crypto from 'utils/Crypto'
import queryString from 'query-string';
import { CORRECTION } from '../Navigation'
import * as dateUtil from 'utils/date'


export default function List(props) {
    const [state, setState] = useState({ isLoading: true, entityData: [] });

    useEffect(() => {
        fetch();
    }, [])

    const fetch = async () => {
        let entityData = []
        await CorrectionService.GetList().then((res) => {
            entityData = res.data
        })
        setState({ isLoading: false, entityData: entityData })
    }

    const handleRedirect = (testId, testName) => {
        const qry = {
            r: testId ? crypto.encrypt(testId) : null,
            n: testName ? crypto.encrypt(testName) : null
        }
        props.history.push(CORRECTION + queryString.stringify(qry));
    }

    return <Fragment>
        <PageHeader title='Manual Correction' />
        {state.isLoading ? <Loading /> :
            <Row>
                {state.entityData.length > 0 ? state.entityData.map((x, i) => {
                    return <Col md='3' className='mt-2'>
                        <Card>
                            <CardBody>
                                <FormGroup row className='m-0 border-bottom'>
                                    <Label className="text-capitalize pl-0" sm={4} >Test</Label>
                                    <p className='mt-2' sm={0}>:</p>
                                    <Label sm={7}>{x.test}</Label>
                                </FormGroup>
                                <FormGroup row className='m-0 border-bottom'>
                                    <Label className="text-capitalize pl-0" sm={4} >Attendees </Label>
                                    <p className='mt-2' sm={0}>:</p>
                                    <Label sm={7}>{x.totalAttendees}</Label>
                                </FormGroup>
                                <FormGroup row className='m-0 border-bottom'>
                                    <Label className="text-capitalize pl-0" sm={4} >Test On </Label>
                                    <p className='mt-2' sm={0}>:</p>
                                    <Label sm={7}>{dateUtil.getDate(x.testOn)}</Label>
                                </FormGroup>
                                <div className='mt-2 text-center'>
                                    <Button size='sm' color='primary'
                                        onClick={() => handleRedirect(x.id, x.test)} > Verify</Button>
                                </div>
                            </CardBody>
                        </Card>
                    </Col>
                }) : <Card style={{ width: '100%' }} className='m-2'>
                    <CardBody>
                        <Row>
                            <Col>
                                <h5 className='text-center'>No data found</h5>
                            </Col>
                        </Row>
                    </CardBody>
                </Card>}
            </Row>
        }
    </Fragment >
}



