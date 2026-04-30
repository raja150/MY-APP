import { faBusinessTime } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import cx from 'classnames';
import MoneyFormat from 'components/Formats/MoneyFormat';
import moment from 'moment';
import queryString from 'query-string';
import Tabs, { TabPane } from 'rc-tabs';
import ScrollableInkTabBar from 'rc-tabs/lib/ScrollableInkTabBar';
import TabContent from 'rc-tabs/lib/TabContent';
import React, { Fragment, useEffect, useState } from 'react';
import { Card, CardBody, CardTitle, Col, DropdownMenu, DropdownToggle, Label, Nav, NavItem, NavLink, Row, UncontrolledDropdown } from 'reactstrap';
import PayMonthService from 'services/PayRoll/PayMonth';
import PayRunService from 'services/PayRoll/PayRun';
import * as compare from '../../../utils/Compare';
import * as crypto from '../../../utils/Crypto';
import EmployeeSummary from './EmployeeSummary';
import SalaryOnHold from './SalaryOnHold';
import TaxDeductions from './TaxDeductions';
import Loading from 'components/Loader';
import * as formUtil from 'utils/form';
import AppendToExcel from './DownloadSheet'

const PayRollTabs = [
    { id: 1, text: 'Employee Salary' },
    { id: 2, text: 'Taxes & Deductions' },
    { id: 3, text: 'Salary on hold' },
    { id: 4, text: 'Download Sheet' }
]

