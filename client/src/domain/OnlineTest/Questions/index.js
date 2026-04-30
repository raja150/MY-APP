import { faPlus, faTrash } from "@fortawesome/free-solid-svg-icons"
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome"
import { notifyError, notifySaved } from 'components/alert/Toast'
import OTCheckBox from './Controls/CheckBox'
import Input from 'components/dynamicform/Controls/Input'
import RichTextArea from 'components/dynamicform/Controls/RichTextArea'
import RWDropdownList from 'components/dynamicform/Controls/RWDropdownList'
import Loading from 'components/Loader/index'
import { FieldArray, Form, Formik } from 'formik'
import PageHeader from 'Layout/AppMain/PageHeader'
import * as _ from 'lodash'
import queryString from 'query-string'
import React, { Fragment, useEffect, useState } from 'react'
import { Button, Card, CardBody, Col, Row } from 'reactstrap'
import QuestionService from 'services/OnlineTest/Question'
import * as crypto from 'utils/Crypto'
import * as formUtil from 'utils/form'
import * as Yup from 'yup'
import { QUESTION } from "../Navigation"
import { CHOICE, LINE_ITEM, NUMBER_TO_ALPHABET, QUESTIONS_TYPES, REMOVE_LAST_CHAR, SINGLE_CORRECT } from './Constant'
import KeyType from './Keytype'


