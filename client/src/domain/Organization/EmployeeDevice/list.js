import DataTable from 'components/table/DataTable';
import UpdateTable from 'domain/HOC/withDataTable';
import React from 'react';
import { Button, Card, CardBody, Col, Input, Label, Row } from 'reactstrap';
import EmployeeDeviceService from 'services/Org/EmployeeDevice';


const title = 'Employee Device';
const fields = [
    { name: 'employeeNo', label: 'Employee Code', type: 'input' },
    { name: 'name', label: 'Employee Name', type: 'input' },
    { name: 'department', label: 'Department', type: 'input' },
    { name: 'designation', label: 'Designation', type: 'input' },
    { name: 'mobileNumber', label: 'Mobile Number', type: 'input' },
    {
        name: 'computerType', label: 'Computer Type', type: 'custom',
        Cell: ({ value, row }) => {
            return <div>{value === 1 ? "Office Desktop" : value === 2 ? 'Office Laptop' : value === 3 ? 'BOYD' :
                value === 4 ? "BOYD RDP System" : value === 5 ? 'Office RDP System' : value === 6 ? 'Working from Office' : ''}</div>
        }
    },
    { name: 'hostName', label: 'Host Name', type: 'input' },
    {
        name: 'isActZeroInstalled', label: 'Is Act Zero Installed', type: 'custom',
        Cell: ({ value, row }) => {
            return <div>{value ? "Yes" : "No"}</div>
        }
    },
    {
        name: 'isK7Installed', label: 'Is K7 Installed', type: 'custom',
        Cell: ({ value, row }) => {
            return <div>{value ? "Yes" : "No"}</div>
        }
    },
    {
        name: 'isUninstalled', label: 'Is Uninstalled', type: 'custom',
        Cell: ({ value, row }) => {
            return <div>{value ? "Yes" : "No"}</div>
        }
    },
    { name: 'installedOn', label: 'Installed On', type: 'dateTime' },
    { name: 'uninstalledOn', label: 'Uninstalled On', type: 'dateTime' },
];
const ddData = ''

function EmployeeDeviceList(props) {

    const searchCard = () => {
        return (
            <Card className="main-card mb-2">
                <CardBody>
                    <Row form className="w-100">
                        <Col md="3">
                            <Label>Employee Name</Label>
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
        <DataTable {...props} searchCard={searchCard} />
    )
}
export default UpdateTable(EmployeeDeviceList, { ...EmployeeDeviceService.paginate() }, fields, ddData, title, '')
