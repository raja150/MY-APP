import React from 'react';
import DataTable from 'components/table/DataTable';
import UpdateTable from 'domain/HOC/withDataTable';
import { Button, Card, CardBody, Col, Input, Label, Row } from 'reactstrap';
import PerformanceService from 'services/Org/Performance';
import { PERFORMANCE } from 'Site_constants';
import { DateTimePicker } from 'react-widgets';
import moment from 'moment';


const title = 'Star Employees';
const fields = [
    { name: 'employeeNo', label: 'Employee Code', type: 'input' },
    { name: 'name', label: 'Employee Name', type: 'input' },
    {
        name: 'performanceType', label: 'Performance Type', type: 'custom',
        Cell: ({ value }) => {
            return (
                <div >
                    {value == 2 ? 'Weekly' : 'Monthly'}
                </div>
            )
        }
    },
    { name: 'performedDate', label: 'Performed Date', type: 'date' },
];
const ddData = ''

function EmployeePerformanceList(props) {

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
                        <Col>
                            <Label>From Date</Label>
                            <DateTimePicker name="fromDate" id="Date"
                                value={props.searchData['fromDate'] ? new Date(props.searchData['fromDate']) : null}
                                onChange={(e) => props.handleOnChange('fromDate', e ? moment(new Date(e)).format('YYYY-MM-DD') : '')}
                                date={true}
                                time={false}
                            />
                        </Col>
                        <Col>
                            <Label>To Date</Label>
                            <DateTimePicker name="toDate" id="Date"
                                value={props.searchData['toDate'] ? new Date(props.searchData['toDate']) : null}
                                onChange={(e) => props.handleOnChange('toDate', e ? moment(new Date(e)).format('YYYY-MM-DD') : '')}
                                date={true}
                                time={false}
                            />
                        </Col>
                        <Col md="3">
                            <Label>Performance Type</Label>
                            <Input name='performanceType' type='select' className="form-control form-control-sm"
                                value={props.searchData['performanceType']}
                                onChange={(e) => props.handleOnChange(e.target.name, e.target.value)} >
                                {PERFORMANCE.map((item, k) => {
                                    return <option key={k} value={item.value}>{item.text}</option>
                                })}
                            </Input>
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
export default UpdateTable(EmployeePerformanceList, { ...PerformanceService.paginate() }, fields, ddData, title, '')
