import DataTable from 'components/table/DataTable';
import UpdateTable from 'domain/HOC/withDataTable';
import React from 'react';
import { Button, Card, CardBody, Col, Input, Label, Row } from 'reactstrap';
import UserService from 'services/User';
import { EmployeeImageProps, EmployeeImageCell } from '../ImageDetails/ConstImage';


const title = 'Users';
const fields = [
    { ...EmployeeImageProps, Cell: ({ row }) => EmployeeImageCell(row.original.employeeID) },
    { name: 'empCode', label: 'Employee Code', type: 'input', id: 'Employee.No' },
    { name: 'empName', label: 'Employee Name', type: 'input', id: 'Employee.Name' },
    { name: 'designation', label: 'Designation', type: 'input', id: 'Employee.Designation.Name' },
    { name: 'department', label: 'Department', type: 'input', id: 'Employee.Department.Name' },
    { name: 'role', label: 'Role', type: 'input',id:'Role.Name' },
    { name: 'name', label: 'User Name', type: 'input' },
    { name: 'lastLogin', label: 'Last login', type: 'dateTime' },
    { name: 'expireOn', label: 'Password expire on', type: 'dateTime' },
];
const ddData = ''

function UsersList(props) {

    const searchCard = () => {
        return (
            <Card className="main-card mb-2">
                <CardBody>
                    <Row form className="w-100">
                        <Col md="3">
                            <Label>Employee Name / User Name</Label>
                            <Input name='name' className="form-control form-control-sm"
                                value={props.searchData['name']}
                                onChange={(e) => props.handleOnChange(e.target.name, e.target.value)} />
                        </Col>
                        <Col md="1" className='mt-3'>
                            <Button className="mr-2  mt-1 btn-icon btn-icon-only btn-secondary btn-sm"
                                onClick={(e) => props.handleSearch(0)}>
                                <i className="pe-7s-search btn-icon-wrapper" />
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
export default UpdateTable(UsersList, { ...UserService.paginate() }, fields, ddData, title, '')
