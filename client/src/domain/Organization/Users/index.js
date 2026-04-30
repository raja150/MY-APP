import { notifySaved } from 'components/alert/Toast'
import GoBack from 'components/Button/GoBack'
import { Input, RWDropdownList } from 'components/dynamicform/Controls'
import Loading from 'components/Loader'
import EmployeeSearch from 'domain/EmployeeSearch'
import { Form, Formik } from 'formik'
import queryString from 'query-string'
import React, { useEffect, useRef, useState } from 'react'
import { Button, Card, CardBody, Col, Label, Row } from 'reactstrap'
import RoleService from 'services/Roles'
import UserService from 'services/User'
import * as crypto from 'utils/Crypto'
import { DisplayDateTime } from 'utils/date'
import * as formUtil from 'utils/form'
import * as Yup from 'yup'
import { USERS } from './Navigation'
import * as _ from 'lodash'
import { actions } from 'react-table'


export default function Users(props) {
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    const [state, setState] = useState({ isLoading: false, roles: [], empS: [], entityData: {} })
    const [selEmp, setSelEmp] = useState({})
    const formRef = useRef()
    const [onEdit, setOnEdit] = useState(false)
    const [showHistory, setShowHistory] = useState(false)

    const [history, setHistory] = useState([])

    useEffect(() => {
        let roles = [], entityData = {}
        const fetchData = async () => {
            await RoleService.getAllRoles().then((res) => {
                roles = res.data;
            })
            if (rid) {
                await UserService.getUserById(rid).then((res) => {
                    entityData = res.data

                })

                if (entityData.employeeId) {
                    await UserService.getEmpById(entityData.employeeId).then((res) => {
                        res.data['employeeId'] = res.data.id
                        setSelEmp(res.data)
                    })
                }
                setOnEdit(true)

            }
            setState({ ...state, isLoading: false, roles: roles, entityData: entityData })
        }
        fetchData()
    }, [])

    const validationSchema = Yup.object().shape({
        employeeId: Yup.string().required('Employee Name is required!'),
        roleId: Yup.string().required('Role is required !'),
        // password: Yup.string().required('Password is required !'),
        confirmPwd: Yup.string().when("password", {
            is: val => (val && val.length > 0 ? true : false),
            then: Yup.string().oneOf(
                [Yup.ref("password")],
                "Both password need to be the same!"
            ).required("Re-enter password!")
        })
    })
    let formValues = {
        employeeId: '',
        name: '',
        roleId: '',
        password: '',
        confirmPwd: '',
        employeeNo: '',
        no: ''
    }
    if (!state.isLoading && rid) {
        formValues.name = state.entityData.name;
        formValues.roleId = state.entityData.roleId;
        formValues.employeeId = state.entityData.employeeId;
    }

    const handleHistory = async () => {
        await UserService.getUserAudit(rid).then((res) => {
            setHistory(res.data)
            setShowHistory({ ...showHistory, tHeader: true })
        });
    }


    const handleSubmit = async (values, actions) => {
        const data = {
            employeeId: values.employeeId,
            name: values.name,
            employeeNo: values.employeeNo,
            roleId: values.roleId,
            password: values.password,
        }
        if (rid) {
            data['id'] = rid
        }
        await UserService.UpdateAsync(data).then((res) => {
            notifySaved();
            props.history.push(USERS)
        }).catch((err) => {
            formUtil.displayFormikError(err, actions)
        })
    }


    return <>
        <Card>
            <CardBody>
                <GoBack title={'User'} link={USERS} />
                {state.isLoading ? <Loading /> :
                    <Formik
                        initialValues={formValues}
                        enableReinitialize={true}
                        validationSchema={validationSchema}
                        onSubmit={(values, actions) => handleSubmit(values, actions)} >
                        {({ values, errors, touched, setFieldValue, isSubmitting }) => {
                            const handleValueChange = async (name, value) => {
                                setFieldValue(name, value)
                                //Update user name when an employee changed from employee search component
                                if (name == 'no') {
                                    setFieldValue('name', value)
                                }
                            }
                            const handlePwdReset = async () => { 
                                const data = {
                                    employeeId: values.employeeId,
                                    name: values.name,
                                    employeeNo: values.employeeNo,
                                    roleId: values.roleId
                                }
                                if (rid) {
                                    data['id'] = rid
                                }

                                await UserService.ResetAsync(data).then((res) => {
                                    notifySaved();
                                    props.history.push(USERS)
                                }).catch((err) => {
                                    formUtil.displayErrors(err)
                                })
                            }
                            return <Form>
                                <Row>
                                    <Col md='6'>
                                        <Label htmlFor='employee'>Employee Name</Label>
                                        <EmployeeSearch disabled={onEdit} name={'employeeNo'} selEmp={selEmp} setSelEmp={setSelEmp}
                                            handleValueChange={handleValueChange} />
                                        <p style={{ color: 'red' }}>{errors.employeeNo}</p>
                                    </Col>
                                    <Col md='6'>
                                        <Input {...{
                                            name: 'name', label: 'User Name', disabled: onEdit,
                                            value: values['name'],
                                            error: errors['name'], touched: touched['name'],
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                </Row>
                                <Row>
                                    <Col md='6'>
                                        <Input {...{
                                            name: 'password', label: 'Password', type: 'password',
                                            value: values['password'], error: errors['password'], touched: touched['password']
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                    <Col md='6'>
                                        <Input {...{
                                            name: 'confirmPwd', label: 'Confirm Password', type: 'password',
                                            value: values['confirmPwd'], error: errors['confirmPwd'], touched: touched['confirmPwd']
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                </Row>
                                <Row className='mb-4'>
                                    <Col md='6'>
                                        <RWDropdownList {...{
                                            name: 'roleId', label: 'Role', textField: 'name', valueField: 'id',
                                            value: values['roleId'], values: state.roles, error: errors['roleId'], touched: touched['roleId']
                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                </Row>
                                <Row className='mb-4'>
                                    <Col md='6'>
                                        <Button className='mr-4' disabled={isSubmitting} onSubmit={handleSubmit} style={{ backgroundColor: '#4CAF50' }}>{!isSubmitting ? 'Submit' : 'Please wait'}</Button>
                                        <Button className='mr-4' disabled={isSubmitting} onClick={handlePwdReset} style={{ backgroundColor: '#4CAF50' }}>{!isSubmitting ? 'Password reset' : 'Please wait'}</Button>
                                        {rid ?
                                            <Button onClick={() => handleHistory()} > History </Button>
                                            : ''}
                                    </Col>

                                </Row>
                                {showHistory ?
                                    <table className='table table-bordered'>
                                        <thead >
                                            <th>S.No</th>
                                            <th>Description</th>
                                            <th>Updated At</th>
                                            <th>Updated By</th>
                                        </thead>
                                        <tbody>
                                            {history && history.length > 0 ? history.map((i, j) => {
                                                return <tr key={j}>
                                                    <td className='text-right'>{j + 1}</td>
                                                    <td>{i.description}</td>
                                                    <td>{DisplayDateTime(i.dateOfUpdated)}</td>
                                                    <td>{i.updatedBy}</td>
                                                </tr>
                                            }) : <tr><td colSpan={4}>No history</td></tr>}
                                        </tbody>
                                    </table>
                                    : ''}
                            </Form>
                        }}
                    </Formik>
                }
            </CardBody>
        </Card>
    </>
}