function Index(props) {

    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    const [reload, setReload] = useState(false);
    const [payRunning, setPayRunning] = useState(false);
    const [state, setState] = useState({ isLoading: true, payroll: {}, employees: [], HoldEmployees: [] })

    useEffect(() => {
        const fetchData = async () => {
            let payroll = {}
            await PayMonthService.getById(rid).then(res => {
                payroll = res.data;
            }).catch(err => {
                formUtil.displayErrors(err);
            })
            setState({ ...state, isLoading: false, payroll: payroll })
        }
        fetchData();
    }, [reload])

    const handlePayRun = async () => {
        setPayRunning(true);
        await PayRunService.PayRunById(rid).then(res => {
            setState({ ...state, isLoading: true, payroll: res.data })
            setReload(!reload)
            setPayRunning(false);
        }).catch(err => {
            formUtil.displayErrors(err);
            setPayRunning(false);
        })
    }

    const handleDelete = async () => {
        await PayRunService.deletePayRun(rid).then(res => {
            props.history.push('/m/Payroll/PayRun')
        }).catch(err => {
            formUtil.displayErrors(err);
        })
    }

    const handlePayRunRelease = async () => {
        await PayRunService.PayRunRelease(rid).then(res => {
            const payroll = res.data;
            setState({ ...state, isLoading: true, payroll: payroll })
            setReload(!reload)
            setPayRunning(false);
        }).catch(err => {
            formUtil.displayErrors(err);
        })
    }

    const handlePayRunHold = async () => {
        await PayRunService.PayRunHold(rid).then(res => {
            const payroll = res.data;
            setState({ ...state, isLoading: true, payroll: payroll })
            setReload(!reload)
            setPayRunning(false);
        }).catch(err => {
            formUtil.displayErrors(err);
            setPayRunning(false);
        })
    }

    const handlePayslipSend = async () => {
        alert(state.payroll.name);
        await PayMonthService.SendPaySlips(rid).then(res => {
            setReload(!reload)
            setPayRunning(false);
        }).catch(err => {
            formUtil.displayErrors(err);
            setPayRunning(false);

        })
    }
    return (
        <Fragment>
            {
                state.isLoading ? <Loading /> :
                    <Fragment>
                        <div className="app-page-title">
                            <div className="page-title-wrapper">
                                <div className="page-title-heading">
                                    <div
                                        className={cx("page-title-icon rounded-circle")}>
                                        <i className="pe-7s-drawer icon-gradient bg-happy-itmeo" />
                                    </div>
                                    <div>
                                        <div> Pay Run for {state.payroll.name}</div>
                                        {/* <div className={cx("page-title-subheading")}>
                                            Payment details.
                                        </div> */}
                                    </div>
                                </div>
                                <div className="page-title-actions">
                                    <UncontrolledDropdown className="d-inline-block">
                                        <DropdownToggle color="info" className="btn-shadow" caret>
                                            <span className="btn-icon-wrapper pr-2 opacity-7">
                                                <FontAwesomeIcon icon={faBusinessTime} />
                                            </span>
                                            Actions
                                        </DropdownToggle>
                                        <DropdownMenu right>
                                            <Nav vertical>
                                                <NavItem>
                                                    {
                                                        state.payroll.status === 3 ? <NavLink href="#" onClick={handlePayRunRelease}>
                                                            <span>Release Payroll</span>
                                                        </NavLink> : ''
                                                    }
                                                </NavItem>
                                                <NavItem>
                                                    {payRunning ?
                                                        <span>Executing, Please wait...</span>
                                                        : (state.payroll.status === 3 ? <NavLink href="#" onClick={handlePayRun}>
                                                            <span>Execute Payroll</span>
                                                        </NavLink> : ''
                                                        )
                                                    }
                                                </NavItem>
                                                <NavItem>
                                                    {state.payroll.status === 2 ? <NavLink href="#" onClick={handlePayRun}>
                                                        <span>Execute Payroll</span>
                                                    </NavLink> : ''
                                                    }
                                                </NavItem>
                                                <NavItem>
                                                    {state.payroll.status === 3 ? <NavLink href="#" onClick={handleDelete}>
                                                        <span>Delete pay sheet</span>
                                                    </NavLink> : ''
                                                    }
                                                </NavItem>
                                                <NavItem>
                                                    {state.payroll.status === 4 ? <NavLink href="#" onClick={handlePayRunHold}>
                                                        <span>Hold Payroll</span>
                                                    </NavLink> : ''

                                                    }
                                                </NavItem>
                                                <NavItem>
                                                    {state.payroll.status === 4 ? <NavLink href="#" onClick={handlePayslipSend}>
                                                        <span>Send Payslips</span>
                                                    </NavLink> : ''

                                                    }
                                                </NavItem>
                                            </Nav>
                                        </DropdownMenu>
                                    </UncontrolledDropdown>
                                </div>
                            </div>
                        </div>
                        <Card>
                            <CardBody>
                                <CardTitle className='mb-4'>
                                    Summary of Pay Run for {state.payroll.name}
                                </CardTitle>

                                <div className='border rounded p-1'>
                                    <Row>
                                        <Col xs='3'>
                                            <Label >Cost</Label>
                                            <p><strong><MoneyFormat value={state.payroll.cost} /></strong></p>
                                        </Col>
                                        <Col xs='3'>
                                            <Label >Period</Label>
                                            <p><strong>{moment(new Date(state.payroll.start)).format('DD/MM/YYYY')} to {moment(new Date(state.payroll.end)).format('DD/MM/YYYY')}</strong></p>
                                        </Col>
                                        <Col xs='3'>
                                            <Label >No of Employees</Label>
                                            <p><strong>{state.payroll.employees}</strong></p>
                                        </Col>
                                        <Col xs='3'>
                                            <Label >Employees Net Pay</Label>
                                            <p><strong>{<MoneyFormat value={state.payroll.net} />}</strong></p>
                                        </Col>
                                    </Row>
                                    <Row>

                                        <Col xs='3'>
                                            <Label >Days</Label>
                                            <p><strong>{state.payroll.days}</strong></p>
                                        </Col>
                                        <Col xs='3'>
                                            <Label >Status</Label>
                                            <p><strong>{state.payroll.statusTxt}</strong></p>
                                        </Col>
                                    </Row>
                                </div>
                                <div>
                                    <Tabs defaultActiveKey="0"
                                        renderTabBar={() => <ScrollableInkTabBar />}
                                        renderTabContent={() => <TabContent />}
                                    >
                                        {
                                            PayRollTabs.length > 0 && PayRollTabs.map((tab, key) => {
                                                return (
                                                    <TabPane tab={tab.text} key={key}>
                                                        {
                                                            compare.isEqual(key, 0) && <EmployeeSummary rid={rid} {...props} payRoll={state.payroll}/>
                                                        }
                                                        {
                                                            compare.isEqual(key, 1) && <TaxDeductions rid={rid} {...props} />
                                                        }
                                                        {
                                                            compare.isEqual(key, 2) && <SalaryOnHold rid={rid} {...props} />
                                                        }
                                                        {
                                                            compare.isEqual(key, 3) && <AppendToExcel rid={rid} {...props} />
                                                        }
                                                    </TabPane>
                                                )
                                            })
                                        }

                                    </Tabs>
                                </div>
                            </CardBody>
                        </Card>
                    </Fragment>
            }
        </Fragment>
    )
}

export default Index
