import React from 'react';
import DataTable from '../../../components/table/DataTable';
import UpdateTable from '../../HOC/withDataTable';
import { Button, Card, CardBody, Col, Label, Row } from 'reactstrap';
import { DateTimePicker } from 'react-widgets';
import moment from 'moment';
import HolidaysService from 'services/Leave/HolidaysService'

const title = ''
const onEdit = true;
const fields = [
    { name: 'date', label: 'Date', type: 'date' },
    { name: 'name', label: 'Holiday Name', type: 'input' }
]
const type = ''

function PastHolidaysList(props) {
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
        <DataTable {...props} searchCard={searchCard} hideAdd={true} />
    )

}
export default UpdateTable(PastHolidaysList, HolidaysService.PastHolidaysPaginate(), fields, {}, title, onEdit, "Hide PopUp", type)