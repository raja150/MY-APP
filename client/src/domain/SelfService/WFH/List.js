import { WFHStatusValues } from 'domain/Approvals/ApplyCompensatory/ConstValue';
import moment from 'moment';
import React from 'react';
import { DateTimePicker } from 'react-widgets';
import { Button, Card, CardBody, Col, Label, Row } from 'reactstrap';
import WorkFromHomeService from 'services/Leave/WorkFromHome';
import DateDetails from 'services/List/DateDetails';
import StatusDetails from 'services/List/StatusDetails';
import DataTable from '../../../components/table/DataTable';
import UpdateTable from '../../HOC/withDataTable';

const title = 'Apply Work From Home'
const fields = [
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
    },

]
// const ddData = { status: { entity: 'status', data: [] } };
// ddData['status'].data = WFHStatusValues;
function WorkFromHomeList(props) {
    const searchCard = () => {
        return (
            <Card className='mb-1'>
                <CardBody>
                    <Row>
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
export default UpdateTable(WorkFromHomeList, WorkFromHomeService.SelfServicePaginate(), fields, '', title, '', '')