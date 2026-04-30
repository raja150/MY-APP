import React from 'react';
import DataTable from '../../../components/table/DataTable';
import UpdateTable from '../../HOC/withDataTable';
import { LEAVES_STATUS } from '../navigation';
import { Button, Card, CardBody, Col, Input, Label, Row } from 'reactstrap';
import { DateTimePicker } from 'react-widgets';
import moment from 'moment';
import UnAuthorizedLeavesService from 'services/Org/UnAtuthorizedLeaves';

const title = 'Un-Authorized Leaves'
const fields = [
    { name: 'name', label: 'Employee Name', type: 'input' },
    { name: 'department', label: 'Department', type: 'select' },
    { name: 'designation', label: 'Designation', type: 'select' },
    { name: 'date', label: 'Date', type: 'date' },
    {
        name: 'leaveStatus', label: 'Leave Status', type: 'custom',
        Cell: ({ value, row }) => {
            return value == 1 ? 'Un-Authorized' : 'Authorized'
        }
    },

]

const ddData = { status: { entity: 'status', data: [] } };
ddData['status'].data = LEAVES_STATUS;

function UnAuthorizedLeaves(props) {
    const searchCard = () => {
        return (
            <Card>
                <CardBody>
                    <Row>
                        <Col md='2'>
                            <Label>Employee</Label>
                            <Input name='name' className="form-control form-control-sm"
                                value={props.searchData['name']}
                                onChange={(e) => { props.handleOnChange(e.target.name, e.target.value) }}></Input>
                        </Col>
                        <Col md='2'>
                            <Label>From Date</Label>
                            <DateTimePicker name="fromDate" id="Date"
                                value={props.searchData['fromDate'] ? new Date(props.searchData['fromDate']) : null}
                                onChange={(e) => props.handleOnChange('fromDate', e ? moment(new Date(e)).format('YYYY-MM-DD') : '')}
                                date={true} time={false} />
                        </Col>
                        <Col md='2'>
                            <Label>To Date</Label>
                            <DateTimePicker name="toDate" id="Date"
                                value={props.searchData['toDate'] ? new Date(props.searchData['toDate']) : null}
                                onChange={(e) => props.handleOnChange('toDate', e ? moment(new Date(e)).format('YYYY-MM-DD') : '')}
                                date={true} time={false} />
                        </Col>
                        <Col md='2'>
                            <Label>Leave Status</Label>
                            <Input name='status' type='select' className="form-control form-control-sm"
                                value={props.searchData['status']}
                                onChange={(e) => props.handleOnChange(e.target.name, e.target.value)} >
                                {LEAVES_STATUS.map((item, k) => {
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
        <DataTable {...props} searchCard={searchCard} />
    )

}
export default UpdateTable(UnAuthorizedLeaves, UnAuthorizedLeavesService.paginate(), fields, ddData, title, '', '')