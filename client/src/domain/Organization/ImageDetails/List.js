import DataTable from 'components/table/DataTable';
import UpdateTable from 'domain/HOC/withDataTable';
import React from 'react';
import { Button, Card, CardBody, Col, Input, Label, Row } from 'reactstrap';
import EmployeeService from 'services/Org/Employee';
import { EmployeeImageCell, EmployeeImageProps } from './ConstImage';

const title = 'Image Details';
const fields = [
    { ...EmployeeImageProps, Cell: ({ row }) => EmployeeImageCell(row.original.id) },
    { name: 'no', label: 'Employee Code', type: 'input' },
    { name: 'name', label: 'Employee Name', type: 'input' },
    { name: 'designation', label: 'Designation', type: 'input' },
    { name: 'department', label: 'Department', type: 'input' },
    {
        name: 'status', label: 'Employee Status', type: 'custom',
        Cell: ({ value}) => {
            return <div>{value === 0 ? "In-Active" : value === 1 ? 'Active' : value === 2 ? 'Resigned' : ''}</div>
        }
    },
];
const ddData = ''

function ImageDetailsList(props) {

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
                            <Button className="mr-2 mt-1 btn-icon btn-icon-only btn-secondary btn-sm"
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
        <DataTable {...props} searchCard={searchCard} hideAdd={true} />
    )
}
export default UpdateTable(ImageDetailsList, { ...EmployeeService.paginate() }, fields, ddData, title, '')