export default function Questions(props) {
    const [qNo, setQNo] = useState(0)
    const [singleCorrect, setSingleCorrect] = useState(SINGLE_CORRECT);
    const [state, setState] = useState({ entityData: {}, isLoading: true })
    const [keyValid, setkeyValid] = useState(false)
    const [loading, setLoading] = useState(false);

    let rid = null
    if (props.location) {
        const parsed = queryString.parse(props.location.search);
        rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    }


    useEffect(() => {
        let entityData = {};
        const fetchData = async () => {
            if (rid) {
                await QuestionService.GetById(rid).then((res) => {
                    entityData = res.data
                    let choices = []
                    if (entityData.type == 1) {
                        entityData.choices.forEach((i, j) => {
                            choices.push({
                                value: NUMBER_TO_ALPHABET(j),
                                text: NUMBER_TO_ALPHABET(j),
                            })
                        })
                        let correct = entityData.choices.findIndex(x => x.text == entityData.key);
                        entityData.key = NUMBER_TO_ALPHABET(correct)
                        setSingleCorrect(choices)
                    }
                    else if (entityData.type === 2) {
                        let keys = entityData.key.split(',');
                        let key = ''
                        entityData.choices.forEach((op, j) => {
                            let selKey = keys.find(x => x == op.text);
                            if (selKey) {
                                op.key = NUMBER_TO_ALPHABET(j)
                                key = NUMBER_TO_ALPHABET(j) + ','
                            } else {
                                op.key = ''
                            }
                        })
                        entityData.key = key
                    }
                }).catch((err) => {
                    notifyError(err.message)
                })
                setkeyValid(true)
            }
            setState({ ...state, entityData: entityData, isLoading: false })
        }
        fetchData()
    }, [])

    let formValues = {
        questions: [{
            text: '', type: '', key: '',
            choices: [
                { sNo: '', text: '', key: '' },
                { sNo: '', text: '', key: '' },
            ]
        }],
        paperId: ''
    }


    if (!state.isLoading && rid) {
        formValues = {
            questions: [{
                text: state.entityData.text,
                key: state.entityData.key,
                type: state.entityData.type,
                choices: state.entityData.choices,
                paperId: state.entityData.paperId,
                id: state.entityData.id
            }]
        }
    }


    const handleSubmit = async (values, saveNext) => {
        setLoading(true);
        var isSaved = true;
        let choices = [], key = '';
        if (values.questions[qNo].type === 1 || values.questions[qNo].type === 2) {
            values.questions[qNo].choices.forEach((op, j) => {
                choices.push({
                    sNo: j + 1,
                    text: op.text,
                    choice: NUMBER_TO_ALPHABET(j)
                })
                if (op.id) {
                    choices[j].id = op.id
                }
                key = op.key ? key + op.key + ',' : key
            })
        }

        var data = {
            text: values.questions[qNo].text,
            type: values.questions[qNo].type,
            choices: choices,
            key: values.questions[qNo].type == 2 ? REMOVE_LAST_CHAR(key) : values.questions[qNo].key,
            paperId: values.questions[0].paperId ? values.questions[0].paperId : props.paperId
        }
        if (rid) {
            data['id'] = rid;
        }
        await QuestionService.UpdateAsync(data).then((res) => {
            if (!saveNext) {
                notifySaved();
                if (rid) {
                    props.history.push(QUESTION)
                }
                else {
                    props.setShowQue(false)
                }
            }
            //After adding ,getting Id from return values.  
            else if (res.data.returnValue) {
                //for the deletion of Question 
                values.questions[qNo].id = res.data.returnValue.id
            }
        }).catch((err) => {
            isSaved = false;
            formUtil.displayAPIError(err)
        })
        setLoading(false);
        return isSaved;
    }

    const validationSchema = Yup.object().shape({
        questions: Yup.array().of(
            Yup.object().shape({
                text: Yup.string().required('Question is required'),
                type: Yup.string().required('Type is Required'),

                key: Yup.string().when('type', {
                    is: (val) => val !== '2',
                    then: Yup.string().required('Key is required'),
                }),
                choices: Yup.array().when('type', (type) => {
                    if (type === "1" || type === "2") {
                        return Yup.array().of(
                            Yup.object().shape({
                                text: Yup.string().required('Option is Required')
                            })
                        )
                    }
                })
            }),
        )
    })

    const checkDuplicates = async (values, setFieldError) => {
        let question = false, choice = false;
        var groupByQue = _.groupBy(values.questions, 'text');
        var queArr = Object.values(groupByQue);
        question = queArr.filter(x => x.length > 1).length > 0 ? true : false;
        if (question) {
            setFieldError(`questions.${qNo}.text`, 'Question already exists')
        }
        if (values.questions[qNo].type === 1 || values.questions[qNo].type === 2) {
            await values.questions[qNo].choices.forEach((x, i) => {
                values.questions[qNo].choices.forEach((y, j) => {
                    if (x.text === y.text && i != j) {
                        choice = true;
                        setFieldError(`questions.${qNo}.choices.${j}.text`, 'Duplicate choices')
                    }
                })
            })
        }
        return { question: question, choice: choice }
    }

    const validate = async (values, setFieldError, formErrors) => {
        var response = await checkDuplicates(values, setFieldError);
        const type = values.questions[qNo].type;
        const choices = values.questions[qNo].choices;
        let keyValidate = choices.find(x => x.key !== '');
        if (type === 2 && !keyValidate && !formErrors) {
            notifyError('Alteast one key is required')
            return false;
        }
        if (response.choice || response.question) {
            return false;
        }
        return true;
    }


    const handleDelete = async (question) => {
        setLoading(true);
        if (question.id) {
            await QuestionService.deleteQuestion(question.id).then((res) => {
                notifySaved('Deleted Successfully');
                if (rid) {
                    props.history.push(QUESTION)
                }
            }).catch((err) => {
                formUtil.displayAPIError(err)
            })
        }
        setLoading(false);
    }
    return <Fragment>
        <Row>
            <Col md='10'>
                <PageHeader title={'Questions'} />
            </Col>
            <Col>
                <Button type="button" onClick={() => rid ? props.history.goBack() : props.setShowQue(false)}
                    className="btn mb-3 btn-white btn-hover-primary"
                    style={{ marginLeft: '50%' }}
                >
                    Go Back
                </Button>
            </Col>
        </Row>
        {state.isLoading ? <Loading /> :
            <Formik
                initialValues={formValues}
                validationSchema={validationSchema}
                onSubmit={(values, actions) => {
                    if (validate(values, actions.setFieldError)) {
                        handleSubmit(values)
                    }
                }}
            >
                {({ values, errors, validateForm, setFieldValue, setFieldError }) => {
                    const error = errors.questions && errors.questions[qNo] ? errors.questions[qNo] : null;
                    const handleValueChange = (name, value) => {
                        setFieldValue(name, value)
                    }
                    return <Form>
                        <FieldArray name='questions'>
                            {({ push, remove }) => {
                                const handleSaveAdd = async () => {
                                    let formErrors = await validateForm();
                                    let keyValidate = await validate(values, setFieldError, formErrors.questions)
                                    if (!formErrors.questions && keyValidate) {
                                        if (await handleSubmit(values, true)) {
                                            push(LINE_ITEM)
                                            setQNo(qNo + 1)
                                        }
                                    }
                                }
                                return <div>
                                    {values.questions[qNo] &&
                                        <div>
                                            <Card>
                                                <CardBody>
                                                    <Row>
                                                        <Row>
                                                            <Col style={{ fontSize: '15px', marginTop: '30px', marginLeft: '7px' }}>Q.</Col>
                                                        </Row>
                                                        <Col>
                                                            <RichTextArea {...{
                                                                name: `questions.${qNo}.text`,
                                                                value: values.questions[qNo].text,
                                                                height: '50px',
                                                                placeholder: 'Enter Question...',
                                                                touched: error && error.text,
                                                                error: error && error.text
                                                            }} handlevaluechange={handleValueChange} />
                                                        </Col>
                                                    </Row>
                                                    <Row className='ml-1'>
                                                        <Col md='3'>
                                                            <RWDropdownList  {...{
                                                                name: `questions.${qNo}.type`, label: 'Type',
                                                                value: values.questions[qNo].type, values: QUESTIONS_TYPES,
                                                                valueField: 'id', textField: 'text',
                                                                touched: error && error.type, error: error && error.type,
                                                            }} handlevaluechange={(name, value) => {
                                                                setFieldValue(`questions.${qNo}.key`, '')
                                                                //if changing from singleCorrect to Multi or vice-versa setting values to initial
                                                                if (values.questions[qNo].choices) {
                                                                    values.questions[qNo].choices.map((x, i) => {
                                                                        x.text = '';
                                                                        x.key = '';
                                                                        x.id = '';
                                                                    })
                                                                }
                                                                if (value === 2) {
                                                                    setkeyValid(false)
                                                                }

                                                                //pushing options when type is changed from non-choice question to choice question
                                                                if ((value === 1 || value === 2) && values.questions[qNo].choices.length === 0) {
                                                                    setFieldValue(`questions.${qNo}.choices`, CHOICE)
                                                                }

                                                                //if type changed from multiCorrect to singleCorrect and 
                                                                //if user added more than two options then it will set to initial options
                                                                if (value === 1 && values.questions[qNo].choices.length > 2) {
                                                                    var len = values.questions[qNo].choices.length
                                                                    values.questions[qNo].choices.splice(2, len - 2)
                                                                    singleCorrect.splice(2, len - 2)
                                                                }
                                                                handleValueChange(name, value)
                                                            }} />
                                                        </Col>
                                                        <Col>
                                                            {values.questions[qNo].type !== 2 ?
                                                                <KeyType type={values.questions[qNo].type}
                                                                    name={`questions.${qNo}.key`}
                                                                    value={values.questions[qNo].key}
                                                                    touched={error} error={error}
                                                                    singleCorrect={singleCorrect}
                                                                    handleValueChange={handleValueChange}
                                                                /> : ''
                                                            }
                                                        </Col>
                                                    </Row>
                                                    {values.questions[qNo].type === 2 || values.questions[qNo].type === 1 ?
                                                        <FieldArray name={`questions.${qNo}.choices`}>
                                                            {({ push, remove }) => {
                                                                const handleAddOptions = () => {
                                                                    // pushing options
                                                                    push({ sNo: '', text: '', key: '' })
                                                                    //pushing values to the singleCorrect key
                                                                    if (values.questions[qNo].type === 1) {
                                                                        singleCorrect.push({
                                                                            value: NUMBER_TO_ALPHABET(singleCorrect.length),
                                                                            text: NUMBER_TO_ALPHABET(singleCorrect.length)
                                                                        })
                                                                        setSingleCorrect(singleCorrect)
                                                                    }
                                                                }

                                                                const handleRemove = (i) => {
                                                                    //Removing values from singleCorrect key
                                                                    if (values.questions[qNo].type === 1) {
                                                                        if (values.questions[qNo].key === NUMBER_TO_ALPHABET(i)) {
                                                                            values.questions[qNo].key = ''
                                                                        }
                                                                        else if (values.questions[qNo].key > NUMBER_TO_ALPHABET(i)) {
                                                                            //assiging key to their respective options
                                                                            values.questions[qNo].key = String.fromCharCode(values.questions[qNo].key.charCodeAt() - 1)
                                                                        }
                                                                        singleCorrect.splice(singleCorrect.length - 1, 1)
                                                                        setSingleCorrect(singleCorrect)
                                                                    }
                                                                    if (values.questions[qNo].type === 2) {
                                                                        //assiging key to their respective options
                                                                        values.questions[qNo].choices.forEach((l, m) => {
                                                                            if (m > i && l.key) {
                                                                                l.key = NUMBER_TO_ALPHABET(m - 1);
                                                                            }
                                                                        })
                                                                    }
                                                                    remove(i)
                                                                }
                                                                return <div style={{ marginLeft: '22px' }}>
                                                                    {values.questions[qNo].choices && values.questions[qNo].choices.map((x, i) => {
                                                                        const err = error && error.choices && error.choices[i] ? error.choices[i] : null

                                                                        return <Row form>
                                                                            {values.questions[qNo].type === 2 &&
                                                                                <div style={{ marginTop: '15px', marginLeft: '15px' }}>
                                                                                    <Row>
                                                                                        <OTCheckBox {...{
                                                                                            name: `questions.${qNo}.choices.${i}.key`,
                                                                                            values: [{ value: NUMBER_TO_ALPHABET(i) }],
                                                                                            value: x.key ? x.key : '',
                                                                                            touched: !keyValid,
                                                                                            error: !keyValid
                                                                                        }} handlevaluechange={(name, value) => {
                                                                                            if (value) {
                                                                                                setkeyValid(true);
                                                                                            }
                                                                                            else {
                                                                                                let a = values.questions[qNo].choices.filter(x => x.key !== '');
                                                                                                if (a.length <= 1) {
                                                                                                    setkeyValid(false)
                                                                                                }
                                                                                            }
                                                                                            handleValueChange(name, value)
                                                                                        }} />
                                                                                    </Row>
                                                                                </div>
                                                                            }
                                                                            <Col>
                                                                                <Input {...{
                                                                                    name: `questions.${qNo}.choices.${i}.text`,
                                                                                    label: `Option ${NUMBER_TO_ALPHABET(i)}`,
                                                                                    value: values.questions[qNo].choices[i] &&
                                                                                        values.questions[qNo].choices[i].text,
                                                                                    touched: err && err.text,
                                                                                    error: err && err.text,
                                                                                }} handlevaluechange={handleValueChange} />
                                                                            </Col>
                                                                            <Row>
                                                                                <Col style={{ marginTop: '20px' }}>
                                                                                    <Button type="button" className="p-1 border-0 btn-transition $purple-600" outline
                                                                                        disabled={values.questions[qNo].choices.length <= 2}
                                                                                        onClick={() => handleRemove(i)}
                                                                                    >
                                                                                        <FontAwesomeIcon icon={faTrash} style={{ color: 'red' }} />
                                                                                    </Button>
                                                                                </Col>
                                                                            </Row>
                                                                        </Row>
                                                                    })}
                                                                    <Row>
                                                                        {(values.questions[qNo].type === 2 || values.questions[qNo].type === 1) &&
                                                                            <Button onClick={() => handleAddOptions()}
                                                                                disabled={values.questions[qNo].choices.length === 6}
                                                                                className="ml-3 p-1 border-0 btn-transition" outline color="primary">
                                                                                Add Option <FontAwesomeIcon icon={faPlus} ></FontAwesomeIcon>
                                                                            </Button>
                                                                        }
                                                                    </Row>
                                                                </div>
                                                            }}
                                                        </FieldArray> : ''
                                                    }
                                                </CardBody>
                                            </Card>
                                            <Row>
                                                <Col className='ml-5'>
                                                    <Button className='m-2' type='submit' color='success'
                                                        disabled={loading}>{loading ? 'Please wait...' : 'Save'}</Button>
                                                </Col>
                                                <Col>
                                                    <Button className='m-2' color='danger'
                                                        disabled={!rid && values.questions.length === 1 || loading}
                                                        onClick={() => {
                                                            !rid && remove(qNo)
                                                            handleDelete(values.questions[qNo]);
                                                            !rid && setQNo(qNo - 1)
                                                        }}>{loading ? 'Please wait...' : 'Delete'}</Button>
                                                </Col>
                                                {!rid &&
                                                    <Col>
                                                        <Button className='m-2' type='button' color='info'
                                                            disabled={loading}
                                                            onClick={() => handleSaveAdd()}
                                                        >{loading ? 'Please wait...' : 'Save & ADD'}</Button>
                                                    </Col>
                                                }
                                            </Row>
                                        </div>
                                    }
                                </div>
                            }}
                        </FieldArray>
                    </Form>
                }}
            </Formik>
        }

    </Fragment>
}