import React from 'react'
import { Card, CardBody, Col, Input, Label, Row, Button } from 'reactstrap'
import DataTable from '../../../components/table/DataTable';
import UpdateTable from '../../HOC/withDataTable';
import { StatusValues } from '../Leave/ConstValues'
import { DateTimePicker } from 'react-widgets';
import ApplyClientVisitsService from 'services/Leave/ApplyClientVisits'
import moment from 'moment';
import { EmployeeImageCell, EmployeeImageProps } from 'domain/Organization/ImageDetails/ConstImage';
import DateDetails from 'services/List/DateDetails';
import EmployeeDetails from 'services/List/EmployeeDetails';
import StatusDetails from 'services/List/StatusDetails';
import { CLIENT_VISIT_STATUS } from 'Site_constants';
// import ApplyClientVisitsService from 'services/Leave/ApplyClientVisits'

const fields = [
    { ...EmployeeImageProps, Cell: ({ row }) => EmployeeImageCell(row.original.employeeId) },
    { name: 'placeOfVisit', label: 'Place of Visit', type: 'input' },
    {
        name: 'employeeId', label: 'Employee', type: 'custom', width: 150,
        Cell: ({ value, row }) => {
            const currentRow = row.original
            return (
                <EmployeeDetails currentRow={currentRow} />
            )
        }
    },
    {
        name: 'fromDate', label: 'Date', type: 'custom',
        Cell: ({ value, row }) => {
            return <DateDetails value={value} row={row} />
        }
    },
    {
        name: 'status', label: 'Status', type: 'custom',
        Cell: ({ value, row }) => {
            return <StatusDetails value={value} row={row} />
        }
    }
]
const title = 'Client Visit';
const type = "ClientVisit";

const ddData = { status: { entity: 'status', data: [] } };
ddData['status'].data = StatusValues;

const onEdit = true;
function ClientVisitList(props) {

    const searchCard = () => {
        return (
            <Card>
                <CardBody>
                    <Row>
                        <Col md='3'>
                            <Label>Employee</Label>
                            <Input name='name' className="form-control form-control-sm"
                                value={props.searchData['name']}
                                onChange={(e) => { props.handleOnChange(e.target.name, e.target.value) }}></Input>
                        </Col>
                        <Col md='3'>
                            <Label>From Date</Label>
                            <DateTimePicker name="fromDate" id="Date"
                                value={props.searchData['fromDate'] ? new Date(props.searchData['fromDate']) : null}
                                onChange={(e) => props.handleOnChange('fromDate', e ? moment(new Date(e)).format('YYYY-MM-DD') : '')}
                                date={true}
                                time={false}
                            />
                        </Col>
                        <Col md='3'>
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
                                {CLIENT_VISIT_STATUS.map((item, k) => {
                                    return <option key={k} value={item.value}>{item.text}</option>
                                })}
                            </Input>
                        </Col>
                        <Col md="1" className='mt-3'>
                            <Button className="mr-2  btn-icon btn-icon-only btn-secondary btn-sm" onClick={(e) => props.handleSearch(0)}>
                                <i className="pe-7s-search btn-icon-wrapper"> </i></Button>
                        </Col>
                    </Row>
                </CardBody>
            </Card>
        )
    }
    return (
        <DataTable {...props} searchCard={searchCard} hideAdd={true} />
    )

}
export default UpdateTable(ClientVisitList, ApplyClientVisitsService.ApprovalPaginate(), fields, ddData, title, onEdit, '', type)