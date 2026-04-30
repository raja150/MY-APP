import React, { Fragment, useState, useEffect } from 'react'
import queryString from 'query-string';
import * as crypto from 'utils/Crypto'
import CorrectionService from 'services/OnlineTest/Correction'
import { notifyError, notifySaved } from 'components/alert/Toast'
import { Card, CardBody, Row, Col, Button, Collapse, ModalBody, Modal } from 'reactstrap'
import { Radio } from 'components/dynamicform/Controls'
import Loading, { Saving } from 'components/Loader/index'
import { Formik, Form } from 'formik'
import { CORRECTION } from './Constant';
import { CORRECTION_LIST } from '../Navigation';
import GoBack from 'components/Button/GoBack';
import { faAngleDown } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import * as formUtil from 'utils/form'

export default function Correction(props) {
    const parsed = queryString.parse(props.location.search)
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null
    const testName = parsed.n ? crypto.decrypt(parsed.n) : null
    const [state, setState] = useState({ isLoading: true, questions: [], answers: [] })
    const [accordion, setAccordion] = useState({ index: 0, isOpen: false, tempIndex: null })
    const [loading, setLoading] = useState(false);
    const [modal, setModal] = useState(false);

    useEffect(() => {
        fetch()
    }, [])

    const fetch = async () => {
        let questions = [];
        await CorrectionService.GetQuestions(rid).then((res) => {
            questions = res.data;
        }).catch((err) => {
            notifyError(err)
        })
        setState({ ...state, isLoading: false, questions: questions })
    }

    const handleSubmit = async (index, values, setFieldValue) => {
        let Arr = [];
        values.answer.length > 0 && values.answer.forEach((x, i) => {
            if (x.isCorrect) {
                Arr.push({
                    id: x.id,
                    isCorrect: x.isCorrect === '1',
                    employeeId: x.employeeId
                })
            }
        })
        const data = {
            id: rid, //here rid is a testId
            ManualAns: Arr
        }

        await CorrectionService.update(data).then((res) => {
            notifySaved();
            getAnswers(index, values.id, setFieldValue, true);
        }).catch((err) => {
            formUtil.displayAPIError(err)
        })
    }

    const toggleAccordion = async (tab, questionId, setFieldValue) => {
        let isOpen = true;
        setLoading(true);
        if (accordion.index == tab) {
            if (accordion.isOpen) {
                //to avoid calling api when closing the accordion
                questionId = ''
                setFieldValue(`${tab}.answer`, null);
            }
            isOpen = !accordion.isOpen
        }
        setAccordion({ ...accordion, index: tab, isOpen: isOpen })
        questionId && await getAnswers(tab, questionId, setFieldValue);
        setLoading(false);
    }

    const handleToggle = (values, index, questionId, setFieldValue) => {
        if (values[accordion.index].answer && values[accordion.index].answer.find(x => x.isCorrect)
            && values[accordion.index].answer.find(x => x.isSaved == false)) {
            setModal(!modal); setAccordion({ ...accordion, tempIndex: index });
        }
        else {
            toggleAccordion(index, questionId, setFieldValue);
        }
    }

    const getAnswers = async (index, questionId, setFieldValue) => {
        let answers = [];
        await CorrectionService.GetAnswers(questionId).then((res) => {
            answers = res.data;
        }).catch((err) => {
            formUtil.displayAPIError(err)
        })
        if (answers.length === 0) {
            setAccordion({ ...accordion, isOpen: false });
        }
        setState({ ...state, answers: answers });
        setFieldValue(`${index}.answer`, answers);
    }

    const toggle = () => setModal(!modal);

    return <Fragment>
        {state.isLoading ? <Loading /> :
            <Formik
                initialValues={state.questions}
                enableReinitialize={true}
            >
                {({ values, setFieldValue, isSubmitting }) => {
                    return <Form>
                        <GoBack title={testName} link={CORRECTION_LIST} />
                        <Card>
                            <CardBody>
                                {values.length > 0 ? values.map((x, i) => {
                                    return <div className='ml-2 mt-2'>
                                        <Row className='mr-3'>
                                            <Col md='0'>Q.</Col>
                                            <Col>
                                                <Button className="text-left m-0 p-0 " color=''
                                                    type='button'
                                                    onClick={() => handleToggle(values, i, x.id, setFieldValue)}
                                                    style={{ width: '100%' }}
                                                >
                                                    <Row>
                                                        <Col style={{ width: '98%' }}>
                                                            <b><div dangerouslySetInnerHTML={{ __html: x.question }} /></b>
                                                        </Col>
                                                        <Col md='0'>
                                                            <FontAwesomeIcon className='mt-2' size='lg'
                                                                icon={faAngleDown}
                                                                rotation={(accordion.index == i && accordion.isOpen) ? 90 : 0}
                                                            />
                                                        </Col>
                                                    </Row>
                                                </Button>
                                            </Col>
                                        </Row>
                                        <Collapse isOpen={accordion.index == i && accordion.isOpen} data-parent="#accordion"
                                            id="collapseOne" aria-labelledby="headingOne" style={{ marginLeft: '11px' }}>
                                            <Row>
                                                <Col md='11'>
                                                    <p>Correct Ans: <b>{x.correctAns}</b></p>
                                                </Col>
                                            </Row>
                                            <Row>
                                                <Col>
                                                    <b>User Answer: </b>
                                                </Col>
                                            </Row>
                                            {loading ? <Saving /> : x.answer && x.answer.map((ans, j) => {
                                                return <Row >
                                                    <Col md='9' className='mt-3'>
                                                        <p>{values[i].answer[j].answerTxt}</p>
                                                    </Col>
                                                    <Col md='3'>
                                                        <Radio {...{
                                                            name: `${i}.answer.${j}.isCorrect`,
                                                            values: CORRECTION,
                                                            value: ans.isCorrect
                                                        }} handlevaluechange={(e, v) => {
                                                            setFieldValue(e, v);
                                                            setFieldValue(`${i}.answer.${j}.isSaved`, false);
                                                        }} />
                                                    </Col>
                                                </Row>
                                            })}
                                            <Row style={{ marginLeft: '40%' }}>
                                                <Col>
                                                    <Button className='m-2' size='sm' type='button' color='primary' onClick={() => handleSubmit(i, values[i], setFieldValue)}
                                                        disabled={isSubmitting || x.answer && !x.answer.find(x => x.isCorrect)}>{isSubmitting ? 'Please wait...' : 'Save'}</Button>
                                                    <Button type='button' size='sm' color='danger'
                                                        onClick={() => handleToggle(values, i, x.id, setFieldValue)}
                                                    >Cancel</Button>
                                                </Col>
                                            </Row>
                                        </Collapse>
                                        {i == 0 && <Modal isOpen={modal} toggle={toggle}>
                                            <ModalBody>
                                                <Row>
                                                    <Col>
                                                        <h6>Changes are not saved, Are you sure want to continue?</h6>
                                                    </Col>
                                                </Row>
                                                <Row>
                                                    <Col>
                                                        <Button className='m-2' color='success'
                                                            onClick={() => {
                                                                toggleAccordion(accordion.tempIndex, values[accordion.tempIndex].id, setFieldValue);
                                                                setModal(!modal)
                                                            }}>Yes</Button>
                                                        <Button className='' color='danger' onClick={() => setModal(!modal)}>No</Button>
                                                    </Col>
                                                </Row>
                                            </ModalBody>
                                        </Modal>}
                                    </div>
                                }) : <h5 className='text-center'>No wrong answers found</h5>}
                            </CardBody>
                        </Card>
                    </Form>
                }}
            </Formik>
        }
    </Fragment>
}
