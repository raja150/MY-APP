import { notifyError, notifySaved } from "components/alert/Toast";
import Loading, { Saving } from "components/Loader";
import React, { Fragment, useEffect, useState } from 'react';
import { Card, CardBody, CardTitle, Col, DropdownMenu, DropdownToggle, Nav, NavItem, NavLink, Row, UncontrolledButtonDropdown } from 'reactstrap';
import PayMonthService from "services/PayRoll/PayMonth";
import FinancialYear from 'services/PayRoll/FinancialYear';
import Mixed from './Mixed';
import _ from 'lodash'
import * as NumberUtil from 'utils/number'
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faDownload } from "@fortawesome/free-solid-svg-icons";
import { FileDownload } from "domain/Setup/DataImport/file-download";
import * as formUtil from 'utils/form';

const Annual = (props) => {
    const [state, setState] = useState({
        financialYearId: "", financialYears: [], annualSalary: [],
        financialYearName: "", isLoading: true, chartCategories: [], chartData: [],
        chartCategories: [], chartData: [], hasFormSixteen: false,
        earningHeaders: [], deductionHeaders: [], earnings: [], deductions: [], payMonths: [],
        isLoading: true
    });
    const [loading, setLoading] = useState(false);




    useEffect(() => {
        const fetchData = async () => {
            let financialYears = [];
            await FinancialYear.getList().then((res) => {
                financialYears = res.data;
            }).catch((err) => {
                notifyError(err.message);
            });
            setState({ ...state, financialYears: financialYears, isLoading: false });
        };
        fetchData();
    }, []);

    const handleValueChange = async (name, value, selectedName) => {
        setState({ ...state, isLoading: true })
        if (name == "financialYear" && value) {
            await PayMonthService.getAnnualSalaryInfo(value).then(async (res) => {
                let categories = [], data = [],
                    earningHeaders = [], deductionHeaders = [],
                    earnings = [], deductions = [],
                    payMonths = [], hasFormSixteen = false;
                //Prepare chart data and Annual data
                res.data.map((s) => {
                    payMonths.push(s);
                    categories.push(s.month);
                    data.push(s.salary);
                    s.earnings.map((e) => {
                        if (earningHeaders.findIndex(x => x.id == e.componentId) === -1) {
                            earningHeaders.push({ id: e.componentId, name: e.name, order: e.order });
                        }
                    });
                    s.deductions.map((d) => {
                        if (deductionHeaders.findIndex(x => x.id == d.componentId) === -1) {
                            deductionHeaders.push({ id: d.componentId, name: d.name, order: d.order });
                        }
                    });
                    earnings.push(...s.earnings);
                    deductions.push(...s.deductions);
                })

                try {
                    await PayMonthService.HasFormSixteen(value).then((res) => {
                        hasFormSixteen = true;
                    });
                } catch (err) {
                    hasFormSixteen = false;
                }
                setState({
                    ...state, financialYearId: value, financialYearName: selectedName,
                    chartCategories: categories, chartData: data,
                    earningHeaders: earningHeaders, deductionHeaders: deductionHeaders, earnings: earnings, deductions: deductions, payMonths: payMonths,
                    isLoading: false, hasFormSixteen: hasFormSixteen
                });

            })
        } else {
            setState({ ...state, annualSalary: [] })
        }
    }

    const downloadAnnualEarning = async (financialYearId) => {
        setLoading(true);
        await PayMonthService.DownloadMyFormSixteen(financialYearId).then(async (r) => {
            FileDownload(r.data, `Form 16.zip`, r.headers['content-type']);
             
        }).catch(err => {
            notifyError("Your form 16 is not available");
        })
        setLoading(false);
    }

    return (
        <Fragment>
            {state.isLoading ? <Loading /> : (
                <Card className="mb-3">
                    <Row>
                        {state.financialYearName &&
                            <Col md='4'>
                                <strong><h6>For the financial year{"  :  " + state.financialYearName}</h6></strong>
                            </Col>
                        }
                        <Col md='1'>
                            <UncontrolledButtonDropdown>
                                <DropdownToggle caret outline className="mb-2 mr-2" color="light">
                                    {state.financialYearName ? state.financialYearName : "Financial Year"}
                                </DropdownToggle>
                                <DropdownMenu className="dropdown-menu-shadow">
                                    <Nav vertical>
                                        {state.financialYears.map((i, j) => {
                                            return <NavItem key={j}>
                                                {
                                                    <NavLink href="#" onClick={() => handleValueChange('financialYear', i.id, i.name)}>
                                                        <span>{i.name}</span>
                                                    </NavLink>
                                                }
                                            </NavItem>
                                        })}
                                    </Nav>
                                </DropdownMenu>
                            </UncontrolledButtonDropdown>
                        </Col>
                        {state.financialYearName && state.financialYearId == "9b471a9b-e1cc-4ac6-02ec-08db22bd2e30" && state.hasFormSixteen &&
                            <Col xs="3" className="ml-5">
                                Download your Form-16
                                {loading ? <Saving /> : <FontAwesomeIcon icon={faDownload} size='2x' style={{ marginLeft: '15px' }} onClick={() => downloadAnnualEarning(state.financialYearId)} />}
                            </Col>}
                    </Row>
                    {state.isLoading ? '' : <>
                        {state.financialYearName ?
                            <>
                                <CardBody className="pt-0">
                                    <Mixed {...props} categories={state.chartCategories} data={state.chartData} />
                                </CardBody>
                                <table>
                                    <thead>
                                        <tr className='table-bordered'>
                                            <th className='text-center'>Header</th>
                                            <th className='text-center'>YTD Total</th>
                                            {state.payMonths.map((item, index) => {
                                                return <th key={index} className='text-center'>{item.month} </th>
                                            })}
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {state.earningHeaders.sort((a, b) => a.order - b.order).map((earning, h1) => {
                                            const total = _.sumBy(state.earnings, function (o) {
                                                return o.componentId == earning.id ? o.amount : 0;
                                            });
                                            return <tr className='table-bordered' key={`err${h1}`}>
                                                <td>{earning.name}</td>
                                                <td className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(total)}</td>
                                                {state.payMonths.map((month, index) => {
                                                    const amount = _.sumBy(state.earnings, function (o) {
                                                        return o.componentId == earning.id && o.salaryId == month.id ? o.amount : 0;
                                                    });
                                                    return <td key={index} className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(amount)} </td>
                                                })}
                                            </tr>
                                        })}

                                        <tr className='table-bordered'>
                                            <td>Incentive</td>
                                            <td className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(_.sumBy(state.payMonths, 'incentive'))}</td>
                                            {state.payMonths.map((month, index) => {
                                                return <td key={index} className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(month.incentive)} </td>
                                            })}
                                        </tr>
                                        <tr className='table-bordered'>
                                            <td>Arrears</td>
                                            <td className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(_.sumBy(state.payMonths, 'payCut'))}</td>
                                            {state.payMonths.map((month, index) => {
                                                return <td key={index} className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(month.arrears)} </td>
                                            })}
                                        </tr>
                                        <tr className='table-bordered'><td colSpan={state.payMonths.length + 2}>&nbsp;</td></tr>
                                        <tr className='table-bordered'>
                                            <td>Pay Cut</td>
                                            <td className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(_.sumBy(state.payMonths, 'payCut'))}</td>
                                            {state.payMonths.map((month, index) => {
                                                return <td key={index} className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(month.payCut)} </td>
                                            })}
                                        </tr>
                                        <tr className='table-bordered'>
                                            <td>ESI</td>
                                            <td className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(_.sumBy(state.payMonths, 'esi'))}</td>
                                            {state.payMonths.map((month, index) => {
                                                return <td key={index} className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(month.esi)} </td>
                                            })}
                                        </tr>
                                        <tr className='table-bordered'>
                                            <td>LOP</td>
                                            <td className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(_.sumBy(state.payMonths, 'lop'))}</td>
                                            {state.payMonths.map((month, index) => {
                                                return <td key={index} className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(month.lop)} </td>
                                            })}
                                        </tr>
                                        <tr className='table-bordered'>
                                            <td>Provisional Tax</td>
                                            <td className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(_.sumBy(state.payMonths, 'pTax'))}</td>
                                            {state.payMonths.map((month, index) => {
                                                return <td key={index} className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(month.pTax)} </td>
                                            })}
                                        </tr>
                                        <tr className='table-bordered'>
                                            <td>PF</td>
                                            <td className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(_.sumBy(state.payMonths, 'epf'))}</td>
                                            {state.payMonths.map((month, index) => {
                                                return <td key={index} className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(month.epf)} </td>
                                            })}
                                        </tr>
                                        <tr className='table-bordered'>
                                            <td>IncomeTax</td>
                                            <td className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(_.sumBy(state.payMonths, 'tax'))}</td>
                                            {state.payMonths.map((month, index) => {
                                                return <td key={index} className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(month.tax)} </td>
                                            })}
                                        </tr>
                                        <tr className='table-bordered'>
                                            <td>Late Coming Deduction</td>
                                            <td className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(_.sumBy(state.payMonths, 'lc'))}</td>
                                            {state.payMonths.map((month, index) => {
                                                return <td key={index} className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(month.lc)} </td>
                                            })}
                                        </tr>
                                        <tr className='table-bordered'>
                                            <td>Unauthorized Salary Deduction</td>
                                            <td className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(_.sumBy(state.payMonths, 'ua'))}</td>
                                            {state.payMonths.map((month, index) => {
                                                return <td key={index} className='text-right'>{NumberUtil.MoneyFormatWithOutDecimal(month.ua)} </td>
                                            })}
                                        </tr>

                                        {state.deductionHeaders.sort((a, b) => a.order - b.order).map((earning, h1) => {
                                            const total = _.sumBy(state.deductions, function (o) {
                                                return o.componentId == earning.id ? o.amount : 0;
                                            });

                                            return <tr className='table-bordered' key={`err${h1}`}>
                                                <td>{earning.name}</td>
                                                <td className="text-right">{NumberUtil.MoneyFormatWithOutDecimal(total)}</td>
                                                {state.payMonths.map((month, index) => {
                                                    const amount = _.sumBy(state.deductions, function (o) {
                                                        return o.componentId == earning.id && o.salaryId == month.id ? o.amount : 0;
                                                    });
                                                    return <td key={index} className="text-right">{NumberUtil.MoneyFormatWithOutDecimal(amount)} </td>
                                                })}
                                            </tr>
                                        })}
                                        <tr className='table-bordered'>
                                            <td>Total</td>
                                            <td className="text-right">{NumberUtil.MoneyFormatWithOutDecimal(_.sumBy(state.payMonths, 'salary'))}</td>
                                            {state.payMonths.map((month, index) => {
                                                return <td key={index} className="text-right">{NumberUtil.MoneyFormatWithOutDecimal(month.salary)} </td>
                                            })}
                                        </tr>
                                    </tbody>
                                </table>
                            </>
                            : <div style={{ marginLeft: '200px', fontSize: '15px' }}>To view your annual statement, please select a financial year.</div>
                        }</>}
                </Card>)
            }
        </Fragment >)
}
export default Annual;