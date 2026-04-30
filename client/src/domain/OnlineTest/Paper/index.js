import { notifyError, notifySaved } from "components/alert/Toast";
import GoBack from "components/Button/GoBack";
import { Input, Radio, RWDatePicker, RWDropdownList } from "components/dynamicform/Controls";
import SwitchInput from "components/dynamicform/Controls/SwitchInput";
import { FileDownload } from 'domain/Setup/DataImport/file-download';
import { Form, Formik } from "formik";
import queryString from 'query-string';
import React, { useEffect, useState } from "react";
import Loading, { Saving } from 'components/Loader/index'
import { Button, Card, CardBody, Col, Row } from "reactstrap";
import APIService from 'services/apiservice';
import PaperService from 'services/OnlineTest/Paper';
import EmployeeService from 'services/Org/Employee';
import * as crypto from 'utils/Crypto';
import * as Yup from 'yup';
import * as formUtil from 'utils/form';
import * as dateUtil from 'utils/date'
import { PAPER } from "../Navigation";
import Questions from '../Questions/index';
import { LIVE_STATUS, SHOW_RESULT, STATUS, TEST_VALUES } from "./Constants";
import TestTabs from './TestTabs';
import QuestionImport from "./QuestionImport";


export default function PaperDetails(props) {
    const [results, setResults] = useState({})
    const [showQue, setShowQue] = useState(false);
    const [state, setState] = useState({ test: [], entityData: {}, isLoading: true });
    const [loading, setLoading] = useState({ isBusy: false, type: '' });
    const [emp, setEmp] = useState([]);
    const [importQuestion, setImportQuestion] = useState(false);
    const [sheetErr, setSheetErr] = useState({ columns: [], data: [] })


    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;

    useEffect(() => {
        fetchData()
    }, [showQue])

    const fetchData = async () => {
        let entityData = {}
        let paperId = rid ? rid : results.id
        if (paperId) {
            await PaperService.GetById(paperId).then((res) => {
                entityData = res.data
            }).catch((err) => {
                notifyError(err.message)
            })
        }
        await EmployeeService.getEmpSList().then((res) => {
            setEmp(res.data)
        }).catch((err) => {
            notifyError(err.message)
        })

        setState({ ...state, entityData: entityData, isLoading: false })
    }

    const formValues = {
        name: '',
        organiserId: '',
        duration: '',
        startAt: '',
        endAt: '',
        status: false,
        isJumbled: '',
        moveToLive: false,
        file: '',
        showResult: false
    }

    if (!state.isLoading && (rid || results.id)) {
        formValues.name = state.entityData.name;
        formValues.organiserId = state.entityData.organiserId;
        formValues.duration = state.entityData.duration;
        formValues.startAt = state.entityData.startAt;
        formValues.endAt = state.entityData.endAt;
        formValues.moveToLive = state.entityData.moveToLive;
        formValues.isJumbled = state.entityData.isJumbled == 1 ? true : false;
        formValues.status = state.entityData.status;
        formValues.showResult = state.entityData.showResult
    }

    const validationSchema = Yup.object({
        name: Yup.string().required("Test Name is Required"),
        organiserId: Yup.string().required("Organiser is Required"),
        duration: Yup.number().transform((value) => isNaN(value) ? undefined : value)
            .min(15, "Duration should be morethan 15 min").required("Duration is Required"),
        startAt: Yup.date().when(['moveToLive'], {
            is: (val) => (val === false),
            then: Yup.date().notRequired(),
            otherwise: Yup.date().min(new Date(new Date().setDate(new Date().getDate() - 1)),
                'Start Date must be Now or Future Date')
        }).required("Start Date is Required"),
        endAt: Yup.date().min(Yup.ref("startAt"), "End date must equal to start date or more").required("End Date is Required"),
        isJumbled: Yup.string().required("Jumbling is Required"),
        status: Yup.boolean().test('test1', "Please enable the status to move paper to live", function () {
            if (!this.parent.status && this.parent.moveToLive) {
                return false;
            }
            return true;
        })
    })

    const handleSubmit = async (values, actions) => {
        setLoading({ ...loading, isBusy: true, type: 'updateTest' });
        const data = { ...values };
        data['id'] = rid ? rid : results.id
        await PaperService.UpdateAsync(data).then((res) => {
            setResults(res.data)
            notifySaved("Data Saved Successfully");
            (rid || results.id) && props.history.push(PAPER);
        }).catch((err) => {
            actions ? formUtil.displayFormikError(err, actions)
                : formUtil.displayAPIError(err)
        })
        setLoading({ ...loading, isBusy: false, type: '' });
    }

    const handleDuplicate = async () => {
        setLoading({ ...loading, isBusy: true, type: 'duplicate' });
        let paperId = rid || results.id;
        await PaperService.duplicate(paperId).then((res) => {
            notifySaved('Duplicate paper created successfully!')
        }).catch((err) => {
            formUtil.displayAPIError(err)
        })
        setLoading({ ...loading, isBusy: false, type: '' });
    }

    const handleExportQue = async (paperName) => {
        setLoading({ ...loading, isBusy: true, type: 'Export' });
        let paperId = rid || results.id;
        await APIService.getBlobAsync(`OnlineTest/Question/Export/${paperId}`).then(r => {
            FileDownload(r.data, `${paperName}_${`${dateUtil.DisplayDateTime(dateUtil.getToday())}`}.xlsx`, r.headers['content-type']);
        }).catch((err) => {
            formUtil.displayAPIError(err);
        })
        setLoading({ ...loading, isBusy: false, type: '' });
    }

    const handleImportQue = async (values) => {
        setLoading({ ...loading, isBusy: true, type: 'Import' })
        let paperId = rid || results.id
        var formData = new FormData();
        formData.append('formFile', values.file)
        formData.append('paperId', paperId)
        await APIService.postFormDataAsync('DataImport/Question', formData).then((result) => {
            if (result.data.hasNoError && result.data.headers && result.data.headers.lenght > 0) {
                const columns = result.data.headers.map((f) => {
                    return { Header: f.name, accessor: f.propertyName }
                })
                setSheetErr({ ...sheetErr, columns: columns, data: result.data.returnValue })
            }
            if (result.data.returnValue === null && result.status === 200) {
                notifySaved('Data imported Sucessfully')
                setSheetErr({ ...sheetErr, columns: [], data: [] })
            }
            if (result.data.hasError) {
                if (result.data && result.data.messages && result.data.messages.length > 0) {
                    notifyError(result.data.messages[0].description);
                }
                const columns = result.data.headers.map(f => {
                    return { Header: f.name, accessor: f.propertyName }
                })
                setSheetErr({ ...sheetErr, columns: columns, data: result.data.returnValue })
            }
        }).catch((err) => {
            formUtil.displayAPIError(err);
        })
        setLoading({ ...loading, isBusy: false, type: '' })
    }

    return !showQue ? state.isLoading ? <Loading /> :
        <>
            <GoBack title='Paper' link={PAPER} />
            <Card>
                <CardBody>
                    <Formik
                        initialValues={formValues}
                        validationSchema={validationSchema}
                        onSubmit={(values, actions) => handleSubmit(values, actions)}
                    >
                        {({ values, errors, touched, setFieldValue }) => {
                            const handleValueChange = async (name, value) => {
                                setFieldValue(name, value)
                            }
                            return <Form>
                                <Row>
                                    <Col md='6'>
                                        <Input {...{
                                            name: 'name', label: ' Name', value: values['name'],
                                            error: errors['name'], touched: touched['name']
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                    <Col md='6' >
                                        <Input {...{
                                            name: 'duration', type: 'number', label: 'Duration(in Min)',
                                            value: values.duration, error: errors['duration'],
                                            touched: touched['duration'], showDate: true
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                </Row>
                                <Row>
                                    <Col md='6'>
                                        <RWDatePicker {...{
                                            name: 'startAt', label: 'Start Date',
                                            value: values['startAt'],
                                            error: errors['startAt'], touched: touched['startAt'],
                                            showDate: true, format: 'MM/DD/YYYY'
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                    <Col md='6' >
                                        <RWDatePicker {...{
                                            name: 'endAt', label: 'End Date',
                                            value: values['endAt'],
                                            error: errors['endAt'], touched: touched['endAt'],
                                            showDate: true, format: 'MM/DD/YYYY'
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                </Row>
                                <Row>
                                    <Col md='6'>
                                        <RWDropdownList {...{
                                            name: `organiserId`, label: 'Organiser',
                                            value: values['organiserId'],
                                            touched: touched['organiserId'],
                                            error: errors['organiserId'],
                                            valueField: 'id', textField: 'name',
                                            values: emp
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                    <Col md='6' className="mt-2">
                                        <Radio {...{
                                            name: 'isJumbled', label: 'Jumbling Question',
                                            value: values.isJumbled === '' ? undefined : `${values.isJumbled}`,
                                            error: errors['isJumbled'], touched: touched['isJumbled'],
                                            values: TEST_VALUES,
                                        }} handlevaluechange={handleValueChange}
                                        />
                                    </Col>
                                </Row>
                                <Row className="mt-2">
                                    <Col md='6'>
                                        <SwitchInput {...{
                                            name: `status`, label: 'Status',
                                            value: values['status'], touched: errors['status'],
                                            error: errors['status'],
                                            values: STATUS
                                        }}
                                            handlevaluechange={handleValueChange}
                                        />
                                    </Col>
                                    {(rid || results.id) && <>
                                        <Col md='3'>
                                            <SwitchInput {...{
                                                name: `moveToLive`, label: 'Move To Live',
                                                value: values['moveToLive'],
                                                touched: touched['moveToLive'], error: errors['moveToLive'],
                                                values: LIVE_STATUS
                                            }} handlevaluechange={handleValueChange} />
                                        </Col>
                                        <Col md='3'>
                                            <SwitchInput {...{
                                                name: `showResult`, label: 'Show Result',
                                                value: values['showResult'],
                                                touched: touched['showResult'], error: errors['showResult'],
                                                values: SHOW_RESULT
                                            }} handlevaluechange={handleValueChange} />
                                        </Col></>}
                                </Row>
                                <Row>
                                    <Col>
                                        <div className='d-flex pt-4'>
                                            {!results.id && !rid ? <Button className="mb-2 mr-2 btn-icon btn-success" color="success" type="submit">
                                                <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i> Create Test Paper
                                            </Button> : <Row>
                                                <Col>
                                                    <Button color="success" className="m-1" type="submit" disabled={loading.isBusy} >
                                                        {loading.type === 'updateTest' ? 'Please wait...' : 'Update Test'}
                                                    </Button>
                                                    <Button type="button" color="info" className="m-1"
                                                        onClick={() => setShowQue(!showQue)}
                                                        disabled={loading.isBusy}>{loading.type === 'Question' ? 'Please wait...' : 'Add Question'} </Button>
                                                    <Button type="button" className="m-1" color="primary"
                                                        onClick={() => handleDuplicate()} disabled={loading.isBusy}
                                                    >{loading.type === 'duplicate' ? 'Please wait...' : 'Duplicate'}</Button>
                                                    <Button className="m-1" color='success' type="button"
                                                        disabled={loading.isBusy} onClick={() => handleExportQue(values.name)}
                                                    >{loading.type === 'Export' ? 'Please wait...' : 'Export question to excel'}</Button>
                                                    <Button type="button" onClick={() => { setImportQuestion(!importQuestion); setFieldValue('file', '') }}
                                                        className="m-1" color="info" disabled={loading.isBusy}>Import questions</Button>
                                                </Col>
                                            </Row>
                                            }
                                        </div>
                                    </Col>
                                </Row>
                                {importQuestion && <div className="m-2">
                                    <QuestionImport
                                        setFieldValue={setFieldValue}
                                        setImportQuestion={setImportQuestion}
                                        importQuestion={importQuestion}
                                        values={values}
                                        handleImportQue={handleImportQue}
                                        columns={sheetErr.columns}
                                        data={sheetErr.data}
                                        loading={loading}
                                    />
                                </div>}
                                {(results.id || rid) ?
                                    <TestTabs currentId={results.id} rid={rid} /> : ''}
                            </Form>
                        }}
                    </Formik>
                </CardBody>
            </Card>
        </> : <Questions paperId={rid || results.id} setShowQue={setShowQue} />
}