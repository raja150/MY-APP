import { notifySaved } from 'components/alert/Toast'
import { Input, PhoneNumber, RWDatePicker, RWDropdownList, SwitchInput } from 'components/dynamicform/Controls'
import Loading from 'components/Loader'
import EmployeeSearch from 'domain/EmployeeSearch'
import { Form, Formik } from 'formik'
import queryString from 'query-string'
import React, { useEffect, useState } from 'react'
import { Button, Card, CardBody, Col, Label, Modal, ModalBody, ModalHeader, Row } from 'reactstrap'
import * as crypto from 'utils/Crypto'
import * as Yup from 'yup'
import * as _ from 'lodash'
import { COMPUTER_TYPES } from 'Site_constants'
import EmployeeDeviceService from '../../../services/Org/EmployeeDevice'
import * as formUtil from 'utils/form'
import { EMPLOYEE_DEVICE } from '../Users/Navigation'
import GoBack from 'components/Button/GoBack'

export default function EmployeeDevice(props) {
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    const [state, setState] = useState({ isLoading: false, entityData: {} })
    const [selEmp, setSelEmp] = useState({})

    const [modal, setModal] = useState(false);
    const toggle = () => setModal(!modal);


    useEffect(() => {
        let entityData = {}
        setState({ ...state, isLoading: true })
        const fetchData = async () => {
            if (rid) {
                await EmployeeDeviceService.getById(rid).then((res) => {
                    entityData = res.data
                    setSelEmp(res.data)
                })
            }
            setState({ ...state, isLoading: false, entityData: entityData })
        }
        fetchData()
    }, [])

    const validationSchema = Yup.object().shape({
        employeeId: Yup.string().required('Employee name is required!'),
        computerType: Yup.string().required('Computer type is required!'),
        hostName: Yup.string().required('Host name is required!'),
        installedOn: Yup.string().when(['isActZeroInstalled', 'isK7Installed'], {
            is: (isActZeroInstalled, isK7Installed) => {
                if (isActZeroInstalled || isK7Installed) {
                    return true
                }
                return false
            },
            then: (schema) => schema.required("Installed on is required!")
        }),
        uninstalledOn: Yup.string().nullable().when('isUninstalled', { is: val => (val == true), then: Yup.string().required('Uninstalled on is required!') }),
    })

    let formValues = {};
    if (rid) {
        formValues = state.entityData;
    }
    else {
        formValues = {
            employeeId: '',
            mobileNumber: '',
            computerType: '',
            hostName: '',
            isActZeroInstalled: false,
            isK7Installed: false,
            installedOn: '',
            isUninstalled: false,
            uninstalledOn: '',
            addDuplicate: false,
        }
    }


    const handleSubmit = async (values, actions) => {
        const data = values;
        if (rid) {
            data['id'] = rid
        }
        await EmployeeDeviceService.PostAsync(data).then((res) => {
            notifySaved();
            props.history.push(EMPLOYEE_DEVICE);
        }).catch((error) => {
            if (error.response && error.response.data.messages[0].feedbackType == 3) {
                setModal(!modal);
                return;
            }
            formUtil.displayFormikError(error, actions);
        });

    }
    const handleSubmitDuplicate = async (values) => {
        values.addDuplicate = true;
        const data = values;
        if (rid) {
            data['id'] = rid
        }
        await EmployeeDeviceService.PostAsync(data).then((res) => {
            notifySaved();
            props.history.push(EMPLOYEE_DEVICE);
        }).catch((err) => {
            formUtil.displayAPIError(err)
        });

    }
    return <>
        <Card>
            <CardBody>
                <GoBack title={'Employee Device'} link={EMPLOYEE_DEVICE} />
                {state.isLoading ? <Loading /> :
                    <Formik
                        initialValues={formValues}
                        validationSchema={validationSchema}
                        onSubmit={(values, actions) => handleSubmit(values, actions)} >
                        {({ values, errors, touched, setFieldValue, isSubmitting }) => {

                            const handleValueChange = async (name, value, employee) => {
                                setFieldValue(name, value)
                                if (name == 'employeeId') {
                                    setFieldValue('mobileNumber', employee.mobileNumber)
                                }
                            }
                            if (!values.isActZeroInstalled && !values.isK7Installed) {
                                values.installedOn = '';
                            }
                            if (!values.isUninstalled) {
                                values.uninstalledOn = '';
                            }
                            return <Form>
                                <Row>
                                    <Col md='6'>
                                        <Label htmlFor='employee'>Employee Name</Label>
                                        <EmployeeSearch name={'employeeNo'} selEmp={selEmp} setSelEmp={setSelEmp}
                                            handleValueChange={handleValueChange} />
                                        <p style={{ color: 'red' }}>{errors.employeeId}</p>
                                    </Col>
                                    <Col md='6'>
                                        <PhoneNumber {...{
                                            name: 'mobileNumber', label: 'Mobile Number', disabled: true,
                                            value: values['mobileNumber'],
                                            error: errors['mobileNumber'], touched: touched['mobileNumber'],
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                </Row>
                                <Row>
                                    <Col md='6'>
                                        <RWDropdownList {...{
                                            name: 'computerType', label: 'Computer Type', textField: 'text', valueField: 'value',
                                            value: values['computerType'], values: COMPUTER_TYPES, error: errors['computerType'], touched: touched['computerType']
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                    <Col md='6'>
                                        <Input {...{
                                            name: 'hostName', label: 'Host Name',
                                            value: values['hostName'], error: errors['hostName'], touched: touched['hostName']
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                </Row>
                                <Row>
                                    <Col md='3'>
                                        <SwitchInput {...{
                                            name: 'isActZeroInstalled', label: 'Is Act Zero Installed',
                                            value: values['isActZeroInstalled'], touched: touched['isActZeroInstalled'], error: errors['isActZeroInstalled'],
                                            values: [{ value: true, text: 'Enable' }, { value: false, text: "Disable" }]
                                        }}
                                            handlevaluechange={handleValueChange}
                                        />
                                    </Col>
                                    <Col md='3'>
                                        <SwitchInput {...{
                                            name: 'isK7Installed', label: 'Is K7 Installed',
                                            value: values['isK7Installed'], touched: touched['isK7Installed'], error: errors['isK7Installed'],
                                            values: [{ value: true, text: 'Enable' }, { value: false, text: "Disable" }]
                                        }}
                                            handlevaluechange={handleValueChange}
                                        />
                                    </Col>
                                    {rid && <Col md='6'>
                                        <SwitchInput {...{
                                            name: 'isUninstalled', label: 'Is Uninstalled',
                                            value: values['isUninstalled'], touched: touched['isUninstalled'], error: errors['isUninstalled'],
                                            values: [{ value: true, text: 'Enable' }, { value: false, text: "Disable" }]
                                        }}
                                            handlevaluechange={handleValueChange}
                                        />
                                    </Col>}
                                </Row>
                                <Row>
                                    {values.isActZeroInstalled || values.isK7Installed ? <Col md='6'>
                                        <RWDatePicker {...{
                                            name: 'installedOn', label: 'Installed On',
                                            value: values['installedOn'], showDate: true, showTime: false,
                                            error: errors['installedOn'], touched: touched['installedOn']
                                        }} handlevaluechange={handleValueChange} />
                                    </Col> : <Col md='6'></Col>}
                                    {values.isUninstalled && <Col md='6'>
                                        <RWDatePicker {...{
                                            name: 'uninstalledOn', label: 'Uninstalled On',
                                            value: values['uninstalledOn'], showDate: true, showTime: false,
                                            error: errors['uninstalledOn'], touched: touched['uninstalledOn']
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>}
                                </Row>

                                <Row className='mb-4'>
                                    <Col md='6'>
                                        <Button style={{ marginTop: '19px' }} color="success" type="submit"
                                            disabled={isSubmitting}>
                                            {isSubmitting ? "Please Wait..." : (values.id ? "Update" : "Submit")}
                                        </Button>
                                    </Col>

                                    <Modal isOpen={modal} toggle={toggle}>
                                        <ModalHeader>Employee Device</ModalHeader>
                                        <ModalBody>
                                            <Label><h6>Are you sure want to continue with duplicate records ?</h6></Label>
                                            <Row>
                                                <div style={{ paddingLeft: '30px' }} >
                                                    <Button onClick={() => handleSubmitDuplicate(values)}>Yes</Button>
                                                </div>
                                                <div style={{ paddingLeft: '10px' }} >
                                                    <Button color="danger" type="button" onClick={() => setModal(!modal)} >
                                                        No
                                                    </Button>
                                                </div>
                                            </Row>
                                        </ModalBody>
                                    </Modal>
                                </Row>
                            </Form>
                        }}
                    </Formik>
                }
            </CardBody>
        </Card>
    </>
}