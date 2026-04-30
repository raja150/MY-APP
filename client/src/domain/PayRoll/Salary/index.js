
import { notifyError, notifySaved } from 'components/alert/Toast'
import { Input, RWDropdownList } from 'components/dynamicform/Controls'
import MoneyFormat from 'components/Formats/MoneyFormat'
import { FieldArray, Form, Formik } from 'formik'
import _ from 'lodash'
import queryString from 'query-string'
import React, { Fragment, useEffect, useRef, useState } from 'react'
import { AsyncTypeahead } from 'react-bootstrap-typeahead'
import { Button, Card, CardBody, CardTitle, Col, Label, Row } from 'reactstrap'
import SalaryService from 'services/PayRoll/Salary'
import * as Yup from 'yup'
import * as crypto from '../../../utils/Crypto'
import { Gender, InitialVals } from './ConstValues'
import DeductionLineItem from './Deduction'
import LineItem from './Earnings'
import SearchService from 'services/Search'

function Salary(props) {
    const [state, setState] = useState({
        isLoading: true, salInfo: [], employeeId: false, deductions: [],
        Employees: [], template: [], initialValues: InitialVals()
    });
    const [selEmp, setSelEmp] = useState({})
    const parsed = queryString.parse(props.location.search);
    const [rid, setRid] = useState(parsed.r ? crypto.decrypt(parsed.r) : null);
    const [isSearching, setIsSearching] = useState(false);
    const ref = useRef();



    useEffect(() => {
        let template = [], components = [], deductions = []
        setState({ ...state, isLoading: true })
        const fetchData = async () => {
            await SalaryService.getTemplates().then((result) => {
                template = result.data
            })
            await SalaryService.getEarningComponents().then((result) => {
                components = [].concat(result.data).sort((a, b) => a.displayOrder - b.displayOrder);
            })

            await SalaryService.getDeductionComponents().then((result) => {
                deductions = result.data
            })
            if (rid) {

                await SalaryService.getSalaryInfo(rid).then((result) => {
                    const salInfo = result.data;
                    const templateComponents = [].concat(result.data.earnings).sort((a, b) => a.displayOrder - b.displayOrder)
                        .map((compo, k) => {
                            return { ...compo, componentName: compo.component, percentOnCompName: compo.percentOnComp };
                        })
                    const values = {
                        id: salInfo.id,
                        employeeId: salInfo.employeeId,
                        employeeNo: salInfo.employeeNo,
                        templateId: salInfo.templateId,
                        annually: salInfo.annually,
                        monthly: salInfo.monthly,
                        ctc: salInfo.ctc,
                        components: components,
                        deductions: deductions,
                        templateComponents: templateComponents,
                        salaryDeductions: result.data.deductions
                    }
                    setSelEmp(salInfo);
                    setState({
                        ...state, salInfo: salInfo, deductions: deductions, employeeId: true, template: template, isLoading: false, initialValues: values
                    })
                })
            } else {
                setState({
                    ...state, template: template, isLoading: false,
                    initialValues: { ...state.initialValues, components: components, deductions: deductions }
                })
            }
        }
        fetchData();

    }, []);

    const validationSchema = Yup.object({
        annually: Yup.string().required(),
        employeeNo: Yup.string().required("Employee Name is required"),
        ctc: Yup.string().oneOf([Yup.ref('annually'), null], "Annual CTC and total should be same!").required(),
        templateComponents: Yup.array().of(
            Yup.object().shape({
                componentId: Yup.string().when('monthly', {
                    is: (val) => val > 0,
                    then: Yup.string().required("Salary Component is required! ")
                }),
            })
        ),
        salaryDeductions: Yup.array().of(
            Yup.object().shape({
                deductionId: Yup.string().when('monthly', {
                    is: (val) => val > 0,
                    then: Yup.string().required("Deduction type is required!")
                })
            })
        )
    })


    const handleSubmit = async (values) => {
        let items = [], deductItems = []

        values.templateComponents.length > 0 && values.templateComponents.map((compo, i) => {
            let c = {
                componentId: compo.componentId,
                type: compo.type,
                percentage: compo.percentage ? compo.percentage : 0,
                percentOn: compo.percentOn,
                monthly: compo.monthly,
                annually: compo.annually,
                fromTemplate: compo.fromTemplate,
                percentOnCompId: compo.percentOnCompId
            };
            if (rid) {
                c['id'] = compo.id;
                c['salaryId'] = compo.salaryId || rid;
                c['IsDeleted'] = compo.isDeleted;
            }
            items.push(c);
        });
        values.salaryDeductions.length > 0 && values.salaryDeductions.map((deduct, i) => {
            let de = {
                deductionId: deduct.deductionId,
                monthly: deduct.monthly
            }
            if (rid) {
                de['id'] = deduct.id;
                de['salaryId'] = deduct.salaryId || rid;
                de['IsDeleted'] = deduct.isDeleted;
            }
            deductItems.push(de)
        })
        const data = {
            employeeId: selEmp.id,
            templateId: values.templateId ? values.templateId : '',
            annually: values.annually,
            monthly: values.monthly,
            ctc: values.ctc,
            earnings: items,
            deductions: deductItems
        }

        try {
            if (rid) {
                data['id'] = rid;
                data['employeeId'] = state.salInfo.employeeId
            }
            await SalaryService.updateAsync(data).then(res => {
                if (res.id != '') {
                    notifySaved();
                    props.history.push(`/m/payRoll/Salary`)

                }
            }).catch(err => {
                notifyError(err.message)
            })
        }
        catch (error) {
            notifyError(error.message);
        }
    }
    return (
        state.isLoading ? '' :
            <Fragment>
                <Card>
                    <CardBody>
                        <CardTitle>
                            Employee Salary Details
                        </CardTitle>
                        <Formik
                            initialValues={{ ...state.initialValues }}
                            validationSchema={validationSchema}
                            onSubmit={(values) => handleSubmit(values)}>
                            {({ values, errors, touched, setFieldValue }) => {
                                values.ctc = _.sumBy(values.templateComponents, (o) => { return (parseInt(o.annually)) })
                                //- _.sumBy(values.salaryDeductions, (o) => { return (parseInt(o.monthly)) })

                                const handleValueChange = async (name, value, i) => {
                                    setFieldValue(name, value);
                                    switch (name) {
                                        case 'templateId':
                                            setFieldValue('templateComponents', []);
                                            setFieldValue('salaryDeductions', [])

                                            if (value) {
                                                await SalaryService.getTemplateComponentsById(value).then((result) => {
                                                    const components = [].concat(result.data).sort((a, b) => a.displayOrder > b.displayOrder ? 1 : -1)
                                                        .map((compo, k) => {
                                                            return {
                                                                ...compo, componentName: compo.component,
                                                                percentOnCompName: compo.percentOnComp, fromTemplate: true
                                                            };
                                                        })
                                                    setFieldValue('templateComponents', components);
                                                })
                                            }
                                            break;
                                        case 'annually':
                                            setFieldValue('monthly', values.monthly / 12);
                                            break;

                                    }
                                }
                                const handleExistEmployee = async (salInfo) => {
                                    setState({ ...state, isLoading: true })

                                    let components = [], template = [], deductions = []
                                    await SalaryService.getTemplates().then((result) => {
                                        template = result.data
                                    })
                                    await SalaryService.getDeductionComponents().then((result) => {
                                        deductions = result.data
                                    })
                                    await SalaryService.getEarningComponents().then((result) => {
                                        components = [].concat(result.data).sort((a, b) => a.displayOrder - b.displayOrder);
                                    })
                                    const templateComponents = [].concat(salInfo.earnings).sort((a, b) => a.displayOrder - b.displayOrder)
                                        .map((compo, k) => {
                                            return { ...compo, componentName: compo.component, percentOnCompName: compo.percentOnComp };
                                        })
                                    const salaryDeductions = [].concat(salInfo.deductions)
                                    const values = {
                                        id: salInfo.id,
                                        employeeId: salInfo.employeeId,
                                        employeeNo: salInfo.employeeNo,
                                        templateId: salInfo.templateId,
                                        annually: salInfo.annually,
                                        monthly: salInfo.monthly,
                                        ctc: salInfo.ctc,
                                        components: components,
                                        deductions: deductions,
                                        templateComponents: templateComponents,
                                        salaryDeductions: salaryDeductions
                                    }

                                    setSelEmp(salInfo);
                                    setState({ ...state, initialValues: values, salInfo: salInfo, components: components, template: template, isLoading: false })

                                }
                                return (
                                    <Form>
                                        <Row >
                                            <Col md='4'>
                                                <Label htmlFor='employee'>Employee Name</Label>
                                                <AsyncTypeahead
                                                    name='employeeNo'
                                                    disabled={state.employeeId ? true : false}
                                                    onChange={async (e) => {
                                                        if (e.length > 0) {
                                                            const selEmployee = e[0];
                                                            setFieldValue('employeeNo', selEmployee.no)
                                                            await SalaryService.getSalary(selEmployee.id).then((result) => {
                                                                if (result.data != null) {
                                                                    setRid(result.data.id);
                                                                    handleExistEmployee(result.data)
                                                                }
                                                            }).catch((err) => {
                                                                    setRid(null);
                                                                    setFieldValue('employeeNo', selEmployee.no)
                                                                    setFieldValue('monthly', '')
                                                                    setFieldValue('templateId', '')
                                                                    setFieldValue('templateComponents', [])
                                                                    setFieldValue('salaryDeductions', [])
                                                                    setSelEmp(selEmployee)
                                                                })
                                                        }
                                                        else {
                                                            setSelEmp({})
                                                            setFieldValue('employeeNo', '')
                                                        }
                                                    }}
                                                    onSearch={async (query) => {
                                                        setIsSearching(true);
                                                        await SearchService.SearchWithEmpName(query).then((res) => {
                                                            setState({ ...state, Employees: res.data })
                                                        })
                                                        setIsSearching(false);
                                                    }}
                                                    renderMenuItemChildren={(option) => {
                                                        return [
                                                            <div>
                                                                <div><strong>Name:{option.name}</strong></div>
                                                                <div>Employee No:{option.employeeNo}</div>
                                                                <div>Designation : {option.designation}</div>
                                                            </div>
                                                        ]
                                                    }}
                                                    isLoading={state.isLoading}
                                                    options={state.Employees ? state.Employees : []}
                                                    labelKey={option => `${option.name}`}
                                                    selected={selEmp && selEmp.id ? [selEmp] : []}
                                                    ref={ref}
                                                    id='id'
                                                    minLength={4}
                                                />
                                                <p style={{ color: 'red' }}>{errors.employeeNo}</p>
                                            </Col>
                                            {!_.isEmpty(selEmp) || rid != null ?
                                                <Col md='4'>
                                                    <Label htmlFor='EmpName'>Employee Name:<strong>{selEmp.name}</strong></Label><br />
                                                    <Label htmlFor='EmpMobNo'>Employee MobNo:<strong>{selEmp.mobileNumber}</strong></Label><br />
                                                    <Label htmlFor='gender'>Gender:<strong>{Gender[selEmp.gender]}</strong></Label>

                                                </Col>
                                                : ''}
                                        </Row>
                                        <Row className='mb-4'>
                                            <Col md='4'>
                                                <RWDropdownList {...{
                                                    name: 'templateId', label: 'Salary Templates', valueField: 'id', textField: 'name',
                                                    value: values['templateId'], values: state.template,
                                                    error: errors['templateId'], touched: touched['templateId']
                                                }} handlevaluechange={handleValueChange}
                                                />
                                            </Col>
                                            <Col md='4'>
                                                <Input {...{
                                                    name: 'monthly', label: 'Monthly', value: values['monthly'], error: errors['monthly'],
                                                    touched: touched['monthly'], type: 'number'
                                                }} handlevaluechange={handleValueChange} />
                                            </Col>
                                        </Row>
                                        <Row className='mb-4'>
                                            <Col md='12'>
                                                <table className="table table-bordered table-hover " id="tab_logic" >
                                                    <thead>
                                                        <tr>
                                                            <th className="text-Left w-25"> Salary Components </th>
                                                            <th className="text-Left w-25"> Calculation Type</th>
                                                            <th className="text-right"> Salary Per Month </th>
                                                            <th className="text-right">Salary Per Annual </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <FieldArray
                                                            name="templateComponents"
                                                            component={LineItem}
                                                        />
                                                        <tr>
                                                            <td colSpan='2'><Label htmlFor='costToCompany'><strong>Cost To Company</strong></Label></td>
                                                            <td className='text-right'><strong><MoneyFormat value={_.sumBy(values.templateComponents, function (o) { return (parseInt(o.monthly)) })} /> </strong></td>
                                                            <td className='text-right'><strong><MoneyFormat value={values.annually} /></strong></td>
                                                        </tr>
                                                        {errors.ctc ?
                                                            <tr>
                                                                <td colSpan='2'>
                                                                    <p style={{ color: 'red' }}>{errors.ctc}</p>  </td>
                                                                <td className='text-right'>
                                                                    Left : <MoneyFormat value={(_.sumBy(values.templateComponents, function (o) { return (parseInt(o.monthly)) }) - values.monthly)} />
                                                                </td>
                                                                <td>

                                                                </td>
                                                                <td>

                                                                </td>
                                                            </tr>
                                                            : ''}
                                                    </tbody>
                                                </table>
                                            </Col>
                                        </Row>
                                        <Row>
                                            <Col md='12'>
                                                <table className="table table-bordered table-hover " id="tab_logic" >
                                                    <thead>
                                                        <tr>
                                                            <th className="text-Left w-25"> Deduction Components </th>
                                                            <th className="text-Left w-25"> Deduction Salary Per Month </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <FieldArray
                                                            name="salaryDeductions"
                                                            component={DeductionLineItem}
                                                        />
                                                    </tbody>
                                                </table>
                                            </Col>
                                        </Row>
                                        <Button color='success'>Submit</Button>
                                    </Form>
                                )
                            }}
                        </Formik>
                    </CardBody>
                </Card>
            </Fragment>
    )
}

export default Salary
