import ConfirmAlertCallBack from 'components/alert/ConfirmAlertCallBack'
import { notifyError, notifySaved } from 'components/alert/Toast'
import Loading from 'components/Loader/index'
import { Form, Formik } from 'formik'
import PageHeader from 'Layout/AppMain/PageHeader'
import queryString from 'query-string'
import React, { Fragment, useEffect, useRef, useState } from 'react'
import { Button, Card, CardBody, Col, FormGroup, Label, Modal, ModalBody, Row } from 'reactstrap'
import TestService from 'services/SelfService/Test'
import * as crypto from 'utils/Crypto'
import * as dateUtil from 'utils/date'
import AnswerType from './AnswerType'
import Timer from './Timer'
import * as formUtil from 'utils/form'


export default function Test(props) {
    const [state, setState] = useState({
        questions: [], formValues: [],
        totalTimeSpent: 0, timeLeft: 0, isLoading: true
    })
    const [qNo, setQNo] = useState(0);
    const [timeSpent, setTimeSpent] = useState({
        startTime: 0, timeSpentOnQue: 0, previousTime: 0,
        unSavedTime: 0
    });
    const [isRefreshedOrNotAllowed, setisRefreshedOrNotAllowed] = useState(false);
    const [timeOver, setTimeOver] = useState(false);
    const [loading, setLoading] = useState(false);
    const [finishExam, setFinishExam] = useState(false);

    const parsed = queryString.parse(props.location.search)
    const rid = parsed.t ? crypto.decrypt(parsed.t) : null
    const formRef = useRef();

    useEffect(async () => {
        if (performance.getEntriesByType("navigation")[0].type === 'reload') {
            setisRefreshedOrNotAllowed(true);
        }
        else {
            await TestService.IsFinished(rid).then((res) => {
                if (res.data) {
                    fetchData();
                }
                else {
                    setisRefreshedOrNotAllowed(true);
                }
            }).catch((err) => {
                formUtil.displayAPIError(err);
            })
        }
    }, [])

    useEffect(() => {
        // calling api when time over
        if (timeOver) {
            let values = formRef.current.initialValues;
            let setFieldValue = formRef.current.setFieldValue;
            handlePreviousOrNextOrSave(values, setFieldValue, false, true)
        }
    }, [timeOver])

    const fetchData = async () => {
        let response = {};
        await TestService.Start(rid).then((res) => {
            response = setValues(res.data)
            localStorage.setItem('isDataGet', 'true');
            localStorage.removeItem('isDataGet')
        }).catch((err) => {
            formUtil.displayAPIError(err);
        })
        setState({
            ...state,
            formValues: response.questions,
            timeLeft: response.timeLeft,
            isLoading: false
        })
        setQNo(response.qNo);
        setTimeSpent({ ...timeSpent, startTime: Date.now() });
    }

    //Saving timeSpent on question on refresh
    const setUnSavedValues = async (questions) => {
        let previousTime = questions[qNo].timeSpent
        let unSavedTime = Date.now() - timeSpent.startTime;
        questions[qNo].timeSpent = previousTime + unSavedTime;
        var data = {
            paperId: rid,
            answer: questions,
            isFinish: true
        }
        await TestService.updateAsync(data).then((res) => {
        }).catch((err) => {
            notifyError(err);
        })
    }

    //Setting API values to the state
    const setValues = (questions) => {
        let totalTimeSpent = 0, qNo = 0, timeLeft = 0;
        questions.forEach((x, i) => {
            if (x.timeSpent) {
                questions[i].isVisited = true;
                totalTimeSpent = totalTimeSpent + x.timeSpent;
                qNo = i
            }
        })

        //calculating totalTimeSpent and converting it to seconds
        let durationInMilliSeconds = questions.length > 0 && questions[0].duration * 60000;
        timeLeft = (durationInMilliSeconds - totalTimeSpent) / 1000
        return {
            questions: questions, qNo: qNo,
            totalTimeSpent: Math.round(totalTimeSpent),
            timeLeft: Math.round(timeLeft)
        }
    }

    const handleSave = async (values, finishExam = false) => {
        var data = {
            // testEmpId: rid,
            paperId: rid,
            answer: values
        }
        if (finishExam) {
            setLoading(true);
            setFinishExam(true);
            data.isFinish = true;
            await TestService.updateAsync(data).then((res) => {
                notifySaved('Test saved successfully!');
                setTimeout(function () {
                    localStorage.setItem('CloseTest', 'true');
                    localStorage.removeItem('CloseTest');
                    window.close();
                }, 500);
            }).catch((err) => {
                notifyError(err);
            })
            setLoading(false);
        }
        else {
            TestService.updateAsync(data).then((res) => {
            }).catch((err) => {
                notifyError(err);
            })
        }
    }

    const handlePreviousOrNextOrSave = async (values, setFieldValue, previousQue = false, finishExam = false) => {
        let previousTime = 0, timeSpentOnQue = 0;
        if (values[qNo].timeSpent) {
            previousTime = values[qNo].timeSpent
        }
        timeSpentOnQue = await calculateTimeSpent(setFieldValue, previousTime);
        values[qNo].timeSpent = timeSpentOnQue;
        if (finishExam) {
            localStorage.setItem('finishExam', true);
            handleSave(values, true);
        }
        else if (previousQue) {
            handleSave(values);
            setQNo(qNo - 1)
        }
        else {
            handleSave(values);
            setQNo(qNo + 1)
        }
    }

    const handleJumpQue = async (values, setFieldValue, index) => {
        let previousTime = 0, timeSpentOnQue = 0;
        if (values[qNo].timeSpent) {
            //getting the previously timeSpent on question
            previousTime = values[qNo].timeSpent;
        }
        timeSpentOnQue = await calculateTimeSpent(setFieldValue, previousTime);
        values[qNo].timeSpent = timeSpentOnQue
        handleSave(values);
        setQNo(index);
    }

    const calculateTimeSpent = async (setFieldValue, previousTime) => {
        let timeSpentOnQue = Date.now() - timeSpent.startTime;
        if (previousTime) {
            //Adding previous spent and current time spent
            timeSpentOnQue = previousTime + timeSpentOnQue
        }
        //Setting values to the respective question
        await setFieldValue(`${qNo}.timeSpent`, timeSpentOnQue);
        setTimeSpent({
            ...timeSpent,
            startTime: Date.now(),
            timeSpentOnQue: timeSpentOnQue,
            previousTime: ''
        })
        return timeSpentOnQue;
    }

    //Setting colors to the question numbers
    const getColor = (val) => {
        if (!val.isVisited) {
            return 'secondary'
        }
        else if (val.isVisited && !val.answer) {
            return 'danger'
        }
        else if (val.isVisited && val.answer) {
            return 'success'
        }
    }
    return <Fragment>
        <div className='m-2'>
            <PageHeader title='Test' />
            {isRefreshedOrNotAllowed ? '' : state.isLoading ? <Loading /> : <Formik
                initialValues={state.formValues}
                innerRef={formRef}
            >
                {({ values, setFieldValue, isSubmitting }) => {
                    const handleOnChange = (name, value) => {
                        setFieldValue(name, value)
                    }
                    return values && values.length > 0 && <Form>
                        <p style={{ color: 'red' }}>* Please don't refresh the page, test will be marked as completed</p>
                        <Card>
                            <CardBody>
                                <Row>
                                    <Col md='9'>
                                        <Row>
                                            <Col><strong style={{ fontSize: '18px' }}>Name &nbsp;:&nbsp;{values[0].testName}</strong></Col>
                                        </Row>
                                        <Row className='mt-2'>
                                            <Col><strong style={{ fontSize: '18px' }}>Date &nbsp;&nbsp;&nbsp;&nbsp;:&nbsp;{dateUtil.getDate(values[0].testDate)}</strong></Col>
                                        </Row>
                                    </Col>
                                    <Col md='3'>
                                        <Row noGutters>
                                            <Col> <strong style={{ fontSize: '18px' }}>Total Time </strong></Col>
                                            <Col><strong style={{ fontSize: '18px' }}>:&nbsp;&nbsp;{values[0].duration} mins</strong></Col>
                                        </Row>
                                        <Row className='mt-2' noGutters>
                                            <Col> <strong style={{ fontSize: '18px' }}>Time Left </strong></Col>
                                            <Col ><strong style={{ fontSize: '18px' }}>:&nbsp;&nbsp;<Timer
                                                setTimeOver={setTimeOver}
                                                setUnSavedValues={setUnSavedValues}
                                                questions={values}
                                                timeLeft={state.timeLeft}
                                                finishExam={finishExam}
                                            /></strong></Col>
                                        </Row>
                                    </Col>
                                </Row>
                            </CardBody>
                        </Card>
                        <Row form>
                            <Col md='8'>
                                <Card className='mt-1'>
                                    {values.length > 0 &&
                                        <CardBody>
                                            <Label><h6 className='text-info'>Question</h6></Label>
                                            <Row className='mt-3'>
                                                <Col>
                                                    <div style={{ float: 'left', fontSize: '18px' }}><p className='mr-2'>{qNo + 1}. </p></div>
                                                    <strong style={{ fontSize: '18px' }} dangerouslySetInnerHTML={{ __html: values[qNo].text }}></strong>
                                                    <Row className='mt-2'>
                                                        <Col>
                                                            <AnswerType
                                                                name={`${qNo}.answer`}
                                                                options={values[qNo].choices}
                                                                type={values[qNo].type}
                                                                handleOnChange={handleOnChange}
                                                                value={values[qNo].answer ? values[qNo].answer : ''}
                                                                disabled={timeOver}
                                                            />
                                                        </Col>
                                                    </Row>
                                                </Col>
                                            </Row>
                                        </CardBody>
                                    }
                                </Card>
                            </Col>
                            <Col md='4'>
                                <Card className='mt-1' >
                                    <CardBody>
                                        <Row className='mb-1'>
                                            <Col>
                                                <div className='mr-1 badge badge-success'>&nbsp;</div>- Answered
                                                <div className='ml-2 mr-1 badge badge-danger'>&nbsp;</div>- Un Answered
                                                <div className='ml-2 mr-1 badge badge-secondary'>&nbsp;</div>- Not Seen
                                            </Col>
                                        </Row>
                                        {values.map((x, i) => {
                                            if (i === qNo && !x.isVisited) {
                                                setFieldValue(`${qNo}.isVisited`, true)
                                            }
                                            return <Button className='m-1' color={getColor(x)} style={{ width: '43px' }} disabled={timeOver}
                                                onClick={() => handleJumpQue(values, setFieldValue, i)} >{i + 1}</Button>
                                        })}
                                    </CardBody>
                                </Card>
                            </Col>
                        </Row>
                        <Row>
                            <Col className='m-2'>
                                <Row className='text-center' noGutters>
                                    <Col>
                                        <Button size='lg' color='primary' disabled={qNo === 0 || loading || timeOver}
                                            onClick={() => handlePreviousOrNextOrSave(values, setFieldValue, true)}>Previous</Button>
                                        <Button size='lg' className='m-2' type='button' color='primary'
                                            disabled={(qNo === values.length - 1 || loading || timeOver)}
                                            onClick={() => handlePreviousOrNextOrSave(values, setFieldValue)}
                                        > Next</Button>
                                    </Col>
                                </Row>
                                <Row>
                                    <Col className='mt-2'>
                                        <ConfirmAlertCallBack title='Finish Test' message="Are you sure?"
                                            confirmText="Finish Test" handleSubmit={handlePreviousOrNextOrSave}
                                            previousTime={values[qNo].timeSpent}
                                            disabled={loading || timeOver}
                                        />
                                    </Col>
                                </Row>
                            </Col>
                        </Row>
                    </Form>
                }}
            </Formik>
            }
            <Modal size='sm' isOpen={isRefreshedOrNotAllowed}>
                <ModalBody>
                    <Row>
                        <Col>
                            <h6 className='text-danger text-center'>Test marked as completed.</h6>
                            <div className='text-center'>
                                <Button color='info' size='sm' onClick={() => window.close()}>Ok</Button>
                            </div>
                        </Col>
                    </Row>
                </ModalBody>
            </Modal>
        </div>
    </Fragment >
}