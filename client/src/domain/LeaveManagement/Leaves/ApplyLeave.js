import { notifySaved } from 'components/alert/Toast';
import FormSearchButton from 'components/Button/FormSearchButton';
import { CheckBox, Input, RWDatePicker, TextAreaInput } from 'components/dynamicform/Controls';
import Loading from 'components/Loader';
import { FieldArray, Form, Formik } from 'formik';
import PageHeader from 'Layout/AppMain/PageHeader';
import * as _ from 'lodash';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Button, Card, CardBody, Col, Label, Modal, ModalBody, ModalHeader, Row } from 'reactstrap';
import ApplyLeavesService from 'services/Leave/ApplyLeave';
import LeaveTypeService from 'services/Leave/LeaveType';
import * as crypto from 'utils/Crypto';
import * as dateUtil from 'utils/date';
import * as formUtil from 'utils/form';
import * as Yup from 'yup';
import { LM_APPLY_LEAVES_LIST } from '../navigation';
import Details from './ConstValues';
import SelectedLeaveTypes from './SelectedLeaveTypes';
import EmployeeSearch from 'domain/EmployeeSearch';


export default function LMApplyLeaves(props) {
    const [state, setState] = useState({ isLoading: true, formValues: {}, LeaveType: [] })
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    const [lopModal, setLopModal] = useState(false);
    const [book, setBooked] = useState(0);
    const [selEmp, setSelEmp] = useState({})
    useEffect(() => {
        let formValues = {}
        const fetchData = async () => {
            let LeaveType = [];
            await LeaveTypeService.getLeaveType().then((response) => {
                LeaveType = response.data
            });
            if (rid) {
                await ApplyLeavesService.getSelfServiceById(rid).then((res) => {
                    formValues = res.data;
                    formValues.fromHalfDisplay = formValues.fromHalf ? '1' : '';
                    formValues.toHalfDisplay = formValues.toHalf ? '1' : '';
                    formValues.disabled = res.data.status === 1 ? false : true;
                    formValues.selectedLeaveTypes = formValues.applyLeaveType.filter(x => x.isDefaultPayOff == false);
                    formValues.lossOfPay = formValues.applyLeaveType.filter(x => x.isDefaultPayOff == true);
                    setSelEmp(formValues)
                });
            }
            else {
                formValues = {
                    employeeId: '',
                    leaveTypeId: '',
                    emergencyContNo: '',
                    fromDate: '',
                    toDate: '',
                    fromHalfDisplay: '',
                    toHalfDisplay: '',
                    reason: '',
                    adminReason: '',
                    status: 1,
                    disabled: false,
                    applyLeaveType: [],
                    selectedLeaveTypes: [Details()],
                    updateId: '',
                    lopDays: 0,
                    lossOfPay: [],
                    empLeaveTypes: []
                }
            }
            setState({ ...state, isLoading: false, formValues: formValues, LeaveType: LeaveType })
        }
        fetchData()
    }, [])
    const validationSchema = Yup.object({
        emergencyContNo: Yup.string().required("Emergency Contact No is required"),
        fromDate: Yup.string().required("From Date is required"),
        toDate: Yup.string().required("To Date is required"),
        reason: Yup.string().required("Reason for Leave is required"),
        employeeId: Yup.string().required("Employee Name is required"),
    });
    const lopToggle = () => setLopModal(!lopModal)
    const handleLossOfPay = async () => {
        setLopModal(true)
    }
    const handleSubmit = async (values, setFieldError, setFieldValue) => {
        var booked = _.sumBy(values.applyLeaveType, function (o) { return o.noOfLeaves; });
        var daysDifference = dateUtil.getDaysDifference(values.fromDate, values.toDate, values.fromHalfDisplay, values.toHalfDisplay);
        const data = values;
        let selectedLeaveType = [], leaveType = [], lopLeave = {}

        values.applyLeaveType.length > 0 && values.applyLeaveType.map((x, y) => {
            let leave = {}
            if (x.noOfLeaves > 0) {
                leave.noOfLeaves = x.noOfLeaves
                leave.leaveTypeId = x.leaveTypeId
                leave.leaveTypeName = x.leaveTypeName
                leave.availableBalance = x.availableBalance
                if (rid) {
                    leave['applyLeaveId'] = x.applyLeaveId || rid
                }
                leaveType.push({
                    name: x.leaveTypeName,
                    days: x.noOfLeaves,
                })
                selectedLeaveType.push(leave)
            }
        })
        //pushing Unpaid Leave(LOP) in ApplyLeave list
        if (values.lossOfPay.length == 1 && values.lossOfPay[0].noOfLeaves > 0) {
            var lOPRecord = values.lossOfPay[0];
            lopLeave.noOfLeaves = values.lossOfPay[0].noOfLeaves > 0 ? (daysDifference - _.sumBy(values.applyLeaveType, function (o) { return o.noOfLeaves; }))
                : (daysDifference - booked)
            //when update,if we make date changes ,
            //Then booked should be zero,But here its not happend i.e noOfLeaves are -ve.
            lopLeave.leaveTypeId = lOPRecord.leaveTypeId
            lopLeave.leaveTypeName = lOPRecord.leaveTypeName
            if (rid) {
                lopLeave['applyLeaveId'] = lOPRecord.applyLeaveId || rid
            }
            leaveType.push({
                name: lOPRecord.leaveTypeName,
                days: daysDifference - (_.sumBy(values.applyLeaveType, function (o) { return o.noOfLeaves; })),
            })
            lopLeave.noOfLeaves > 0 && selectedLeaveType.push(lopLeave)
        }

        if (rid) {
            data['id'] = rid;
        }
        data.fromHalf = data.fromHalfDisplay === '1' ? true : false;
        data.toHalf = data.toHalfDisplay === '1' ? true : false;
        data.applyLeaveType = selectedLeaveType;
        data.leaveTypes = JSON.stringify(leaveType);
        setLopModal(false)
        await ApplyLeavesService.LeavePostAsync(data).then((res) => {
            notifySaved();
            props.history.push(LM_APPLY_LEAVES_LIST);
        }).catch((err) => {

            setFieldValue('applyLeaveType', []);
            setFieldValue('selectedLeaveTypes', [Details()])
            formUtil.setFormikError(err, setFieldError)
        })
    }
    return (
        <Fragment>
            <PageHeader title="Apply Leave" />
            <Card>
                <CardBody>
                    {state.isLoading ? <Loading /> :
                        <Formik
                            initialValues={state.formValues}
                            validationSchema={validationSchema}
                        >
                            {({ values, errors, actions, touched, setFieldValue, isSubmitting, setFieldError, setFieldTouched }) => {

                                var booked = _.sumBy(values.applyLeaveType, function (o) { return o.noOfLeaves; });
                                var daysDifference = dateUtil.getDaysDifference(values.fromDate, values.toDate, values.fromHalfDisplay, values.toHalfDisplay);
                                //It runs, when we click the clear
                                const handleClear = async () => {
                                    if (rid) {
                                        var booked = _.sumBy(values.applyLeaveType, function (o) { return o.noOfLeaves; });
                                        if (values.lossOfPay.length > 0) {
                                            values.lossOfPay[0].noOfLeaves = booked
                                        } else {
                                            await LeaveTypeService.getDefaultPayOffLeaveType().then((res) => {
                                                let lTypeArray = [];
                                                if (res.data != null) {
                                                    lTypeArray.push({
                                                        leaveTypeId: res.data.id,
                                                        leaveTypeName: res.data.name,
                                                        noOfLeaves: booked
                                                    });
                                                    setFieldValue('lossOfPay', lTypeArray)
                                                }
                                            })
                                        }
                                    }
                                    setFieldValue('applyLeaveType', [])
                                    setFieldValue('selectedLeaveTypes', [Details()])
                                }

                                const handleValueChange = async (name, value, { selected }) => {
                                    setFieldValue(name, value);
                                    if (name == 'employeeId') {
                                        await ApplyLeavesService.GetSearchedEmpLeaveTypes(value).then((response) => {
                                            setFieldValue('empLeaveTypes', response.data)
                                        })
                                    }
                                    var booked = _.sumBy(values.applyLeaveType, function (o) { return o.noOfLeaves; });
                                    var differ = dateUtil.getDaysDifference(values.fromDate, name == 'toDate' ? value : values.toDate,
                                        name == 'fromHalfDisplay' ? value : values.fromHalfDisplay,
                                        name == 'toHalfDisplay' ? value : values.toHalfDisplay);


                                    //if we change the selected dates and halfDay,then clearing the selected leaveTypes
                                    if (name == 'fromDate' || name == 'toDate' || name == 'fromHalfDisplay' || name == 'toHalfDisplay') {

                                        values.applyLeaveType = []
                                        setFieldValue('selectedLeaveTypes', [Details()])
                                        
                                        var bkd = rid == null ? _.sumBy(values.applyLeaveType, function (o) { return o.noOfLeaves; }) : 0;
                                        await LeaveTypeService.getDefaultPayOffLeaveType().then((res) => {
                                            let lTypeArray = [];
                                            if (res.data != null) {
                                                lTypeArray.push({
                                                    leaveTypeId: res.data.id,
                                                    leaveTypeName: res.data.name,
                                                    availableBalance: differ - bkd,
                                                    noOfLeaves: differ - bkd == 0 ? 0.5 : differ - bkd
                                                    //if we select only half day leve (0.5 leave) without paid leaves , then num of leaves should be 0.5 LOP,
                                                });
                                                setFieldValue('lossOfPay', lTypeArray)
                                                setFieldValue('applyLeaveType', [])
                                            }
                                        })
                                    }
                                }
                                return (
                                    <Form>
                                        <Row>
                                            <Col md='7'>
                                                <Row>
                                                    <Col md='4'>
                                                        <span>Employee Name</span>
                                                    </Col>
                                                    <Col md='8'>
                                                        <EmployeeSearch disabled={false} name={'employeeNo'} selEmp={selEmp} setSelEmp={setSelEmp} handleValueChange={handleValueChange} />
                                                        <p style={{ color: 'red' }}>{errors.employeeId}</p>
                                                    </Col>
                                                </Row>
                                                <Row>
                                                    <Col md='4'>
                                                        <span>Leave Date</span>
                                                    </Col>
                                                    <Col md='4'>
                                                        <RWDatePicker {...{
                                                            name: 'fromDate', label: '', showDate: true, showTime: false,
                                                            value: values['fromDate'], disabled: values.disabled,
                                                            error: errors['fromDate'], touched: touched['fromDate'],

                                                        }} handlevaluechange={handleValueChange} />
                                                    </Col>
                                                    <Col md='4'>
                                                        <RWDatePicker {...{
                                                            name: 'toDate', label: '',
                                                            value: values['toDate'], disabled: values.disabled,
                                                            error: errors['toDate'], touched: touched['toDate'],
                                                            showDate: true, showTime: false,

                                                        }} handlevaluechange={handleValueChange} />
                                                    </Col>
                                                </Row>
                                                <Row>
                                                    <Col md='4'>
                                                    </Col>
                                                    <Col md='4'>
                                                        <CheckBox {...{
                                                            name: 'fromHalfDisplay', label: 'Half day', values: [{ value: 1, text: '' }],
                                                            value: values['fromHalfDisplay'], disabled: values.disabled,
                                                            error: errors['fromHalfDisplay'], touched: touched['fromHalfDisplay'],
                                                        }} handlevaluechange={handleValueChange} />

                                                    </Col>
                                                    <Col md='4'>
                                                        <CheckBox {...{
                                                            name: 'toHalfDisplay', label: 'Half day', values: [{ value: 1, text: '' }],
                                                            value: values['toHalfDisplay'], disabled: values.disabled,
                                                            error: errors['toHalfDisplay'], touched: touched['toHalfDisplay'],
                                                        }} handlevaluechange={handleValueChange} />
                                                    </Col>
                                                </Row>
                                                <Row>
                                                    <Col md='4'>
                                                        <span>Reason For Leave</span>
                                                    </Col>
                                                    <Col md='8'>
                                                        <TextAreaInput {...{
                                                            name: 'reason', label: '',
                                                            required: true, value: values['reason'], disabled: values.disabled,
                                                            error: errors['reason'], touched: touched['reason'],
                                                        }} handlevaluechange={handleValueChange} />
                                                    </Col>
                                                </Row>
                                                <Row>
                                                    <Col md='4'>
                                                        <span>Emergency Contact No</span>
                                                    </Col>
                                                    <Col md='8'>
                                                        <Input {...{
                                                            name: 'emergencyContNo', label: '', type: 'string',
                                                            value: values['emergencyContNo'], disabled: values.disabled,
                                                            error: errors['emergencyContNo'], touched: touched['emergencyContNo'],
                                                        }} handlevaluechange={handleValueChange} />
                                                    </Col>
                                                </Row>
                                                <Row>
                                                    <Col md='4'><span>Leave Type</span></Col>
                                                    <Col md='8' >
                                                        <FieldArray
                                                            name="selectedLeaveTypes"
                                                            component={SelectedLeaveTypes}
                                                        />
                                                    </Col>
                                                </Row>
                                                {values.status == 1 &&
                                                    <Row>
                                                        <Col md='4'></Col>
                                                        <Col md='8' >
                                                            <a href="javascript:void(0);" onClick={handleClear} style={{ float: "right" }}>
                                                                Clear
                                                            </a>
                                                        </Col>
                                                    </Row>
                                                }
                                            </Col>
                                            <Col md='5'>
                                                <Card>
                                                    <CardBody>
                                                        <table className='table table-bordered'>
                                                            <thead >
                                                                <tr>
                                                                    <th>Leave Type</th>
                                                                    {rid == null && <th>Available Balance</th>}
                                                                    <th>Booked Leaves</th>
                                                                    {rid == null && <th>Balance After Booked Leave</th>}
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                {values.applyLeaveType.map((leave, index) => {
                                                                    return (
                                                                        <tr>
                                                                            <td>{leave.leaveTypeName}</td>
                                                                            {rid == null && <td >{leave.availableBalance ? leave.availableBalance : '-'}</td>}
                                                                            <td>{leave.noOfLeaves}</td>
                                                                            {rid == null && <td>{leave.availableBalance && (leave.availableBalance - leave.noOfLeaves)}</td>}
                                                                        </tr>
                                                                    )
                                                                })}
                                                                {rid == null && daysDifference > 0 && (daysDifference - booked != 0) && values.lossOfPay.length > 0 && values.lossOfPay.map((leave, index) => {
                                                                    return (
                                                                        <tr>
                                                                            <td>{leave.leaveTypeName}</td>
                                                                            {rid == null && <td>0</td>}
                                                                            <td>{daysDifference > 0 && daysDifference - booked}</td>
                                                                            {rid == null && <td>0</td>}
                                                                        </tr>
                                                                    )
                                                                })
                                                                }
                                                            </tbody>
                                                        </table>
                                                        <Row>
                                                            <Col md='5' style={{ marginLeft: '5px', }}><strong>Current Booked : {values.fromDate && values.toDate ? dateUtil.getDaysDifference(values.fromDate, values.toDate, values.fromHalfDisplay, values.toHalfDisplay) : 0}</strong></Col>
                                                            <Col md='2'></Col>
                                                            {rid != null && (daysDifference - booked != 0) && <Col md='4' style={{ marginLeft: '5px', }}><strong>Loss of pay : {daysDifference > 0 ? (daysDifference - booked) : 0}</strong></Col>}
                                                        </Row>
                                                    </CardBody>
                                                </Card>
                                                {/* } */}
                                            </Col>
                                        </Row>
                                        {/* disable buttons when leave is cancelled */}
                                        {values.disabled ? '' : <Fragment>
                                            <Row>
                                                <Col md='2.5'>
                                                    <Button style={{ marginTop: '19px' }} color="success" key='button' type="button" name="save"
                                                        disabled={isSubmitting} onClick={handleLossOfPay}>
                                                        {isSubmitting ? "Please Wait..." : (values.id ? "Update" : "Apply")}
                                                    </Button>
                                                </Col>
                                                <Col>

                                                    <Modal isOpen={lopModal} toggle={lopToggle}>
                                                        <ModalHeader>Apply Leave</ModalHeader>
                                                        <ModalBody>
                                                            <Col ms="4">
                                                                {daysDifference > 0 && (daysDifference - booked) > 0 ? <Label><h6>Are you sure want to continue with loss of pay ?</h6></Label> :
                                                                    <Label><h6>Are you sure want Apply Leaves ?</h6></Label>}
                                                            </Col>
                                                            <Row>
                                                                <div style={{ paddingLeft: '30px' }} >
                                                                    <FormSearchButton title='Yes'
                                                                        setModel={setLopModal}
                                                                        handleSubmit={handleSubmit}
                                                                    />
                                                                </div>
                                                                <div style={{ paddingLeft: '10px' }} >
                                                                    <Button color="danger" type="button" onClick={() => setLopModal(!lopModal)} >
                                                                        No
                                                                    </Button>
                                                                </div>
                                                            </Row>
                                                        </ModalBody>
                                                    </Modal>
                                                </Col>
                                            </Row>
                                        </Fragment>}
                                    </Form>
                                )
                            }}
                        </Formik>
                    }
                </CardBody>
            </Card>
        </Fragment>
    )
}
