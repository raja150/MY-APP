import React from 'react';
import DataTable from 'components/table/DataTable';
import UpdateTable from 'domain/HOC/withDataTable';
import { Button, Card, CardBody, Col, Input, Label, Row } from 'reactstrap';
import { DateTimePicker } from 'react-widgets';
import moment from 'moment';
import { LEAVE_STATUS } from 'Site_constants';
import ApplyLeavesService from 'services/Leave/ApplyLeave'
import EmployeeDetails from 'services/List/EmployeeDetails'
import DateDetails from 'services/List/DateDetails';
import LeaveTypeDetails from 'services/List/LeaveTypeDetails';
import StatusDetails from 'services/List/StatusDetails';
import { Radio } from 'components/dynamicform/Controls';
import { EmployeeImageCell, EmployeeImageProps } from 'domain/Organization/ImageDetails/ConstImage';
const fields =
    [
        { ...EmployeeImageProps, Cell: ({ row }) => EmployeeImageCell(row.original.employeeId) },
        {
            name: 'employeeNo', label: 'Employee', type: 'custom', width: 200,
            Cell: ({ value, row }) => {
                const currentRow = row.original
                return (
                    <EmployeeDetails currentRow={currentRow} />
                )
            }
        },
        {
            name: 'leaveType', label: 'Leave Type - Day(s)', type: 'custom',
            Cell: ({ value, row }) => {
                const currentRow = row.original
                return (
                    <LeaveTypeDetails currentRow={currentRow} />
                )
            }
        },
        {
            name: 'fromDate', label: 'Date', type: 'custom',
            Cell: ({ value, row }) => {
                return <DateDetails value={value} row={row} />
            }
        },
        { name: 'reportingTo', label: 'Reporting To', type: 'input' },
        {
            name: 'status', label: 'Status', type: 'custom',
            Cell: ({ value, row }) => {
                return <StatusDetails value={value} row={row} />
            }
        },
        {
            name: 'isPlanned', label: 'Planned Leave', type: 'custom',
            Cell: ({ value, row }) => {
                return value == true ? 'Yes' : 'No'
            }
        }
    ]

const type = 'Approve';
const onEdit = true;

function LeavesList(props) {
    const searchCard = () => {
        return (
            <Card className='mb-1'>
                <CardBody>
                    <Row>
                        <Col md="2">
                            <Label><strong>Employee</strong></Label>
                            <Input name='name' className="form-control form-control-sm"
                                value={props.searchData['name']}
                                onChange={(e) => props.handleOnChange(e.target.name, e.target.value)} />
                        </Col>
                        <Col md='2'>
                            <Label>From Date</Label>
                            <DateTimePicker name="fromDate" id="Date"
                                value={props.searchData['fromDate'] ? new Date(props.searchData['fromDate']) : null}
                                onChange={(e) => props.handleOnChange('fromDate', e ? moment(new Date(e)).format('YYYY-MM-DD') : '')}
                                date={true}
                                time={false}
                            />
                        </Col>
                        <Col md='2'>
                            <Label>To Date</Label>
                            <DateTimePicker name="toDate" id="Date"
                                value={props.searchData['toDate'] ? new Date(props.searchData['toDate']) : null}
                                onChange={(e) => props.handleOnChange('toDate', e ? moment(new Date(e)).format('YYYY-MM-DD') : '')}
                                date={true}
                                time={false}
                            />
                        </Col>
                        <Col md='2'>
                            <Label>Status</Label>
                            <Input name='status' type='select' className="form-control form-control-sm"
                                value={props.searchData['status']}
                                onChange={(e) => props.handleOnChange(e.target.name, e.target.value)} >
                                {LEAVE_STATUS.map((item, k) => {
                                    return <option key={k} value={item.value}>{item.text}</option>
                                })}
                            </Input>
                        </Col>
                        <Col md='2'>
                            <Radio {...{
                                name: 'isPlanned', value: props.searchData.isPlanned ? props.searchData.isPlanned : false,
                                values: [{ value: 1, text: 'Planned' }, { value: 2, text: 'Unplanned' }],
                            }} handlevaluechange={(name, value) => props.handleOnChange(name, value)} />

                        </Col>
                        <Col md="1" className='mt-3'>
                            <Button className="mr-3  btn-icon btn-icon-only btn-secondary btn-sm"
                                onClick={(e) => props.handleSearch(0)} style={{ marginTop: '5px' }}>
                                <i className="pe-7s-search btn-icon-wrapper">
                                </i>
                            </Button>
                        </Col>
                    </Row>
                </CardBody>
            </Card>
        )
    }

    return (
        <DataTable {...props} searchCard={searchCard} />
    )

}
export default UpdateTable(LeavesList, ApplyLeavesService.LMApprovalPaginate(),
    fields, {}, 'Approve Leaves', onEdit, '', type)