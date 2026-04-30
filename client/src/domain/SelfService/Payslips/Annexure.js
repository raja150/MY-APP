import React, { Fragment} from 'react';
import { Col, Label, Row } from 'reactstrap';
import styled from 'styled-components';
import * as NumberUtil from 'utils/number';

const Annexure = (props) => {
    const TableComponent = styled.table.attrs(() => ({
        className: 'table table-bordered'
    }))`
    td {
        border-right: 1px solid #e9ecef;
        vertical-align: top; 
    }`
    return (
        <Fragment>
            <Row>
                <p className='font-weight-bold h5 ml-3 mt-3'>Annexure 1</p>
                <Col xs='12'>
                    <p style={{ fontSize: '15px' }}>Please find below details of total incentives and deductions.</p>
                    <TableComponent>
                        <tr>
                            <td className='w-50'>
                                <div className='d-flex justify-content-between'>
                                    <p><strong>Incentives</strong></p>
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
                            {props.incentivesAndPayCut && <td className='w-50'>
                                {props.incentivesAndPayCut.faxFilesAndArrears > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>Fax Files And Arrears</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.faxFilesAndArrears)}</p>
                                </div>}
                                {props.incentivesAndPayCut.sundayInc > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>Sunday Incentives</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.sundayInc)}</p>
                                </div>}
                                {props.incentivesAndPayCut.productionInc > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>Production Incentives</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.productionInc)}</p>
                                </div>}
                                {props.incentivesAndPayCut.spotInc > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>Spot Incentives</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.spotInc)}</p>
                                </div>}
                                {props.incentivesAndPayCut.punctualityInc > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>Punctuality Incentives</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.punctualityInc)}</p>
                                </div>}
                                {props.incentivesAndPayCut.centumClub > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>Centum Club</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.centumClub)}</p>
                                </div>}
                                {props.incentivesAndPayCut.firstMinuteInc > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>First Minute Incentives</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.firstMinuteInc)}</p>
                                </div>}
                                {props.incentivesAndPayCut.doublePay > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>Double Pay</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.doublePay)}</p>
                                </div>}
                                {props.incentivesAndPayCut.nightShift > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>Night Shift</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.nightShift)}</p>
                                </div>}
                                {props.incentivesAndPayCut.weeklyStarInc > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>Weekly Star Incentives</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.weeklyStarInc)}</p>
                                </div>}
                                {props.incentivesAndPayCut.tTeamInc > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>Transition Team Incentives</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.tTeamInc)}</p>
                                </div>}
                                {props.incentivesAndPayCut.otherInc > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>Other Incentives</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.otherInc)}</p>
                                </div>}

                            </td>}
                            {props.incentivesAndPayCut && <td className='w-50'>
                                {props.incentivesAndPayCut.internalQualityFeedbackDed > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>Internal Quality Feedback</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.internalQualityFeedbackDed)}</p>
                                </div>}
                                {props.incentivesAndPayCut.externalQualityFeedbackDed > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>External Quality Feedback</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.externalQualityFeedbackDed)}</p>
                                </div>}
                                {props.incentivesAndPayCut.lateComingDed > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>Late Coming Deductions</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.lateComingDed)}</p>
                                </div>}
                                {props.incentivesAndPayCut.unauthorizedLeaveDed > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>Unauthorized Leave Deduction</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.unauthorizedLeaveDed)}</p>
                                </div>}
                                {props.incentivesAndPayCut.otherDed > 0 && <div className='d-flex justify-content-between'>
                                    <Label><strong>Other Deductions</strong></Label>
                                    <p>{NumberUtil.MoneyFormat(props.incentivesAndPayCut.otherDed)}</p>
                                </div>}
                            </td>}
                        </tr>
                        <tr>
                            <td>
                                <div className='d-flex justify-content-between'>
                                    <Label ><strong>Total Incentives</strong></Label>
                                    {props.incentivesAndPayCut ? <p>{NumberUtil.MoneyFormatWithDecimal(props.incentivesAndPayCut.incentives)}</p> : "₹0.00"}
                                </div>
                            </td>
                            <td>
                                <div className='d-flex justify-content-between'>
                                    <Label ><strong>Total Deductions</strong></Label>
                                    {props.incentivesAndPayCut ? <p>{NumberUtil.MoneyFormatWithDecimal(props.incentivesAndPayCut.payCut)}</p> : "₹0.00"}
                                </div>
                            </td>
                        </tr>
                    </TableComponent>
                </Col>
            </Row>
        </Fragment>
    );
}

export default Annexure;