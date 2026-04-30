import React, { Fragment, useState } from 'react';
import { CardTitle, Col, Label, Row } from 'reactstrap';
import styled from 'styled-components';
import * as NumberUtil from 'utils/number';

const SalaryDetails = (props) => {
    const TableComponent = styled.table.attrs(() => ({
        className: 'table table-bordered'
    }))`
    td {
        border-right: 1px solid #e9ecef;
        vertical-align: top; 
    }`

    return (
        <Fragment>
            {
                <>
                    <Row>
                        <Col xs='12'>
                            <TableComponent >
                                <tr className='text-center'>
                                    <td colSpan='2'><CardTitle>Salary Details</CardTitle></td>
                                </tr>
                                <tr>
                                    <td md='6'>
                                        <div className='d-flex justify-content-between'>
                                            <Label><strong>{'Name :'}</strong></Label>
                                            <p>{props.paySlip && props.paySlip.name != null ? props.paySlip.name : '-'}</p>
                                        </div>
                                        <div className='d-flex justify-content-between'>
                                            <Label><strong>{'No :'}</strong></Label>
                                            <p>{props.paySlip && props.paySlip.no != null ? props.paySlip.no : '-'}</p>

                                        </div>
                                        <div className='d-flex justify-content-between'>
                                            <Label><strong>{'Designation :'}</strong></Label>
                                            <p>{props.paySlip && props.paySlip.designation != null ? props.paySlip.designation : '-'}</p>

                                        </div>

                                        <div className='d-flex justify-content-between'>
                                            <Label><strong>{'Department :'}</strong></Label>
                                            <p>{props.paySlip && props.paySlip.department != null ? props.paySlip.department : '-'}</p>
                                        </div>

                                        <div className='d-flex justify-content-between'>
                                            <Label><strong>{'PF Number :'}</strong></Label>
                                            <p>{props.paySlip && props.paySlip.epfNo ? props.paySlip.epfNo : '-'}</p>
                                        </div>
                                    </td>
                                    <td>
                                        <div className='d-flex justify-content-between'>
                                            <Label><strong>{'Working Days :'}</strong></Label>
                                            <p>{props.paySlip && props.paySlip.workDays != null ? props.paySlip.workDays : '-'}</p>

                                        </div>
                                        <div className='d-flex justify-content-between'>
                                            <Label><strong>{'Present Days :'}</strong></Label>
                                            <p>{props.paySlip && props.paySlip.presentDays != null ? props.paySlip.presentDays : '-'}</p>

                                        </div>
                                        <div className='d-flex justify-content-between'>
                                            <Label><strong>{'LOP Days :'}</strong></Label>
                                            <p>{props.paySlip && props.paySlip.lopDays != null ? props.paySlip.lopDays : '-'}</p>

                                        </div>

                                        <div className='d-flex justify-content-between'>
                                            <Label><strong>{'Unauthorized Days :'}</strong></Label>
                                            <p>{props.paySlip && props.paySlip.uaDays != null ? props.paySlip.uaDays : '-'}</p>

                                        </div>
                                        <div className='d-flex justify-content-between'>
                                            <Label><strong>{'ESI Number :'}</strong></Label>
                                            <p>{props.paySlip && props.paySlip.esiNo ? props.paySlip.esiNo : '-'}</p>
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td className='w-50'>
                                        <div className='d-flex justify-content-between'>
                                            <p><strong>Earnings</strong></p>
                                            <p><strong>Amount</strong></p>
                                        </div>
                                    </td>
                                    <td className='w-50'>
                                        <div className='d-flex justify-content-between'>
                                            <p><strong>Deductions</strong></p>
                                            <p><strong>Amount</strong></p>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td className='w-50'>
                                        {
                                            props.paySlip.earnings && props.paySlip.earnings.map((e) => {
                                                return (
                                                    <div className='d-flex justify-content-between'>
                                                        <Label><strong>{e.headerName}</strong></Label>
                                                        <p>{e.salary}</p>
                                                    </div>
                                                )
                                            })
                                        }
                                        {props.paySlip.incentive > 0 ? <div className='d-flex justify-content-between'>
                                            <Label><strong>Incentive</strong></Label>
                                            <p>{NumberUtil.MoneyFormat(props.paySlip.incentive)}</p>
                                        </div> : <div />
                                        }
                                        <div className='d-flex justify-content-between'>
                                            <Label><strong>Arrears</strong></Label>
                                            <p>{NumberUtil.MoneyFormat(props.paySlip.arrears)}</p>
                                        </div>
                                    </td>
                                    <td className='w-50'>
                                        {
                                            props.paySlip.deductions && props.paySlip.deductions.map((e) => {
                                                return (
                                                    <div className='d-flex justify-content-between'>
                                                        <Label><strong>{e.headerName}</strong></Label>
                                                        <p>{e.salary}</p>
                                                    </div>
                                                )
                                            })
                                        }
                                        {props.paySlip.payCut > 0 ? <div className='d-flex justify-content-between'>
                                            <Label><strong>Pay Cut</strong></Label>
                                            <p>{NumberUtil.MoneyFormat(props.paySlip.payCut)}</p>
                                        </div> : <div />}
                                        {props.paySlip.lop > 0 ? <div className='d-flex justify-content-between'>
                                            <Label><strong>LOP</strong></Label>
                                            <p>{NumberUtil.MoneyFormat(props.paySlip.lop)}</p>
                                        </div> : <div />}
                                        {props.paySlip.lc > 0 ? <div className='d-flex justify-content-between'>
                                            <Label><strong>Late Coming Deduction</strong></Label>
                                            <p>{NumberUtil.MoneyFormat(props.paySlip.lc)}</p>
                                        </div> : <div />}
                                        {props.paySlip.ua > 0 ? <div className='d-flex justify-content-between'>
                                            <Label><strong>Unauthorized Salary Deduction</strong></Label>
                                            <p>{NumberUtil.MoneyFormat(props.paySlip.ua)}</p>
                                        </div> : <div />}
                                        <div className='d-flex justify-content-between'>
                                            <Label><strong>ESI</strong></Label>
                                            <p>{NumberUtil.MoneyFormat(props.paySlip.esi)}</p>
                                        </div>
                                        <div className='d-flex justify-content-between'>
                                            <Label><strong>PF</strong></Label>
                                            <p>{NumberUtil.MoneyFormat(props.paySlip.epf)}</p>
                                        </div>
                                        <div className='d-flex justify-content-between'>
                                            <Label><strong>Provisional Tax</strong></Label>
                                            <p>{NumberUtil.MoneyFormat(props.paySlip.pTax)}</p>
                                        </div>
                                        <div className='d-flex justify-content-between'>
                                            <Label><strong>Income Tax</strong></Label>
                                            <p>{NumberUtil.MoneyFormat(props.paySlip.tax)}</p>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div className='d-flex justify-content-between'>
                                            <Label ><strong>Gross Earnings</strong></Label>
                                            <p>{NumberUtil.MoneyFormatWithDecimal(props.paySlip.gross)}</p>
                                        </div>
                                    </td>
                                    <td>
                                        <div className='d-flex justify-content-between'>
                                            <Label ><strong>Gross Deductions</strong></Label>
                                            <p>{NumberUtil.MoneyFormatWithDecimal(props.paySlip.deduction)}</p>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td><Label><strong>NET PAY</strong></Label></td>
                                    <td>{NumberUtil.MoneyFormatWithDecimal(props.paySlip.net)}</td>
                                </tr>
                            </TableComponent>
                        </Col>

                    </Row>
                </>
            }
        </Fragment>
    );
}

export default SalaryDetails;