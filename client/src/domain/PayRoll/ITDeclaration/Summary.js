import React, { Fragment, useState } from 'react';
import { Button, Card, CardBody, Col, Row } from 'reactstrap';
import styled from 'styled-components';
import MoneyFormat from 'components/Formats/MoneyFormat'
import * as _ from 'lodash'; 

function DeclarationSummary(props, form) {
    const { declaration } = props; 
    const TableComponent = styled.table`
    width: 100%;
    margin-bottom: 1em;
    border: 1px solid;
    thead th {
        border-bottom: 1px solid;
        padding: 0.55rem;
    }
    td {
        border: none ;
        border-right: 1px solid ;
        border-collapse: collapse;
        padding: 0.55rem;
    }`;

    // if (declaration === undefined || declaration.id === undefined) return <></>;
    return (
         
                    <Row className='mb-5'>
                        <Col xs='12'>
                            {declaration.isNewRegime === true ?
                                <TableComponent>
                                    <thead>
                                        <tr className='text-center'>
                                            <th colSpan='4'>Details Of Salary Paid And Any Other Income and Tax Deducted</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>1.Total Income (8-10)</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.salary} /></td>
                                        </tr>
                                        <tr>
                                            <td>2.Standard Deduction</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.standardDeduction} /></td>
                                        </tr>
                                        <tr>
                                            <td>3.Tax on Total Income</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.tax} /></td>
                                        </tr>
                                        <tr>
                                            <td>4.Education Cess @4% (on tax computed at S.No.12)</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.cess} /></td>
                                        </tr>
                                        <tr>
                                            <td>5.Tax Payable(12+13)</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.taxPayable} /></td>
                                        </tr>
                                        <tr>
                                            <td>6.Less: Relief under Section 89</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.relief} /></td>
                                        </tr>
                                        <tr>
                                            <td>7.Tax Payable(14-15)</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.taxPayable} /></td>
                                        </tr>
                                        <tr>
                                            <td>8.Tax Deducted(12+13)</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.taxPaid} /></td>
                                        </tr>
                                        <tr>
                                            <td>9.Net Tax Payable / Refundable (16-17)</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.due} /></td>
                                        </tr>
                                    </tbody>
                                </TableComponent>
                                :
                                <TableComponent >
                                    <thead>
                                        <tr className='text-center'>
                                            <th colSpan='4'> Details Of Salary Paid And Any Other Income and Tax Deducted</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td className='w-50'>1. Gross Salary </td>
                                            <td className='w-15'></td>
                                            <td className='w-15'></td>
                                            <td className='w-15'></td>
                                        </tr>
                                        <tr>
                                            <td className='pl-3'>(a) Salary as per provision contained in sec.17(1)</td>
                                            <td className='text-right'><MoneyFormat value={declaration.salary} /></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td className='pl-3'>(b) values of perquisites u/s 17(2)(as per Form no 12 BA, Whenever applicable</td>
                                            <td className='text-right'><MoneyFormat value={declaration.perquisites} /></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td className='pl-3'>(c) Profits in lieu of salary u/s 17(3) (as per Form no 12 BA, Whenever applicable</td>
                                            <td className='text-right'><MoneyFormat value={declaration.previousEmployment} /></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td className='pl-3'>(d) Total</td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.totalSalary} /></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>2.Less:Allowance to the extent exempt u/s 10</td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.allowance} /></td>
                                            <td></td>
                                        </tr>
                                        {
                                            declaration.allowances.map((aItem) => {
                                                return <tr>
                                                    <td className='pl-3'>{aItem.name}</td>
                                                    <td></td>
                                                    <td className='text-right'><MoneyFormat value={aItem.amount} /></td>
                                                    <td></td>
                                                </tr>
                                            })
                                        }
                                        <tr>
                                            <td>3.Balance(1-2)[Inclusive of Previous  Employer Sal After Sec 10]</td>
                                            <td />
                                            <td />
                                            <td className='text-right'><MoneyFormat value={declaration.balance} /></td>
                                        </tr>
                                        <tr>
                                            <td>4.Deductions:</td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td className='pl-3'>(a) Standard Deduction</td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.standardDeduction} /></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td className='pl-3'>(b) Entertainment Allowance</td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={0} /></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td className='pl-3'>(c) Tax on Employment [Inclusive of Previous Employer PT] </td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.taxOnEmployment} /></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>5.Aggregate of 4(a) and (b)</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.deductions} /></td>
                                        </tr>
                                        <tr>
                                            <td>6.Income chargeable under the head "Salaries" (3-5)</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.incomeChargeable} /></td>
                                        </tr>
                                        <tr>
                                            <td>7. Add: Any other income reported by the Employee</td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td className='pl-3'>Income from House Property</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.houseIncome} /></td>
                                        </tr>
                                        <tr>
                                            <td className='pl-3'>Other Income</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.otherIncome} /></td>
                                        </tr>
                                        <tr>
                                            <td>8. Gross Total Income(6 + 7)</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.grossTotal} /></td>
                                        </tr>
                                        <tr className='border-top'>
                                            <td>9.Deduction Under chapter VI-A[incl .of Prv Employer Investments]</td>
                                            <td className='text-center'><strong>Gross Amt.</strong></td>
                                            <td className='text-center' />
                                            <td className='text-center'><strong>Deductible Amt.</strong></td>
                                        </tr>
                                        <tr>
                                            <td className='pl-3'>(A) Sections 80C, 80CCC, 80CCD</td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                        {
                                            declaration.epf && declaration.epf > 0 ?
                                                <tr>
                                                    <td className='pl-5'>Provident Fund</td>
                                                    <td className='text-right'><MoneyFormat value={declaration.epf} /></td>
                                                    <td></td>
                                                    <td></td>
                                                </tr> : ''
                                        }
                                        {
                                            declaration.section80CLines.map(eight => {
                                                return <tr>
                                                    <td className='pl-5'>{eight.name}</td>
                                                    <td className='text-right'><MoneyFormat value={eight.amount} /></td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                            })
                                        }
                                        {
                                            declaration.homeLoanPay && declaration.homeLoanPay.principle > 0 ?
                                                <tr>
                                                    <td className='pl-5'>Home Loan Principal Amount</td>
                                                    <td className='text-right'><MoneyFormat value={declaration.homeLoanPay.principle} /></td>
                                                    <td></td>
                                                    <td></td>
                                                </tr> : ''
                                        }
                                        <tr>
                                            <td className='pl-3'>Total</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.eightyC} /></td>
                                        </tr>
                                        <tr>
                                            <td className='pl-3'>(B) Other Section Under Chapter VI-A</td>
                                            <td className='text-center'><strong>Gross Amt.</strong></td>
                                            <td className='text-center'><strong>Qualifying Amt.</strong></td>
                                            <td className='text-center'><strong>Deductible Amt.</strong></td>
                                        </tr>
                                        {
                                            declaration.section80DLines.map(eightyD => {
                                                return <tr>
                                                    <td className='pl-5'>{eightyD.name}</td>
                                                    <td className='text-right'><MoneyFormat value={eightyD.amount} /></td>
                                                    <td className='text-right'><MoneyFormat value={eightyD.qualified} /></td>
                                                    <td className='text-right' />
                                                </tr>
                                            })
                                        }
                                        {
                                            declaration.sectionOtherLines.map(othSec => {
                                                return <tr>
                                                    <td className='pl-5'>{othSec.name}</td>
                                                    <td className='text-right'><MoneyFormat value={othSec.amount} /></td>
                                                    <td className='text-right'><MoneyFormat value={othSec.qualified} /></td>
                                                    <td className='text-right' />
                                                </tr>
                                            })
                                        }
                                        <tr>
                                            <td className='pl-3'>Total</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.eightyD + declaration.otherSections} /></td>
                                        </tr>
                                        <tr>
                                            <td>10.Aggregate Of deductible amount under chapter VI-A</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.eightyC + declaration.eightyD + declaration.otherSections} /></td>
                                        </tr>
                                        <tr>
                                            <td>11.Total Income (8-10)</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.taxable} /></td>
                                        </tr>
                                        <tr>
                                            <td>12.Tax on Total Income</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.tax} /></td>
                                        </tr>
                                        <tr>
                                            <td>13.Education Cess @4% (on tax computed at S.No.12)</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.cess} /></td>
                                        </tr>
                                        <tr>
                                            <td>14.Tax Payable(12+13)</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.taxPayable} /></td>
                                        </tr>
                                        <tr>
                                            <td>15.Less: Relief under Section 89</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.relief} /></td>
                                        </tr>
                                        <tr>
                                            <td>16.Tax Payable(14-15)</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.taxPayable} /></td>
                                        </tr>
                                        <tr>
                                            <td>17.Tax Deducted(12+13)</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.taxPaid} /></td>
                                        </tr>
                                        <tr>
                                            <td>18.Net Tax Payable / Refundable (16-17)</td>
                                            <td></td>
                                            <td></td>
                                            <td className='text-right'><MoneyFormat value={declaration.due} /></td>
                                        </tr>
                                    </tbody>
                                </TableComponent>
                            }
                        </Col>
                    </Row>

               
    );
}

export default DeclarationSummary;












