import React, { Fragment, useState } from 'react'
import { Button, Card, CardBody, Col, Label, Modal, ModalBody, Row } from 'reactstrap'
import { REMOVE_LAST_CHAR } from '../Questions/Constant'
import ResultService from 'services/OnlineTest/Result'
import { notifySaved } from 'components/alert/Toast'
import * as formUtil from 'utils/form'

export default function Review(props) {
    let { data } = props
    const [modal, setModal] = useState(false);
    const [state, setState] = useState({ empId: props.empId, paperId: props.paperId })
    const [loading, setLoading] = useState(false);

    const handleReTake = async () => {
        setLoading(true);
        await ResultService.AllowReTake(state.empId, state.paperId).then((res) => {
            notifySaved();
        }).catch((err) => {
            formUtil.displayAPIError(err)
        })
        setModal(!modal);
        setLoading(false);
    }

    const toggle = () => !loading && setModal(!modal);

    return <Fragment>
        <Row>
            <Col md='11'>
                <Label className='text-info m-3' style={{ fontSize: '20px' }}>Review</Label>
            </Col>
            {!props.fromSelfService && <Col>
                <Button className='mt-2' color='primary' onClick={() => setModal(!modal)}>Action</Button>
            </Col>
            }
        </Row>
        <Card>
            <CardBody >
                {data.length > 0 && data.map((i, j) => {
                    return <div className='ml-2' key={j}>
                        <Row>
                            <Col md='0' className='mt-3 ml-3'>
                                <b>Q{j + 1}.</b>
                            </Col>
                            <Col className='mt-3 pl-1'>
                                <b><div dangerouslySetInnerHTML={{ __html: i.text }} ></div></b>
                            </Col >
                        </Row>
                        <Row>
                            <Col>
                                <p><b>Answer : </b>
                                    {i.userAnswer === null ? " - - " : i.type === 2 ?
                                        REMOVE_LAST_CHAR(i.userAnswer) : i.userAnswer}</p>
                            </Col>
                        </Row>
                        <Row>
                            <Col >
                                <p style={{ color: 'green' }}>
                                    <b>Correct answer  :   </b>
                                    <i>{i.type === 2 ? REMOVE_LAST_CHAR(i.correctAnswer) : i.correctAnswer}</i></p>
                            </Col>
                        </Row>
                    </div>
                })}
            </CardBody>
        </Card>
        <Modal isOpen={modal} toggle={toggle}>
            <ModalBody>
                <Row>
                    <Col>
                        <p style={{ fontSize: '16px' }}>Allow user to re-take the test?</p>
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <Button className='mr-1' color='success' onClick={() => handleReTake()}
                            disabled={loading}>{loading ? 'Please wait...' : 'Yes'}</Button>
                        <Button color='danger' disabled={loading}
                            onClick={() => setModal(!modal)}>{loading ? 'Please wait' : 'No'}</Button>
                    </Col>
                </Row>
            </ModalBody>
        </Modal>
    </Fragment>
}



