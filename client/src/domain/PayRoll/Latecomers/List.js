import React from 'react'
import { Card, CardBody, Col, Input, Label, Row, Button } from 'reactstrap'
import DataTable from '../../../components/table/DataTable';
import UpdateTable from '../../HOC/withDataTable';
import LatecomersService from 'services/PayRoll/Latecomers';
import { EmployeeImageCell, EmployeeImageProps } from 'domain/Organization/ImageDetails/ConstImage';

const title = 'Latecomers'
const fields = [
    { ...EmployeeImageProps, Cell: ({ row }) => EmployeeImageCell(row.original.employeeId) },
    { name: 'employeeNo', label: 'Employee No', type: 'string' },
    { name: 'name', label: 'Employee Name', type: 'string' },
    { name: 'designation', label: 'Designation ', type: 'string' },
    { name: 'department', label: 'Department', type: 'string' },
    { name: 'numberOfDays', label: 'Number Of Days', type: 'int' },

]
function LatecomersList(props) {
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
export default UpdateTable(LatecomersList, { ...LatecomersService.paginate() }, fields, '', title, '', '')