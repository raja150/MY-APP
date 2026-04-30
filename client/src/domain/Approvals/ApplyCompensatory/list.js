import React from 'react';
import { Button, Card, CardBody, Col, Input, InputGroup, Row } from 'reactstrap';
import DateDetails from 'services/List/DateDetails';
import EmployeeDetails from 'services/List/EmployeeDetails';
import StatusDetails from 'services/List/StatusDetails';
import DataTable from '../../../components/table/DataTable';
import ApprovalCompensatoryService from '../../../services/Approval/CompensatoryDayService';
import UpdateTable from '../../HOC/withDataTable';
import { StatusValues } from './ConstValue';
import { EmployeeImageCell, EmployeeImageProps } from 'domain/Organization/ImageDetails/ConstImage';

const title = 'Compensatory Work Day Application'
const onEdit = true;
const type = "CompensatoryWork"
const fields = [
    { ...EmployeeImageProps, Cell: ({ row }) => EmployeeImageCell(row.original.employeeId) },
    {
        name: 'employeeNo', label: 'Employee', type: 'custom', width: 250,
        Cell: ({ value, row }) => {
            const currentRow = row.original
            return (
                <EmployeeDetails currentRow={currentRow} />
            )
        }
    },
    {
        name: 'fromDate', label: 'From Date', type: 'custom',
        Cell: ({ value, row }) => {
            return <DateDetails value={value} row={row} />
        }
    },
    {
        name: 'status', label: 'Status', type: 'custom',
        Cell: ({ value, row }) => {
            return <StatusDetails value={value} row={row}/>
        }
    },
];
const ddData = { status: { entity: 'status', data: [] } };
ddData['status'].data = StatusValues;

const Status = [{ id: '', name: 'select' },
{ id: '1', name: 'Applied' },
{ id: '2', name: 'Approved' },
{ id: '3', name: 'Rejected' }]
function CompensatoryWorkList(props) {
    const searchCard = () => {
        return (
            <Card className="main-card mb-2">
                <CardBody>
                    <Row form className="w-100">
                        <Col md="3">
                            <InputGroup>
                                <label className="text-capitalize mr-2" htmlFor='name'>Employee Name</label>
                                <Input name="name" className="form-control form-control-sm" onChange={(e) => props.handleOnChange(e.target.name, e.target.value)} />
                            </InputGroup>
                        </Col>
                        <Col md='3'>
                            <InputGroup>
                                <label className="text-capitalize mr-2" htmlFor='name'>Status</label>
                                <Input name='status' type='select' className="form-control form-control-sm"
                                    onChange={(e) => props.handleOnChange(e.target.name, e.target.value)} >
                                    {Status.map((item, k) => {
                                        return <option key={k} value={item.id}>{item.name}</option>
                                    })}
                                </Input>
                            </InputGroup>

                        </Col>
                        <Col md="3">
                            <Button className="mr-2  btn-icon btn-icon-only btn-secondary btn-sm" onClick={(e) => props.handleSearch(0)}>
                                <i className="pe-7s-search btn-icon-wrapper"> </i></Button>
                        </Col>
                    </Row>
                </CardBody>
            </Card>
        );
    }
    return (
        <DataTable {...props} searchCard={searchCard} />
    )
}
export default UpdateTable(CompensatoryWorkList, { ...ApprovalCompensatoryService.paginate() }, fields, ddData, title, onEdit, '', type)