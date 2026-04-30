import { notifyError, notifySaved } from "components/alert/Toast";
import _ from "lodash";
import Loading, { Saving } from 'components/Loader';
import React, { Fragment, useEffect, useState } from 'react';
import { Col, DropdownMenu, DropdownToggle, Nav, NavItem, NavLink, Row, Table, UncontrolledButtonDropdown } from 'reactstrap';
import PayMonthService from "services/PayRoll/PayMonth";
import SalaryDetails from './SalaryDetails';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { FileDownload } from "domain/Setup/DataImport/file-download";
import * as formUtil from 'utils/form';
import { faDownload } from "@fortawesome/free-solid-svg-icons";
import Annexure from "./Annexure";
import IncentivesPayCutService from '../../../services/PayRoll/IncentivesPayCut'

const PaySlips = () => {
    const [state, setState] = useState({ payMonth: "", payMonthName: "", months: [], paySlips: {}, earnings: [], deductions: [] });
    const [paySlip, setPaySlip] = useState({});
    const [isLoading, setIsLoading] = useState(true);
    const [annexure, setAnnexure] = useState(false);
    const [incentivesAndPayCut, setIncentivesAndPayCut] = useState({});
    const [isPdfLoading, setIsPdfLoading] = useState(false);

    useEffect(() => {
        const fetchData = async () => {
            let months = [];
            await PayMonthService.MyPayMonths().then((res) => {
                months = res.data
            }).catch((err) => {
                notifyError(err.message);
            });
            setIsLoading(false);
            setState({ ...state, months: months, isLoading: false });
        };
        fetchData();
    }, []);
    const handleValueChange = async (name, value, selectedName, selectedMonth, selectedYear) => {
        setAnnexure(false);
        let incentivesAndPayCut = {};
        if (name === "payMonth" && value !== "") {
            let paySlips = {}, earnings = [], deductions = [];
            setIsLoading(true);
            await PayMonthService.MyPaySlip(value).then((res) => {
                paySlips = res.data;
                setPaySlip(res.data);
                paySlips.earnings.length > 0 && paySlips.earnings.map((e) => {
                    if (e.headerType === 1) {
                        earnings.push(e);
                    } else {
                        deductions.push(e);
                    }
                });
            }).catch((err) => {
                notifyError(err.message);
            });
            setIsLoading(false);
            setState({
                ...state, payMonthName: selectedName, paySlip: paySlips, earnings: earnings,
                deductions: deductions, [name]: value
            });
            await IncentivesPayCutService.getIncetivesPayCut(selectedMonth, selectedYear).then((res) => {
                incentivesAndPayCut = res.data;
            }).catch((err) => {
                notifyError(err.message);
            });
            setIncentivesAndPayCut(incentivesAndPayCut);
        } else {
            setState({ ...state, [name]: value, paySlips: {}, earnings: [], deductions: [], payMonthName: "" });
        }
    };
    const downloadPaySlip = async (monthId) => { 
        setIsPdfLoading(true);
        await PayMonthService.DownloadMyPaySlip(monthId).then((r) => {
            FileDownload(r.data, `PaySlip.pdf`, r.headers['content-type']);
            notifySaved("Downloaded successfully")
        }).catch(err => {
            formUtil.displayErrors(err);
        })
        setIsPdfLoading(false);
    }
    return (<Fragment>{isLoading ? <Loading /> : (
        <>
            <Row>
                {state.payMonthName ? <Col md='4'>
                    <b>Salary Statement for the month of: {state.payMonthName}</b>
                </Col> :
                    <div></div>
                }
                <Col>
                    <UncontrolledButtonDropdown>
                        <DropdownToggle caret outline className="mb-2 mr-2" color="light">
                            {state.payMonthName ? state.payMonthName : "Financial Month"}
                        </DropdownToggle>
                        <DropdownMenu className="dropdown-menu-shadow"  >
                            <Nav vertical>
                                {state.months.map((i, j) => {
                                    return <NavItem key={j}>
                                        <NavLink href="#" onClick={() => handleValueChange('payMonth', i.payMonthId, i.name, i.month, i.year)}>
                                            <span>{i.name}</span>
                                        </NavLink>
                                    </NavItem>
                                })}
                            </Nav>
                        </DropdownMenu>
                    </UncontrolledButtonDropdown>
                </Col>
            </Row>
            {
                _.isEmpty(paySlip) ?
                    <div style={{ marginLeft: '200px', fontSize: '15px' }}>Please choose a month to view your payslip.</div>
                    : (
                        isLoading ? <Loading /> : <div>
                            <Row>
                                <Col xs="6">
                                    <SalaryDetails paySlip={paySlip} />
                                </Col>
                            </Row>
                            <Row>
                                <Col xs="6">
                                    {isPdfLoading ? <Saving /> : <p className='ml-3'> To download your payslip, please click here
                                        <FontAwesomeIcon icon={faDownload} size='2x' style={{ marginLeft: '15px' }} onClick={() => downloadPaySlip(paySlip.payMonthId)} />
                                    </p>
                                    }</Col>
                            </Row>
                            <Row>
                                <a href='javascript:void()' className='ml-3' style={{ fontSize: '15px' }} onClick={(e) => setAnnexure(true)}>Annexure</a>
                            </Row>
                            {annexure && <Row className="ml-0">
                                <Annexure incentivesAndPayCut={incentivesAndPayCut} />
                            </Row>}
                        </div>
                    )}
        </>)}
    </Fragment>)
}
export default PaySlips;
